# API リファレンス

[English](../api-reference.md)

`com.uzimaru.veauty-uitoolkit` (アセンブリ `Veauty.UIToolkit`) の全公開 API を名前空間ごとに掲載します。コアパッケージの型 (`IVTree`, `IAttribute<T>`, `Attribute<T,U>`, `Node<T,U>`, `Diff<T>`, `HookRuntime`, `IHostLifecycle<T>`) は `com.uzimaru.veauty` 側で文書化されています。

---

## 名前空間 `Veauty.UIToolkit`

### VeautyElement&lt;State&gt;

```csharp
public class VeautyElement<State> where State : struct
```

Veauty の仮想ツリーを `VisualElement` にマウントし、state 変更で再レンダリングします。レンダリングはコンストラクタ内で同期実行され、ルート要素は `flexGrow = 1` でマウント先の子として追加されます。

| メンバー | シグネチャ | 説明 |
|---|---|---|
| ctor (セッター) | `VeautyElement(VisualElement mounter, Func<State, Action<State>, IVTree> renderFunc, State state = default)` | レンダー関数は state + 値セッターを受け取る。即座にマウント。 |
| ctor (アップデータ) | `VeautyElement(VisualElement mounter, Func<State, Action<Func<State, State>>, IVTree> renderFunc, State state = default)` | レンダー関数は state + アップデータを受け取る。アップデータはレンダリング時にキャプチャした state に適用される。 |
| ctor (state のみ) | `VeautyElement(VisualElement mounter, Func<State, IVTree> renderFunc, State state = default)` | セッターなし。再レンダリングは hooks か `ForceUpdate` から。 |
| `ForceUpdate` | `void ForceUpdate()` | state 変更なしでも再レンダリング + diff + patch。レンダリング中やマウント前の呼び出しは遅延。 |

備考: レンダリング中の state 書き込みは遅延され、1回の追加レンダリングにまとめられます (再入ガード `isRendering` + `renderRequested`)。

### Renderer

```csharp
public static class Renderer
```

| メンバー | シグネチャ | 説明 |
|---|---|---|
| `Render` | `static VisualElement Render(IVTree vTree)` | 新しい要素階層を構築。ノードごとの順序: 生成 → ライフサイクル `Init` → 属性 → 子 → `AfterRenderKids`。非対応のツリー型や `VisualElement` を継承しないコンポーネント型では `ArgumentException`。 |

備考: 素の `FunctionComponentNode` は使い捨ての `HookRuntime` で解決されます (hook の state は保持されません)。`IVTreeWrapper` は unwrap されます。

### Patch

```csharp
public static class Patch
```

| メンバー | シグネチャ | 説明 |
|---|---|---|
| `Apply` | `static VisualElement Apply(VisualElement rootElement, IVTree oldVTree, IPatch<VisualElement>[] patches)` | `oldVTree` を走査して各パッチをターゲット要素に紐付け、削除/redraw されるサブツリーに `IHostLifecycle.Destroy` を (子から先に) 実行してからパッチを適用。(差し替えられた可能性のある) ルートを返す。 |

備考: 値が `null` の `Attrs` エントリ (属性の削除) はスキップされます — リセットなし。コントロールが作った内部の子は、仮想の子と実際の子のマッピング時に `childOffset` でスキップされます。

### ISubComponent

```csharp
public interface ISubComponent { }
```

サブコンポーネントのデータキャリアのためのマーカー。uGUI パッケージとの互換のために存在し、本パッケージの組み込みコントロールでサブコンポーネントを定義するものはありません。

### UIBase&lt;T&gt;

```csharp
public abstract class UIBase<T> : Node<VisualElement, T>,
    IHostLifecycle<VisualElement>, IHostLifecycleTree<VisualElement>
    where T : VisualElement
```

コントロールノードの基底。tag は `typeof(T).FullName` で、要素は `Activator.CreateInstance(typeof(T))` で生成されます。

| メンバー | シグネチャ | 説明 |
|---|---|---|
| ctor | `protected UIBase(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids)` | 属性 + 仮想の子。 |
| `Init` | `virtual VisualElement Init(VisualElement ve)` | 属性適用前のセットアップフック。デフォルトは no-op。 |
| `Destroy` | `virtual void Destroy(VisualElement ve)` | 破棄フック。デフォルトは no-op。 |
| `AfterRenderKids` | `virtual void AfterRenderKids(VisualElement ve)` | 子のレンダリング後フック。デフォルトは no-op。 |
| `GetHostLifecycles` | `IHostLifecycle<VisualElement>[] GetHostLifecycles()` | `{ this }` を返す。 |
| `FindPart<T>` | `protected T FindPart<T>() where T : class` | 型 `T` の最初の子、なければ `null`。 |

### UIAttributeBase&lt;TElement, TValue&gt;

```csharp
public abstract class UIAttributeBase<TElement, TValue> : Attribute<VisualElement, TValue>
    where TElement : VisualElement
```

| メンバー | シグネチャ | 説明 |
|---|---|---|
| ctor | `protected UIAttributeBase(string key, TValue value)` | diff キー + 値。 |
| `Apply` (abstract) | `protected abstract void Apply(TElement element)` | 型付きの適用。 |
| `Apply` (override) | `override void Apply(VisualElement obj)` | 型チェック後に型付き Apply を呼ぶ。`obj` が `TElement` でない場合は**黙って no-op**。 |

### StyleAttribute&lt;TValue&gt;

```csharp
public abstract class StyleAttribute<TValue> : Attribute<VisualElement, TValue>
```

インラインスタイル属性の基底。ツリーからの削除ではスタイルプロパティはリセットされません (`Patch` の備考を参照)。

| メンバー | シグネチャ | 説明 |
|---|---|---|
| ctor | `protected StyleAttribute(string key, TValue value)` | diff キー + 値。 |

### 共通属性

すべて `Attribute<VisualElement, TValue>` を継承し、任意の要素に適用できます。

| 型 | コンストラクタ | 適用内容 |
|---|---|---|
| `ClassName` | `ClassName(string value)` | スペースで分割し、Veauty 管理の USS クラスを置き換える (外部のクラスは維持)。 |
| `ClassList` | `ClassList(params string[] value)` | Veauty 管理の USS クラスを配列で置き換える。 |
| `StyleSheetAttr` | `StyleSheetAttr(StyleSheet value)` | 未追加ならスタイルシートを追加 (冪等。削除はされない)。 |
| `PickingMode` | `PickingMode(UnityEngine.UIElements.PickingMode value)` | `pickingMode`。 |
| `Tooltip` | `Tooltip(string value)` | `tooltip`。 |
| `Visible` | `Visible(bool value)` | `visible` (レイアウト領域は保持)。 |
| `Enabled` | `Enabled(bool value)` | `SetEnabled(value)`。 |

### スタイル属性

すべて `StyleAttribute<TValue>` を継承し、コンストラクタは表の値を1つ取ります。

| 型 | 値の型 | 設定先 |
|---|---|---|
| `FlexGrow` / `FlexShrink` | `float` | `flexGrow` / `flexShrink` |
| `FlexDirection` | `UnityEngine.UIElements.FlexDirection` | `flexDirection` |
| `FlexWrap` | `Wrap` | `flexWrap` |
| `JustifyContent` | `Justify` | `justifyContent` |
| `AlignItems` / `AlignSelf` | `Align` | `alignItems` / `alignSelf` |
| `Width` / `Height` / `MinWidth` / `MinHeight` / `MaxWidth` / `MaxHeight` | `StyleLength` | 対応するサイズプロパティ |
| `MarginTop` / `MarginBottom` / `MarginLeft` / `MarginRight` | `StyleLength` | 対応する辺のマージン |
| `PaddingTop` / `PaddingBottom` / `PaddingLeft` / `PaddingRight` | `StyleLength` | 対応する辺のパディング |
| `BackgroundColor` | `Color` | `backgroundColor` |
| `BorderColor` | `Color` | 4辺すべてのボーダー色 |
| `BorderWidth` | `float` | 4辺すべてのボーダー幅 |
| `BorderRadius` | `StyleLength` | 4隅すべての角丸 |
| `FontSize` | `StyleLength` | `fontSize` |
| `TextColor` | `Color` | `color` (diff キーは `"Color"`) |
| `Opacity` | `float` | `opacity` |
| `Display` | `DisplayStyle` | `display` |
| `Overflow` | `UnityEngine.UIElements.Overflow` | `overflow` |
| `Position` | `UnityEngine.UIElements.Position` | `position` |
| `Top` / `Bottom` / `Left` / `Right` | `StyleLength` | 対応するオフセット |

### StyleBuilder / Style

```csharp
public class StyleBuilder
public static class Style
```

| メンバー | シグネチャ | 説明 |
|---|---|---|
| `Style.Set` | `static StyleBuilder Set { get; }` | アクセスごとに新しいビルダー。 |
| チェーンメソッド | `StyleBuilder Xxx(value)` | 上記スタイル属性ごとに1つ、加えて `Margin(StyleLength)` と `Padding(StyleLength)` ショートハンド (各4属性)。`this` を返す。 |
| `Build` | `IAttribute<VisualElement>[] Build()` | 呼び出し順に集められた属性。 |
| 暗黙変換 | `static implicit operator IAttribute<VisualElement>[](StyleBuilder b)` | `Build()` と同じ。 |

### コントロールノードとその属性

各コントロールは次のパターンに従います:

```csharp
public abstract class XxxAttribute<T> : UIAttributeBase<UnityEngine.UIElements.Xxx, T>
public partial class Xxx : UIBase<UnityEngine.UIElements.Xxx>
{
    public Xxx(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids);
    // ネストされた属性クラス
}
```

#### Button

| ネスト型 | コンストラクタ | 適用内容 |
|---|---|---|
| `Button.Text` | `Text(string value)` | `text`。 |
| `Button.OnClick` | `OnClick(Action value)` | `clickable` を `new Clickable(value)` で置き換え — 積み重ならないが、以前の clickable は破棄される。`Attribute<VisualElement, Action>` を継承。Button 以外では no-op。 |

#### DropdownField

| ネスト型 | コンストラクタ | 適用内容 |
|---|---|---|
| `DropdownField.Value` | `Value(string value)` | `SetValueWithoutNotify` (変更イベントなし)。 |
| `DropdownField.Label` | `Label(string value)` | `label`。 |
| `DropdownField.Choices` | `Choices(List<string> value)` | `choices`。 |
| `DropdownField.Index` | `Index(int value)` | `index` (変更イベントが発火)。 |
| `DropdownField.OnValueChanged` | `OnValueChanged(EventCallback<ChangeEvent<string>> value)` | `(要素, "onValueChanged")` でキー付けされたコールバックストア — レンダリング間で重複排除。 |

#### Foldout

| ネスト型 | コンストラクタ | 適用内容 |
|---|---|---|
| `Foldout.Text` | `Text(string value)` | `text`。 |
| `Foldout.Value` | `Value(bool value)` | `SetValueWithoutNotify`。 |

#### Label

| ネスト型 | コンストラクタ | 適用内容 |
|---|---|---|
| `Label.Text` | `Text(string value)` | `text`。 |
| `Label.EnableRichText` | `EnableRichText(bool value)` | `enableRichText`。 |

#### ProgressBar

| ネスト型 | コンストラクタ | 適用内容 |
|---|---|---|
| `ProgressBar.Value` | `Value(float value)` | `value`。 |
| `ProgressBar.Title` | `Title(string value)` | `title`。 |
| `ProgressBar.LowValue` | `LowValue(float value)` | `lowValue`。 |
| `ProgressBar.HighValue` | `HighValue(float value)` | `highValue`。 |

#### ScrollView

| ネスト型 | コンストラクタ | 適用内容 |
|---|---|---|
| `ScrollView.Mode` | `Mode(ScrollViewMode value)` | `mode`。 |
| `ScrollView.HorizontalScrollerVisibility` | `HorizontalScrollerVisibility(ScrollerVisibility value)` | `horizontalScrollerVisibility`。 |
| `ScrollView.VerticalScrollerVisibility` | `VerticalScrollerVisibility(ScrollerVisibility value)` | `verticalScrollerVisibility`。 |
| `ScrollView.Elasticity` | `Elasticity(float value)` | `elasticity`。 |

#### Slider

| ネスト型 | コンストラクタ | 適用内容 |
|---|---|---|
| `Slider.Value` | `Value(float value)` | `SetValueWithoutNotify` (現在のレンジにクランプされる — レンジ属性を先に適用)。 |
| `Slider.LowValue` | `LowValue(float value)` | `lowValue`。 |
| `Slider.HighValue` | `HighValue(float value)` | `highValue`。 |
| `Slider.Direction` | `Direction(SliderDirection value)` | `direction`。 |
| `Slider.ShowInputField` | `ShowInputField(bool value)` | `showInputField`。 |
| `Slider.Label` | `Label(string value)` | `label`。 |
| `Slider.OnValueChanged` | `OnValueChanged(EventCallback<ChangeEvent<float>> value)` | コールバックストア、重複排除。 |

#### TextField

| ネスト型 | コンストラクタ | 適用内容 |
|---|---|---|
| `TextField.Value` | `Value(string value)` | `SetValueWithoutNotify` (フィードバックループ安全)。 |
| `TextField.Label` | `Label(string value)` | `label`。 |
| `TextField.IsReadOnly` | `IsReadOnly(bool value)` | `isReadOnly`。 |
| `TextField.Multiline` | `Multiline(bool value)` | `multiline`。 |
| `TextField.MaxLength` | `MaxLength(int value)` | `maxLength`。 |
| `TextField.Placeholder` | `Placeholder(string value)` | `textEdition.placeholder`。 |
| `TextField.OnValueChanged` | `OnValueChanged(EventCallback<ChangeEvent<string>> value)` | コールバックストア、重複排除。 |

#### Toggle

| ネスト型 | コンストラクタ | 適用内容 |
|---|---|---|
| `Toggle.Value` | `Value(bool value)` | `SetValueWithoutNotify`。 |
| `Toggle.Label` | `Label(string value)` | `label`。 |
| `Toggle.OnValueChanged` | `OnValueChanged(EventCallback<ChangeEvent<bool>> value)` | コールバックストア、重複排除。 |

カスタム属性のための属性基底: `ButtonAttribute<T>`, `DropdownFieldAttribute<T>`, `FoldoutAttribute<T>`, `LabelAttribute<T>`, `ProgressBarAttribute<T>`, `ScrollViewAttribute<T>`, `SliderAttribute<T>`, `TextFieldAttribute<T>`, `ToggleAttribute<T>` — いずれも `abstract class XxxAttribute<T> : UIAttributeBase<UnityEngine.UIElements.Xxx, T>` で、コンストラクタは `protected XxxAttribute(string key, T value)` です。

---

## 名前空間 `Veauty.UIToolkit.Presets`

### V

```csharp
public static class V
```

名前付きオプション引数のファクトリ。省略した引数は属性を追加しません。すべてのメソッドは末尾に `params IAttribute<VisualElement>[] extras` を持ちます。`className` の値は `ClassName` に渡されます (スペース区切りのクラス)。

| メンバー | シグネチャ | 戻り値 |
|---|---|---|
| `Children` | `static IVTree[] Children(IEnumerable<IVTree> children)` / `static IVTree[] Children(params IVTree[] children)` | null 以外の子 (null 安全)。 |
| `Classes` | `static ClassList Classes(params string[] classNames)` / `static ClassList Classes(IEnumerable<string> classNames)` | null/空白の名前を除外した `ClassList`。 |
| `Label` | `static IVTree Label(string text, string className = null, params IAttribute<VisualElement>[] extras)` | Label ノード (子なし)。 |
| `Button` | `static Element Button(string text = null, Action onClick = null, string className = null, params ... extras)` | `Element`。 |
| `TextField` | `static Element TextField(string value = null, string label = null, string placeholder = null, bool? multiline = null, bool? isReadOnly = null, int? maxLength = null, EventCallback<ChangeEvent<string>> onValueChanged = null, string className = null, params ... extras)` | `Element`。 |
| `Toggle` | `static Element Toggle(bool? value = null, string label = null, EventCallback<ChangeEvent<bool>> onValueChanged = null, string className = null, params ... extras)` | `Element`。 |
| `Slider` | `static Element Slider(float? value = null, float? lowValue = null, float? highValue = null, string label = null, SliderDirection? direction = null, bool? showInputField = null, EventCallback<ChangeEvent<float>> onValueChanged = null, string className = null, params ... extras)` | `Element`。`value` 属性はレンジ属性の後。 |
| `ScrollView` | `static Element ScrollView(ScrollViewMode? mode = null, ScrollerVisibility? horizontalScrollerVisibility = null, ScrollerVisibility? verticalScrollerVisibility = null, string className = null, params ... extras)` | `Element`。 |
| `Foldout` | `static Element Foldout(string text = null, bool? value = null, string className = null, params ... extras)` | `Element`。 |
| `DropdownField` | `static Element DropdownField(string value = null, string label = null, List<string> choices = null, int? index = null, EventCallback<ChangeEvent<string>> onValueChanged = null, string className = null, params ... extras)` | `Element`。`value` 属性は `choices`/`index` の後。 |
| `ProgressBar` | `static IVTree ProgressBar(float? value = null, string title = null, float? lowValue = null, float? highValue = null, string className = null, params ... extras)` | ProgressBar ノード (子なし)。 |
| `Box` | `static Element Box(string className = null, params ... extras)` | プレーンな `VisualElement` コンテナ、tag "Box"。 |
| `Row` | `static Element Row(string className = null, params ... extras)` | `FlexDirection.Row` のコンテナ、tag "Row"。 |
| `Column` | `static Element Column(string className = null, params ... extras)` | `FlexDirection.Column` のコンテナ、tag "Column"。 |

### Element

```csharp
public class Element : IVTreeWrapper
```

`V` メソッドが返す遅延ノードビルダー。コンストラクタは `internal` — インスタンスは `V` から取得します。

| メンバー | シグネチャ | 説明 |
|---|---|---|
| インデクサ | `IVTree this[params IVTree[] children]` | 子付きの新しいノードを構築。 |
| インデクサ | `IVTree this[IEnumerable<IVTree> children]` | 同上、シーケンスから。 |
| `GetNodeType` | `VTreeType GetNodeType()` | unwrap した (子なし) ノードに委譲。 |
| `GetDescendantsCount` | `int GetDescendantsCount()` | unwrap したノードに委譲。 |
| `Unwrap` | `IVTree Unwrap()` | 子ゼロのノードを遅延構築してキャッシュ。 |

備考: `Element` を `IVTree` として直接使うとキャッシュされた子なしノードが得られます。インデクサの使用はそのたびに新しい独立したノードを構築します。
