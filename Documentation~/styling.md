# Styling

[日本語](ja/styling.md)

Inline style attributes, the fluent `StyleBuilder`, USS class management, and stylesheets.

## Inline style attributes (`StyleAttribute<TValue>`)

Every class below derives from `StyleAttribute<TValue>` and writes one (or a group of) inline USS style properties on `VisualElement.style`. They apply to **any** element.

| Category | Attribute | Value type | Sets |
|---|---|---|---|
| Flex | `FlexGrow` | `float` | `flexGrow` |
| Flex | `FlexShrink` | `float` | `flexShrink` |
| Flex | `FlexDirection` | `UnityEngine.UIElements.FlexDirection` | `flexDirection` |
| Flex | `FlexWrap` | `Wrap` | `flexWrap` |
| Alignment | `JustifyContent` | `Justify` | `justifyContent` |
| Alignment | `AlignItems` | `Align` | `alignItems` |
| Alignment | `AlignSelf` | `Align` | `alignSelf` |
| Size | `Width`, `Height` | `StyleLength` | `width` / `height` |
| Size | `MinWidth`, `MinHeight` | `StyleLength` | `minWidth` / `minHeight` |
| Size | `MaxWidth`, `MaxHeight` | `StyleLength` | `maxWidth` / `maxHeight` |
| Margin | `MarginTop`, `MarginBottom`, `MarginLeft`, `MarginRight` | `StyleLength` | one margin side each |
| Padding | `PaddingTop`, `PaddingBottom`, `PaddingLeft`, `PaddingRight` | `StyleLength` | one padding side each |
| Color | `BackgroundColor` | `Color` | `backgroundColor` |
| Color | `TextColor` | `Color` | `color` (diff key `"Color"`) |
| Color | `Opacity` | `float` | `opacity` |
| Border | `BorderColor` | `Color` | all four `border*Color` |
| Border | `BorderWidth` | `float` | all four `border*Width` |
| Border | `BorderRadius` | `StyleLength` | all four corner radii |
| Text | `FontSize` | `StyleLength` | `fontSize` |
| Layout | `Display` | `DisplayStyle` | `display` |
| Layout | `Overflow` | `UnityEngine.UIElements.Overflow` | `overflow` |
| Position | `Position` | `UnityEngine.UIElements.Position` | `position` |
| Position | `Top`, `Bottom`, `Left`, `Right` | `StyleLength` | the matching offset |

```csharp
using Veauty.UIToolkit;

V.Box(extras: new IAttribute<VisualElement>[] {
    new BackgroundColor(Color.gray),
    new PaddingTop(new StyleLength(16)),
    new BorderRadius(new StyleLength(8)),
})
```

> **Gotcha — no reset on removal.** When an attribute disappears from the tree between renders, the diff emits a `null` entry for it and the patcher skips `null`s. The style property therefore **keeps its last value**; it is not reset to the USS/default value. To "remove" a style, apply an attribute with the neutral value explicitly (or drive the look via USS classes instead).

## StyleBuilder (fluent API)

`Style.Set` starts a fresh `StyleBuilder`; each call appends one attribute (the `Margin`/`Padding` shorthands append four). `Build()` returns `IAttribute<VisualElement>[]`, and the builder also converts implicitly to that array type.

```csharp
using Veauty.UIToolkit;

V.Box(extras: Style.Set
    .FlexDirection(UnityEngine.UIElements.FlexDirection.Row)
    .Padding(new StyleLength(16))       // all four sides
    .BackgroundColor(Color.gray)
    .BorderRadius(new StyleLength(8))
    .Build())
```

Builder methods, mirroring the attribute list above: `FlexGrow`, `FlexShrink`, `FlexDirection`, `FlexWrap`, `JustifyContent`, `AlignItems`, `AlignSelf`, `Width`, `Height`, `MinWidth`, `MinHeight`, `MaxWidth`, `MaxHeight`, `MarginTop/Bottom/Left/Right`, `Margin` (all sides), `PaddingTop/Bottom/Left/Right`, `Padding` (all sides), `BackgroundColor`, `TextColor`, `Opacity`, `BorderColor`, `BorderWidth`, `BorderRadius`, `FontSize`, `Display`, `Overflow`, `Position`, `Top`, `Bottom`, `Left`, `Right`.

## USS classes: ClassName, ClassList, and the managed class list

- `ClassName(string)` — splits its value on single spaces and applies each token as a USS class: `new ClassName("card highlighted")` adds both `card` and `highlighted`.
- `ClassList(params string[])` — same, from an array. `V.Classes(...)` builds one while dropping null/whitespace entries.

Both go through an internal **managed class list** with replace semantics:

- On each apply, only the classes **Veauty itself added previously** (via `ClassName`/`ClassList`) are removed before the new set is added.
- Classes added by anything else — UI Toolkit's own control classes (`unity-button`, ...) or your code calling `AddToClassList` — are **never touched**.
- Consequently, removing a class from the attribute value between renders removes it from the element (unlike inline styles, classes do have remove semantics), as long as the attribute itself is still applied.

```csharp
V.Box(className: isActive ? "panel active" : "panel")
// re-render with isActive=false removes "active" but keeps "panel"
// and never disturbs classes added outside Veauty
```

## StyleSheetAttr

`StyleSheetAttr(StyleSheet)` attaches a `StyleSheet` asset to the element's `styleSheets` list. The apply is idempotent (the sheet is only added if not already present). Removing the attribute does **not** detach the stylesheet. Typically applied once on the root node:

```csharp
V.Column(extras: new IAttribute<VisualElement>[] { new StyleSheetAttr(myUss) })[ ... ]
```

## Non-style common attributes

These also apply to any element: `PickingMode(PickingMode)`, `Tooltip(string)`, `Visible(bool)` (hidden elements keep their layout space; use `Display(DisplayStyle.None)` to remove from layout), `Enabled(bool)` (calls `SetEnabled`).

## See also

- [Elements](elements.md) — where to pass these attributes (`extras`).
- [API reference](api-reference.md) — full signatures.
