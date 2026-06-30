# Veauty-UIToolkit

Build Unity UI Toolkit (UIElements) UIs declaratively with Veauty.

## Requirements

- Unity 6000.5 or later (Unity 6)
- `com.uzimaru.veauty`

## Install

Add to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.uzimaru.veauty": "https://github.com/uzimaru0000/Veauty.git",
    "com.uzimaru.veauty-uitoolkit": "https://github.com/uzimaru0000/Veauty-UIToolkit.git"
  }
}
```

## Usage

Mount a UI tree to a `VisualElement` with `VeautyElement<State>`.

```csharp
using UnityEngine;
using UnityEngine.UIElements;
using Veauty;
using Veauty.UIToolkit;

public class CounterPanel : MonoBehaviour
{
    record CounterState(int Count);

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var app = new VeautyElement<CounterState>(
            root,
            (state, setState) => V.Column()[
                CountDisplay(state.Count),
                V.Button("Increment", () => setState(s => s with { Count = s.Count + 1 }))
            ],
            new CounterState(0)
        );
    }

    [Component]
    static IVTree CountDisplay(int value)
    {
        Hooks.UseEffect(() =>
        {
            Debug.Log($"Count: {value}");
            return null;
        }, new object[] { value });

        return V.Label($"Count: {value}");
    }
}
```

## V factory API

Create controls with `V` class methods. Use `[]` indexer for children.

### Controls

```csharp
V.Label("Hello", className: "title")

V.Button("Click", onClick: () => { /* ... */ })

V.TextField(value: "input", placeholder: "Type here",
    onValueChanged: v => { /* ... */ })

V.Toggle(value: true, label: "Enable",
    onValueChanged: v => { /* ... */ })

V.Slider(value: 0.5f, lowValue: 0f, highValue: 1f,
    onValueChanged: v => { /* ... */ })

V.ProgressBar(value: 0.7f, title: "Loading...")

V.DropdownField(choices: new List<string> { "A", "B", "C" },
    index: 0, onValueChanged: v => { /* ... */ })

V.Foldout(text: "Details", value: true)[
    V.Label("Content")
]

V.ScrollView()[
    // scrollable content
]
```

### Layout

```csharp
// Flexbox Row
V.Row()[
    V.Label("Left"),
    V.Label("Right")
]

// Flexbox Column
V.Column()[
    V.Label("Top"),
    V.Label("Bottom")
]

// Generic container
V.Box(className: "card")[
    V.Label("Content")
]
```

## Styling

### Attribute-based

```csharp
V.Box(extras: new IAttribute<VisualElement>[] {
    new FlexDirection(UnityEngine.UIElements.FlexDirection.Row),
    new BackgroundColor(Color.gray),
    new PaddingTop(new StyleLength(16)),
    new PaddingBottom(new StyleLength(16)),
    new BorderRadius(new StyleLength(8)),
    new FontSize(new StyleLength(18))
})
```

### Style Builder (fluent API)

```csharp
V.Box(extras: new IAttribute<VisualElement>[] {
    new Style.Set
        .FlexDirection(FlexDirection.Row)
        .Padding(new StyleLength(16))
        .BackgroundColor(Color.gray)
        .BorderRadius(new StyleLength(8))
        .Build()
})
```

### Style attributes

| Category | Attributes |
|----------|-----------|
| Layout | `FlexDirection`, `FlexGrow`, `FlexShrink`, `FlexWrap`, `JustifyContent`, `AlignItems`, `AlignSelf` |
| Sizing | `Width`, `Height`, `MinWidth`, `MinHeight`, `MaxWidth`, `MaxHeight` |
| Spacing | `MarginTop/Bottom/Left/Right`, `PaddingTop/Bottom/Left/Right` |
| Visual | `BackgroundColor`, `TextColor`, `BorderColor`, `BorderWidth`, `BorderRadius`, `Opacity` |
| Position | `Display`, `Overflow`, `Position`, `Top`, `Bottom`, `Left`, `Right` |
| Text | `FontSize` |

## Common attributes

Available on all controls:

| Attribute | Effect |
|-----------|--------|
| `ClassName(string)` | Set a USS class |
| `ClassList(string[])` | Set multiple USS classes |
| `StyleSheetAttr(StyleSheet)` | Apply a stylesheet |
| `Visible(bool)` | Show/hide |
| `Enabled(bool)` | Enable/disable |
| `PickingMode(PickingMode)` | Picking mode |
| `Tooltip(string)` | Tooltip text |

## Comparison with uGUI package

| | Veauty-uGUI | Veauty-UIToolkit |
|---|---|---|
| Target | Canvas + uGUI components | VisualElement tree |
| Mount to | `GameObject` | `VisualElement` |
| Entry point | `VeautyObject` | `VeautyElement<State>` |
| Styling | Attribute-based | USS-like attributes + Style Builder |
| Layout | Layout Group components | Flexbox |
