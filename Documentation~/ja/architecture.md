# アーキテクチャ

[English](../architecture.md)

仮想ツリーがどのように生きた `VisualElement` 階層に変換され、同期が保たれるか。

## データフロー

```
new VeautyElement<State>(mounter, renderFunc, initialState)
        │
        ▼
┌─ 初回マウント (コンストラクタ内で同期実行) ─────────────────────┐
│  1. renderFunc(state, setState)      → IVTree                  │
│  2. hookRuntime.Resolve<VisualElement>(tree)                   │
│       [Component] 関数コンポーネントを解決し hooks を実行       │
│  3. Renderer.Render(resolvedTree)    → VisualElement           │
│  4. mounter.Add(rootElement); rootElement.style.flexGrow = 1   │
│  5. hookRuntime.CommitEffects()      (UseEffect コールバック)   │
└─────────────────────────────────────────────────────────────────┘
        │  setState / hook の state 変更 / ForceUpdate()
        ▼
┌─ 更新 ──────────────────────────────────────────────────────────┐
│  1. renderFunc(state, setState)      → 新しい IVTree            │
│  2. hookRuntime.Resolve<VisualElement>(newTree)                 │
│  3. Diff<VisualElement>.Calc(oldResolvedTree, newResolvedTree)  │
│                                       → IPatch<VisualElement>[] │
│  4. Patch.Apply(rootElement, oldResolvedTree, patches)          │
│  5. oldResolvedTree = newResolvedTree                           │
│  6. hookRuntime.CommitEffects()                                 │
└─────────────────────────────────────────────────────────────────┘
```

コアパッケージ (`com.uzimaru.veauty`) はツリーの型 (`Node`, `KeyedNode`, `FunctionComponentNode`)、hook ランタイム、汎用 diff を提供します。本パッケージは `VisualElement` 専用のレンダラー (`Render.cs`) とパッチ適用 (`Patch.cs`)、およびコントロール/属性レイヤーを提供します。

## 再レンダリングのスケジューリング

`VeautyElement<State>` は `isRendering` フラグで再入を防ぎます:

- レンダリング**実行中**の state 設定 (または hook からの再レンダリング要求) はネストしたレンダリングを開始しません。代わりに `renderRequested` フラグが立ちます。
- 現在のレンダリングが完了すると `FlushPendingRender` がちょうど1回の追加レンダリングを実行します。1回のレンダリング中の複数回の state 書き込みは、その1回のレンダリングに**まとめられます**。
- `ForceUpdate()` も同じルールに従います: レンダリング中 (またはマウント完了前) に呼ばれた場合は遅延され、それ以外の場合は state が変わっていなくても即座に再レンダリングします。

## Renderer

`Renderer.Render(IVTree)` は新しい `VisualElement` サブツリーを生成します:

1. `IVTreeWrapper` (インデクサを使っていない Presets の `Element` など) はまず unwrap されます。
2. 素の `FunctionComponentNode` は**使い捨ての新しい** `HookRuntime` で解決されます — この経路では hook の state はレンダリング間で保持されません。`VeautyElement` はレンダリングや diff の*前に*、永続的なランタイムでツリー全体を解決することでこれを回避しています。
3. 解決済みノードの処理順:
   - **生成** — `ITypedNode.GetComponentType()` を `Activator.CreateInstance` でインスタンス化 (`VisualElement` を継承し、引数なしコンストラクタが必要)。型指定のないノードはプレーンな `VisualElement` になります。要素の `name` はノードの tag です。
   - **Init** — 各 `IHostLifecycle<VisualElement>.Init(ve)` を実行 (`UIBase<T>` ノードは自身が唯一のライフサイクルハンドラ)。
   - **ApplyAttrs** — すべての属性の `Apply(ve)` を実行。
   - **RenderKids** — 子を再帰的にレンダリングして `Add`。
   - **AfterRenderKids** — 各ライフサイクルの `AfterRenderKids(ve)` を実行。

それ以外の `IVTree` 型は `ArgumentException` を送出します。

## パッチ適用

`Patch.Apply(rootElement, oldVTree, patches)`:

1. **ターゲットの紐付け** — 旧仮想ツリーを実際の階層と並行して走査し、(ツリーインデックスで識別される) 各パッチを `SetTarget` で対象の `VisualElement` に紐付けます。コントロールが自前の内部要素を作った場合 (例: `ScrollView` のスクローラー) は `childOffset = element.childCount - kids.Length` (0 でクランプ) で仮想の子を実際の子にマッピングします。
2. **ライフサイクルの破棄** — `Redraw`、`Remove` (move エントリなし)、`RemoveLast` では、要素が切り離される前に、削除されるサブツリーに対して `IHostLifecycle.Destroy` が子から先に呼ばれます。
3. **適用** — 各パッチが階層を変更します:
   - `Attrs` — 変更された属性を再適用。`null` のエントリ (ツリーから削除された属性) は**スキップ**されるため、削除された属性の最後の値は要素に残ります (リセットされないセマンティクス)。
   - `Redraw` — 新しいサブツリーをレンダリングし、同じインデックスで差し替え。
   - `Append` / `RemoveLast` — 末尾の子の追加/削除。
   - `Remove` / `Reorder` — キー付きリストの削除・挿入・移動 (移動された要素は再生成ではなく付け替え)。
   - `Attach` — 要素を別の `VisualElement` 型の新しいインスタンスに差し替え (`name` は維持)。

パッチがルート要素を差し替えた場合、`Apply` は新しいルートを返し、`VeautyElement` がそれを保持します。

## ホストライフサイクルの順序

`UIBase<T>` ノードでは: `Init` → 属性 → 子 → `AfterRenderKids`。要素が削除または redraw されるときに `Destroy` (破棄時は子から先に呼び出し)。3つともデフォルトは no-op です。

## uGUI パッケージとの比較

両パッケージは同じコア (`Diff`, `HookRuntime`, ツリー型) を使いますが、ホストレイヤーの構造が異なります:

| | `com.uzimaru.veauty-ugui` | `com.uzimaru.veauty-uitoolkit` |
|---|---|---|
| ホストオブジェクト | `GameObject` + コンポーネント | `VisualElement` (要素そのものがコントロール) |
| ノード → ホスト | `AttachComponent` が GameObject に型 `U` の `Component` を追加 | `VisualElement` サブクラス自体を `Activator.CreateInstance` |
| 型付き属性の不一致時 | `GuiAttributeBase` が `TComponent` を検索。一部のコンポーネント型は欠けていると**自動追加** (LayoutElement, CanvasGroup など) | `UIAttributeBase<TElement, TValue>` は**黙って no-op** — コンポーネントモデルがないため何も自動追加されない |
| レイアウト | Layout Group コンポーネント + RectTransform 属性 | インライン USS スタイル (`StyleAttribute<T>`) による Flexbox |
| 内部構造 | 複雑な widget は明示的なサブコンポーネントが必要 (`Slider.Fill()`, `Toggle.Checkmark()` など) を `Init` で読み取る | UI Toolkit コントロールは内部を自前で構築。`ISubComponent`/`FindPart<T>()` は互換のために存在するが**組み込みコントロールでサブコンポーネントを定義するものはない** |
| スタイリング | コンポーネントプロパティごとの属性 | USS クラス (`ClassName`/`ClassList`)、スタイルシート (`StyleSheetAttr`)、インラインスタイル |
| エントリーポイント | `VeautyObject<State>` (GameObject にマウント) | `VeautyElement<State>` (VisualElement にマウント) |

内部の子をスキップする `childOffset` の仕組みは両方のパッチ適用に存在します。本パッケージでは UI Toolkit コントロールが自身のために作る要素もカバーします。

## 関連ページ

- [要素](elements.md) — エントリーポイントとコントロール一覧。
- [API リファレンス](api-reference.md) — 完全なシグネチャ。
