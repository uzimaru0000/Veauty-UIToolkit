# 要素

[English](../elements.md)

マウントポイント `VeautyElement<State>`、`UIBase<T>` コントロール一覧、`Veauty.UIToolkit.Presets` 便利レイヤー。

## VeautyElement&lt;State&gt;

`Veauty.UIToolkit.VeautyElement<State>` (`State : struct`) は任意の `VisualElement` に仮想ツリーをマウントし、state 変更時に再レンダリングします。コンストラクタで**同期的に**レンダリングとマウントが行われ、レンダリングされたルートはマウント要素の子として追加され、マウント領域を満たすように `style.flexGrow = 1` が設定されます。

### renderFunc の3つのオーバーロード

```csharp
// 1. 値セッター: setState が state 全体を置き換える
new VeautyElement<S>(mounter, (S state, Action<S> setState) => tree, initial);

// 2. アップデータ: setState が State => State 関数を取る
new VeautyElement<S>(mounter, (S state, Action<Func<S, S>> setState) => tree, initial);

// 3. state のみ: 再レンダリングは hooks か ForceUpdate() から
new VeautyElement<S>(mounter, (S state) => tree, initial);
```

オーバーロード 2 の注意: 更新関数は、アップデータ呼び出し時点の state ではなく、**レンダー関数実行時点**の state (キャプチャされた `s`) に適用されます。同じレンダリングから発火した2つのアップデータは同じスナップショットから始まります。

### ForceUpdate

`ForceUpdate()` は state が変わっていなくてもレンダー関数を再実行し diff を適用します — ツリーが Veauty の観測できない外部データに依存している場合に有用です。マウント完了前やレンダリング中に呼ばれた場合は即時実行されず遅延されます。

### 再入とまとめ (coalescing)

レンダリング**中**の state 書き込み (`setState`、hooks、`ForceUpdate`) はネストしたレンダリングを開始しません。保留フラグが立ち、現在のレンダリング完了後にちょうど1回の追加レンダリングが実行されます。1回のレンダリング内の何回の state 書き込みも、その1回にまとめられます。

## UIBase&lt;T&gt; コントロール一覧

`Veauty.UIToolkit` の各コントロールは `UIBase<T> : Node<VisualElement, T>` で、tag は `typeof(T).FullName` です。属性はネストクラスとして定義され、各コントロールにはその要素型を対象とするカスタム属性を書くための抽象基底 `XxxAttribute<T>` もあります (対象以外の要素では黙って no-op)。

| コントロールノード | レンダリング先 | 属性 (ネストクラス) |
|---|---|---|
| `Button` | `UnityEngine.UIElements.Button` | `Text`, `OnClick` |
| `DropdownField` | `UnityEngine.UIElements.DropdownField` | `Value`*, `Label`, `Choices`, `Index`, `OnValueChanged` |
| `Foldout` | `UnityEngine.UIElements.Foldout` | `Text`, `Value`* |
| `Label` | `UnityEngine.UIElements.Label` | `Text`, `EnableRichText` |
| `ProgressBar` | `UnityEngine.UIElements.ProgressBar` | `Value`, `Title`, `LowValue`, `HighValue` |
| `ScrollView` | `UnityEngine.UIElements.ScrollView` | `Mode`, `HorizontalScrollerVisibility`, `VerticalScrollerVisibility`, `Elasticity` |
| `Slider` | `UnityEngine.UIElements.Slider` | `Value`*, `LowValue`, `HighValue`, `Direction`, `ShowInputField`, `Label`, `OnValueChanged` |
| `TextField` | `UnityEngine.UIElements.TextField` | `Value`*, `Label`, `IsReadOnly`, `Multiline`, `MaxLength`, `Placeholder`, `OnValueChanged` |
| `Toggle` | `UnityEngine.UIElements.Toggle` | `Value`*, `Label`, `OnValueChanged` |

`*` = `SetValueWithoutNotify` で適用されるため、値の設定でコントロールの変更イベントは発火しません ([イベント](events.md) を参照)。`DropdownField.Index` は素の `index` 代入なので通知**されます**。

基本レイヤーでの使い方 (明示的な属性リスト):

```csharp
using Veauty.UIToolkit;

new Slider(new IAttribute<VisualElement>[] {
    new Slider.LowValue(0f),
    new Slider.HighValue(1f),
    new Slider.Value(0.5f),
    new Slider.OnValueChanged(evt => Debug.Log(evt.newValue)),
})
```

`UIBase<T>` はホストライフサイクル (`Init`/`AfterRenderKids`/`Destroy`、すべて virtual no-op) も実装し、`ISubComponent` の子を探す `FindPart<T>()` を提供します。現在、組み込みコントロールでサブコンポーネントを使うものはありません — UI Toolkit コントロールは内部要素を自前で作ります。

## Presets: V ファクトリ

`Veauty.UIToolkit.Presets.V` は基本コントロールを名前付きオプション引数のファクトリメソッドでラップします。省略した引数は属性を一切追加せず、コントロール自身のデフォルトが残ります。すべてのメソッドは末尾に `params IAttribute<VisualElement>[] extras` を持ち、スタイル・クラス・その他の属性を渡せます。

| ファクトリ | 戻り値 | 引数 (注記がなければすべてオプション) |
|---|---|---|
| `V.Label(text, className, extras)` | `IVTree` | `text` は必須 |
| `V.Button(text, onClick, className, extras)` | `Element` | |
| `V.TextField(value, label, placeholder, multiline, isReadOnly, maxLength, onValueChanged, className, extras)` | `Element` | |
| `V.Toggle(value, label, onValueChanged, className, extras)` | `Element` | |
| `V.Slider(value, lowValue, highValue, label, direction, showInputField, onValueChanged, className, extras)` | `Element` | `value` はレンジ属性の後に適用 |
| `V.ScrollView(mode, horizontalScrollerVisibility, verticalScrollerVisibility, className, extras)` | `Element` | |
| `V.Foldout(text, value, className, extras)` | `Element` | |
| `V.DropdownField(value, label, choices, index, onValueChanged, className, extras)` | `Element` | `value` は `choices`/`index` の後に適用 |
| `V.ProgressBar(value, title, lowValue, highValue, className, extras)` | `IVTree` | `value` はレンジ属性の後に適用 |
| `V.Box(className, extras)` | `Element` | プレーンな `VisualElement`、tag "Box" |
| `V.Row(className, extras)` | `Element` | `flex-direction: row` コンテナ |
| `V.Column(className, extras)` | `Element` | `flex-direction: column` コンテナ |

ヘルパー:

- `V.Children(params IVTree[])` / `V.Children(IEnumerable<IVTree>)` — `null` エントリを除外。条件付きの子 (`condition ? node : null`) に便利。
- `V.Classes(params string[])` / `V.Classes(IEnumerable<string>)` — null/空白のみの名前を除外して `ClassList` を作成。

```csharp
V.Column()[
    V.Children(
        V.Label("Always"),
        showDetails ? V.Label("Details") : null
    )
]
```

## Element ビルダー

子を取れる `V` メソッドは `Element` (`IVTreeWrapper`) を返します。インデクサで子を渡します:

```csharp
V.Row()[ V.Label("Left"), V.Label("Right") ]          // params IVTree[]
V.Column()[ items.Select(RenderItem) ]                 // IEnumerable<IVTree> オーバーロード
```

どちらのインデクサも呼び出しごとに新しいノードを生成します。インデクサを**使わずに** `IVTree` として直接使うと、初回アクセス時に子なしのノードが遅延生成されてキャッシュされます — そのため `V.Button("OK", onClick)` 単体でも有効な葉ノードです。

## 関連ページ

- [スタイリング](styling.md) — `extras` に渡すスタイル属性とクラス。
- [イベント](events.md) — 再レンダリング時の `onClick`/`onValueChanged` の挙動。
- [API リファレンス](api-reference.md) — 完全なシグネチャ。
