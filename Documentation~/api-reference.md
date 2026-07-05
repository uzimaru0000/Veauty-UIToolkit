# API Reference

[日本語](ja/api-reference.md)

Complete public API of `com.uzimaru.veauty-uitoolkit` (assembly `Veauty.UIToolkit`), grouped by namespace. Types from the core package (`IVTree`, `IAttribute<T>`, `Attribute<T,U>`, `Node<T,U>`, `Diff<T>`, `HookRuntime`, `IHostLifecycle<T>`) are documented in `com.uzimaru.veauty`.

---

## Namespace `Veauty.UIToolkit`

### VeautyElement&lt;State&gt;

```csharp
public class VeautyElement<State> where State : struct
```

Mounts a Veauty virtual tree onto a `VisualElement` and re-renders on state change. Rendering happens synchronously in the constructor; the root element is added under the mounter with `flexGrow = 1`.

| Member | Signature | Description |
|---|---|---|
| ctor (setter) | `VeautyElement(VisualElement mounter, Func<State, Action<State>, IVTree> renderFunc, State state = default)` | Render function gets state + value setter. Mounts immediately. |
| ctor (updater) | `VeautyElement(VisualElement mounter, Func<State, Action<Func<State, State>>, IVTree> renderFunc, State state = default)` | Render function gets state + updater; the updater is applied to the state captured at render time. |
| ctor (state only) | `VeautyElement(VisualElement mounter, Func<State, IVTree> renderFunc, State state = default)` | No setter; re-render via hooks or `ForceUpdate`. |
| `ForceUpdate` | `void ForceUpdate()` | Re-render + diff + patch even without a state change. Deferred if called mid-render or pre-mount. |

Remarks: state writes during a render are deferred and coalesced into a single follow-up render (reentrancy guard `isRendering` + `renderRequested`).

### Renderer

```csharp
public static class Renderer
```

| Member | Signature | Description |
|---|---|---|
| `Render` | `static VisualElement Render(IVTree vTree)` | Builds a fresh element hierarchy. Order per node: create → lifecycle `Init` → attributes → children → `AfterRenderKids`. Throws `ArgumentException` for unsupported tree types or non-`VisualElement` component types. |

Remarks: a bare `FunctionComponentNode` is resolved with a throwaway `HookRuntime` (hook state not persisted). `IVTreeWrapper`s are unwrapped.

### Patch

```csharp
public static class Patch
```

| Member | Signature | Description |
|---|---|---|
| `Apply` | `static VisualElement Apply(VisualElement rootElement, IVTree oldVTree, IPatch<VisualElement>[] patches)` | Binds each patch to its target element by walking `oldVTree`, runs `IHostLifecycle.Destroy` for removed/redrawn subtrees (children first), then applies the patches. Returns the (possibly replaced) root. |

Remarks: `Attrs` entries with a `null` value (attribute removed) are skipped — no reset. Internal children created by controls are skipped via a `childOffset` when mapping virtual to real children.

### ISubComponent

```csharp
public interface ISubComponent { }
```

Marker for sub-component data carriers. Present for parity with the uGUI package; no built-in control in this package defines sub-components.

### UIBase&lt;T&gt;

```csharp
public abstract class UIBase<T> : Node<VisualElement, T>,
    IHostLifecycle<VisualElement>, IHostLifecycleTree<VisualElement>
    where T : VisualElement
```

Base for control nodes. Tag is `typeof(T).FullName`; the element is created via `Activator.CreateInstance(typeof(T))`.

| Member | Signature | Description |
|---|---|---|
| ctor | `protected UIBase(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids)` | Attributes + virtual children. |
| `Init` | `virtual VisualElement Init(VisualElement ve)` | Pre-attribute setup hook; default no-op. |
| `Destroy` | `virtual void Destroy(VisualElement ve)` | Teardown hook; default no-op. |
| `AfterRenderKids` | `virtual void AfterRenderKids(VisualElement ve)` | Post-children hook; default no-op. |
| `GetHostLifecycles` | `IHostLifecycle<VisualElement>[] GetHostLifecycles()` | Returns `{ this }`. |
| `FindPart<T>` | `protected T FindPart<T>() where T : class` | First child of type `T`, or `null`. |

### UIAttributeBase&lt;TElement, TValue&gt;

```csharp
public abstract class UIAttributeBase<TElement, TValue> : Attribute<VisualElement, TValue>
    where TElement : VisualElement
```

| Member | Signature | Description |
|---|---|---|
| ctor | `protected UIAttributeBase(string key, TValue value)` | Diff key + value. |
| `Apply` (abstract) | `protected abstract void Apply(TElement element)` | Typed apply. |
| `Apply` (override) | `override void Apply(VisualElement obj)` | Type-checks then calls the typed apply; **silently no-ops** when `obj` is not a `TElement`. |

### StyleAttribute&lt;TValue&gt;

```csharp
public abstract class StyleAttribute<TValue> : Attribute<VisualElement, TValue>
```

Base for inline-style attributes. Removal from the tree does not reset the style property (see `Patch` remarks).

| Member | Signature | Description |
|---|---|---|
| ctor | `protected StyleAttribute(string key, TValue value)` | Diff key + value. |

### Common attributes

All extend `Attribute<VisualElement, TValue>` and apply to any element.

| Type | Constructor | Applies |
|---|---|---|
| `ClassName` | `ClassName(string value)` | Splits on spaces; replaces the Veauty-managed USS classes (external classes preserved). |
| `ClassList` | `ClassList(params string[] value)` | Replaces the Veauty-managed USS classes with the array. |
| `StyleSheetAttr` | `StyleSheetAttr(StyleSheet value)` | Adds the stylesheet if not present (idempotent; never removed). |
| `PickingMode` | `PickingMode(UnityEngine.UIElements.PickingMode value)` | `pickingMode`. |
| `Tooltip` | `Tooltip(string value)` | `tooltip`. |
| `Visible` | `Visible(bool value)` | `visible` (keeps layout space). |
| `Enabled` | `Enabled(bool value)` | `SetEnabled(value)`. |

### Style attributes

All extend `StyleAttribute<TValue>`; each constructor takes the single value shown.

| Type | Value type | Sets |
|---|---|---|
| `FlexGrow` / `FlexShrink` | `float` | `flexGrow` / `flexShrink` |
| `FlexDirection` | `UnityEngine.UIElements.FlexDirection` | `flexDirection` |
| `FlexWrap` | `Wrap` | `flexWrap` |
| `JustifyContent` | `Justify` | `justifyContent` |
| `AlignItems` / `AlignSelf` | `Align` | `alignItems` / `alignSelf` |
| `Width` / `Height` / `MinWidth` / `MinHeight` / `MaxWidth` / `MaxHeight` | `StyleLength` | matching size property |
| `MarginTop` / `MarginBottom` / `MarginLeft` / `MarginRight` | `StyleLength` | matching margin side |
| `PaddingTop` / `PaddingBottom` / `PaddingLeft` / `PaddingRight` | `StyleLength` | matching padding side |
| `BackgroundColor` | `Color` | `backgroundColor` |
| `BorderColor` | `Color` | all four border colors |
| `BorderWidth` | `float` | all four border widths |
| `BorderRadius` | `StyleLength` | all four corner radii |
| `FontSize` | `StyleLength` | `fontSize` |
| `TextColor` | `Color` | `color` (diff key `"Color"`) |
| `Opacity` | `float` | `opacity` |
| `Display` | `DisplayStyle` | `display` |
| `Overflow` | `UnityEngine.UIElements.Overflow` | `overflow` |
| `Position` | `UnityEngine.UIElements.Position` | `position` |
| `Top` / `Bottom` / `Left` / `Right` | `StyleLength` | matching offset |

### StyleBuilder / Style

```csharp
public class StyleBuilder
public static class Style
```

| Member | Signature | Description |
|---|---|---|
| `Style.Set` | `static StyleBuilder Set { get; }` | New builder per access. |
| chain methods | `StyleBuilder Xxx(value)` | One per style attribute above, plus `Margin(StyleLength)` and `Padding(StyleLength)` shorthands (four attributes each). Returns `this`. |
| `Build` | `IAttribute<VisualElement>[] Build()` | Collected attributes in call order. |
| implicit op | `static implicit operator IAttribute<VisualElement>[](StyleBuilder b)` | Same as `Build()`. |

### Control nodes and their attributes

Each control follows the pattern:

```csharp
public abstract class XxxAttribute<T> : UIAttributeBase<UnityEngine.UIElements.Xxx, T>
public partial class Xxx : UIBase<UnityEngine.UIElements.Xxx>
{
    public Xxx(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids);
    // nested attribute classes
}
```

#### Button

| Nested type | Constructor | Applies |
|---|---|---|
| `Button.Text` | `Text(string value)` | `text`. |
| `Button.OnClick` | `OnClick(Action value)` | Replaces `clickable` with `new Clickable(value)` — never stacks, but discards any prior clickable. Extends `Attribute<VisualElement, Action>`; no-ops on non-Buttons. |

#### DropdownField

| Nested type | Constructor | Applies |
|---|---|---|
| `DropdownField.Value` | `Value(string value)` | `SetValueWithoutNotify` (no change event). |
| `DropdownField.Label` | `Label(string value)` | `label`. |
| `DropdownField.Choices` | `Choices(List<string> value)` | `choices`. |
| `DropdownField.Index` | `Index(int value)` | `index` (fires change event). |
| `DropdownField.OnValueChanged` | `OnValueChanged(EventCallback<ChangeEvent<string>> value)` | Callback store keyed `(element, "onValueChanged")` — de-duplicated across renders. |

#### Foldout

| Nested type | Constructor | Applies |
|---|---|---|
| `Foldout.Text` | `Text(string value)` | `text`. |
| `Foldout.Value` | `Value(bool value)` | `SetValueWithoutNotify`. |

#### Label

| Nested type | Constructor | Applies |
|---|---|---|
| `Label.Text` | `Text(string value)` | `text`. |
| `Label.EnableRichText` | `EnableRichText(bool value)` | `enableRichText`. |

#### ProgressBar

| Nested type | Constructor | Applies |
|---|---|---|
| `ProgressBar.Value` | `Value(float value)` | `value`. |
| `ProgressBar.Title` | `Title(string value)` | `title`. |
| `ProgressBar.LowValue` | `LowValue(float value)` | `lowValue`. |
| `ProgressBar.HighValue` | `HighValue(float value)` | `highValue`. |

#### ScrollView

| Nested type | Constructor | Applies |
|---|---|---|
| `ScrollView.Mode` | `Mode(ScrollViewMode value)` | `mode`. |
| `ScrollView.HorizontalScrollerVisibility` | `HorizontalScrollerVisibility(ScrollerVisibility value)` | `horizontalScrollerVisibility`. |
| `ScrollView.VerticalScrollerVisibility` | `VerticalScrollerVisibility(ScrollerVisibility value)` | `verticalScrollerVisibility`. |
| `ScrollView.Elasticity` | `Elasticity(float value)` | `elasticity`. |

#### Slider

| Nested type | Constructor | Applies |
|---|---|---|
| `Slider.Value` | `Value(float value)` | `SetValueWithoutNotify` (clamped to current range — apply range attrs first). |
| `Slider.LowValue` | `LowValue(float value)` | `lowValue`. |
| `Slider.HighValue` | `HighValue(float value)` | `highValue`. |
| `Slider.Direction` | `Direction(SliderDirection value)` | `direction`. |
| `Slider.ShowInputField` | `ShowInputField(bool value)` | `showInputField`. |
| `Slider.Label` | `Label(string value)` | `label`. |
| `Slider.OnValueChanged` | `OnValueChanged(EventCallback<ChangeEvent<float>> value)` | Callback store, de-duplicated. |

#### TextField

| Nested type | Constructor | Applies |
|---|---|---|
| `TextField.Value` | `Value(string value)` | `SetValueWithoutNotify` (feedback-loop safe). |
| `TextField.Label` | `Label(string value)` | `label`. |
| `TextField.IsReadOnly` | `IsReadOnly(bool value)` | `isReadOnly`. |
| `TextField.Multiline` | `Multiline(bool value)` | `multiline`. |
| `TextField.MaxLength` | `MaxLength(int value)` | `maxLength`. |
| `TextField.Placeholder` | `Placeholder(string value)` | `textEdition.placeholder`. |
| `TextField.OnValueChanged` | `OnValueChanged(EventCallback<ChangeEvent<string>> value)` | Callback store, de-duplicated. |

#### Toggle

| Nested type | Constructor | Applies |
|---|---|---|
| `Toggle.Value` | `Value(bool value)` | `SetValueWithoutNotify`. |
| `Toggle.Label` | `Label(string value)` | `label`. |
| `Toggle.OnValueChanged` | `OnValueChanged(EventCallback<ChangeEvent<bool>> value)` | Callback store, de-duplicated. |

Attribute bases available for custom attributes: `ButtonAttribute<T>`, `DropdownFieldAttribute<T>`, `FoldoutAttribute<T>`, `LabelAttribute<T>`, `ProgressBarAttribute<T>`, `ScrollViewAttribute<T>`, `SliderAttribute<T>`, `TextFieldAttribute<T>`, `ToggleAttribute<T>` — each `abstract class XxxAttribute<T> : UIAttributeBase<UnityEngine.UIElements.Xxx, T>` with a `protected XxxAttribute(string key, T value)` constructor.

---

## Namespace `Veauty.UIToolkit.Presets`

### V

```csharp
public static class V
```

Factory with named optional parameters. Omitted parameters add no attribute. All methods end with `params IAttribute<VisualElement>[] extras`. `className` values are passed to `ClassName` (space-separated classes).

| Member | Signature | Returns |
|---|---|---|
| `Children` | `static IVTree[] Children(IEnumerable<IVTree> children)` / `static IVTree[] Children(params IVTree[] children)` | Non-null children (null-safe). |
| `Classes` | `static ClassList Classes(params string[] classNames)` / `static ClassList Classes(IEnumerable<string> classNames)` | `ClassList` without null/whitespace names. |
| `Label` | `static IVTree Label(string text, string className = null, params IAttribute<VisualElement>[] extras)` | Label node (no children). |
| `Button` | `static Element Button(string text = null, Action onClick = null, string className = null, params ... extras)` | `Element`. |
| `TextField` | `static Element TextField(string value = null, string label = null, string placeholder = null, bool? multiline = null, bool? isReadOnly = null, int? maxLength = null, EventCallback<ChangeEvent<string>> onValueChanged = null, string className = null, params ... extras)` | `Element`. |
| `Toggle` | `static Element Toggle(bool? value = null, string label = null, EventCallback<ChangeEvent<bool>> onValueChanged = null, string className = null, params ... extras)` | `Element`. |
| `Slider` | `static Element Slider(float? value = null, float? lowValue = null, float? highValue = null, string label = null, SliderDirection? direction = null, bool? showInputField = null, EventCallback<ChangeEvent<float>> onValueChanged = null, string className = null, params ... extras)` | `Element`. `value` attr ordered after range attrs. |
| `ScrollView` | `static Element ScrollView(ScrollViewMode? mode = null, ScrollerVisibility? horizontalScrollerVisibility = null, ScrollerVisibility? verticalScrollerVisibility = null, string className = null, params ... extras)` | `Element`. |
| `Foldout` | `static Element Foldout(string text = null, bool? value = null, string className = null, params ... extras)` | `Element`. |
| `DropdownField` | `static Element DropdownField(string value = null, string label = null, List<string> choices = null, int? index = null, EventCallback<ChangeEvent<string>> onValueChanged = null, string className = null, params ... extras)` | `Element`. `value` attr ordered after `choices`/`index`. |
| `ProgressBar` | `static IVTree ProgressBar(float? value = null, string title = null, float? lowValue = null, float? highValue = null, string className = null, params ... extras)` | ProgressBar node (no children). |
| `Box` | `static Element Box(string className = null, params ... extras)` | Plain `VisualElement` container, tag "Box". |
| `Row` | `static Element Row(string className = null, params ... extras)` | Container with `FlexDirection.Row`, tag "Row". |
| `Column` | `static Element Column(string className = null, params ... extras)` | Container with `FlexDirection.Column`, tag "Column". |

### Element

```csharp
public class Element : IVTreeWrapper
```

Deferred node builder returned by `V` methods. The constructor is `internal` — obtain instances from `V`.

| Member | Signature | Description |
|---|---|---|
| indexer | `IVTree this[params IVTree[] children]` | Builds a fresh node with the children. |
| indexer | `IVTree this[IEnumerable<IVTree> children]` | Same, from a sequence. |
| `GetNodeType` | `VTreeType GetNodeType()` | Delegates to the unwrapped (childless) node. |
| `GetDescendantsCount` | `int GetDescendantsCount()` | Delegates to the unwrapped node. |
| `Unwrap` | `IVTree Unwrap()` | Lazily builds and caches the node with zero children. |

Remarks: using an `Element` directly as an `IVTree` yields the cached childless node; each indexer use builds a new, independent node.
