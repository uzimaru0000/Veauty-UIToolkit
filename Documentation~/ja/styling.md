# スタイリング

[English](../styling.md)

インラインスタイル属性、fluent な `StyleBuilder`、USS クラス管理、スタイルシート。

## インラインスタイル属性 (`StyleAttribute<TValue>`)

以下のクラスはすべて `StyleAttribute<TValue>` を継承し、`VisualElement.style` のインライン USS スタイルプロパティを1つ (またはグループで) 書き込みます。**任意の**要素に適用できます。

| カテゴリ | 属性 | 値の型 | 設定先 |
|---|---|---|---|
| Flex | `FlexGrow` | `float` | `flexGrow` |
| Flex | `FlexShrink` | `float` | `flexShrink` |
| Flex | `FlexDirection` | `UnityEngine.UIElements.FlexDirection` | `flexDirection` |
| Flex | `FlexWrap` | `Wrap` | `flexWrap` |
| 整列 | `JustifyContent` | `Justify` | `justifyContent` |
| 整列 | `AlignItems` | `Align` | `alignItems` |
| 整列 | `AlignSelf` | `Align` | `alignSelf` |
| サイズ | `Width`, `Height` | `StyleLength` | `width` / `height` |
| サイズ | `MinWidth`, `MinHeight` | `StyleLength` | `minWidth` / `minHeight` |
| サイズ | `MaxWidth`, `MaxHeight` | `StyleLength` | `maxWidth` / `maxHeight` |
| マージン | `MarginTop`, `MarginBottom`, `MarginLeft`, `MarginRight` | `StyleLength` | 各辺のマージン |
| パディング | `PaddingTop`, `PaddingBottom`, `PaddingLeft`, `PaddingRight` | `StyleLength` | 各辺のパディング |
| 色 | `BackgroundColor` | `Color` | `backgroundColor` |
| 色 | `TextColor` | `Color` | `color` (diff キーは `"Color"`) |
| 色 | `Opacity` | `float` | `opacity` |
| ボーダー | `BorderColor` | `Color` | 4辺すべての `border*Color` |
| ボーダー | `BorderWidth` | `float` | 4辺すべての `border*Width` |
| ボーダー | `BorderRadius` | `StyleLength` | 4隅すべての角丸 |
| テキスト | `FontSize` | `StyleLength` | `fontSize` |
| レイアウト | `Display` | `DisplayStyle` | `display` |
| レイアウト | `Overflow` | `UnityEngine.UIElements.Overflow` | `overflow` |
| 配置 | `Position` | `UnityEngine.UIElements.Position` | `position` |
| 配置 | `Top`, `Bottom`, `Left`, `Right` | `StyleLength` | 対応するオフセット |

```csharp
using Veauty.UIToolkit;

V.Box(extras: new IAttribute<VisualElement>[] {
    new BackgroundColor(Color.gray),
    new PaddingTop(new StyleLength(16)),
    new BorderRadius(new StyleLength(8)),
})
```

> **注意 — 削除してもリセットされない。** レンダリング間で属性がツリーから消えた場合、diff はその属性に `null` エントリを出力し、パッチ適用は `null` をスキップします。そのためスタイルプロパティは**最後の値を保持**し、USS/デフォルト値にはリセットされません。スタイルを「外す」には、中立の値を持つ属性を明示的に適用してください (もしくは USS クラスで見た目を制御してください)。

## StyleBuilder (fluent API)

`Style.Set` が新しい `StyleBuilder` を開始します。各呼び出しは属性を1つ追加します (`Margin`/`Padding` ショートハンドは4つ)。`Build()` は `IAttribute<VisualElement>[]` を返し、ビルダーはその配列型へ暗黙変換もできます。

```csharp
using Veauty.UIToolkit;

V.Box(extras: Style.Set
    .FlexDirection(UnityEngine.UIElements.FlexDirection.Row)
    .Padding(new StyleLength(16))       // 4辺すべて
    .BackgroundColor(Color.gray)
    .BorderRadius(new StyleLength(8))
    .Build())
```

ビルダーメソッドは上記の属性一覧に対応します: `FlexGrow`, `FlexShrink`, `FlexDirection`, `FlexWrap`, `JustifyContent`, `AlignItems`, `AlignSelf`, `Width`, `Height`, `MinWidth`, `MinHeight`, `MaxWidth`, `MaxHeight`, `MarginTop/Bottom/Left/Right`, `Margin` (全辺), `PaddingTop/Bottom/Left/Right`, `Padding` (全辺), `BackgroundColor`, `TextColor`, `Opacity`, `BorderColor`, `BorderWidth`, `BorderRadius`, `FontSize`, `Display`, `Overflow`, `Position`, `Top`, `Bottom`, `Left`, `Right`。

## USS クラス: ClassName、ClassList、管理クラスリスト

- `ClassName(string)` — 値を半角スペースで分割し、各トークンを USS クラスとして適用します: `new ClassName("card highlighted")` は `card` と `highlighted` の両方を追加します。
- `ClassList(params string[])` — 配列から同様に適用します。`V.Classes(...)` は null/空白のエントリを除外して作成します。

どちらも内部の**管理クラスリスト**を通した置き換えセマンティクスです:

- 適用のたびに、新しいセットを追加する前に削除されるのは **Veauty 自身が以前に追加した** クラス (`ClassName`/`ClassList` 経由) だけです。
- それ以外が追加したクラス — UI Toolkit 自身のコントロールクラス (`unity-button` など) や、あなたのコードの `AddToClassList` — には**決して触れません**。
- したがって、レンダリング間で属性値からクラスを取り除くと要素からも削除されます (インラインスタイルと異なり、クラスには削除セマンティクスがあります)。ただし属性自体は適用され続けている必要があります。

```csharp
V.Box(className: isActive ? "panel active" : "panel")
// isActive=false での再レンダリングは "active" を削除し "panel" は維持、
// Veauty の外で追加されたクラスには影響しない
```

## StyleSheetAttr

`StyleSheetAttr(StyleSheet)` は要素の `styleSheets` リストに `StyleSheet` アセットを追加します。適用は冪等です (未追加の場合のみ追加)。属性を削除してもスタイルシートは**外れません**。通常はルートノードに1回適用します:

```csharp
V.Column(extras: new IAttribute<VisualElement>[] { new StyleSheetAttr(myUss) })[ ... ]
```

## スタイル以外の共通属性

これらも任意の要素に適用できます: `PickingMode(PickingMode)`、`Tooltip(string)`、`Visible(bool)` (非表示でもレイアウト領域は保持。レイアウトから外すには `Display(DisplayStyle.None)`)、`Enabled(bool)` (`SetEnabled` を呼ぶ)。

## 関連ページ

- [要素](elements.md) — これらの属性を渡す場所 (`extras`)。
- [API リファレンス](api-reference.md) — 完全なシグネチャ。
