# Elements

[日本語](ja/elements.md)

The mount point `VeautyElement<State>`, the `UIBase<T>` control catalog, and the `Veauty.UIToolkit.Presets` convenience layer.

## VeautyElement&lt;State&gt;

`Veauty.UIToolkit.VeautyElement<State>` (with `State : struct`) mounts a virtual tree onto any `VisualElement` and re-renders it when state changes. Construction renders and mounts **synchronously**; the rendered root is added as a child of the mount element and given `style.flexGrow = 1` so it fills the mount area.

### The three renderFunc overloads

```csharp
// 1. Value setter: setState replaces the whole state
new VeautyElement<S>(mounter, (S state, Action<S> setState) => tree, initial);

// 2. Updater: setState takes a State => State function
new VeautyElement<S>(mounter, (S state, Action<Func<S, S>> setState) => tree, initial);

// 3. State only: re-renders come from hooks or ForceUpdate()
new VeautyElement<S>(mounter, (S state) => tree, initial);
```

Note on overload 2: the update function is applied to the state that was current **when the render function ran** (the captured `s`), not the state at the moment the updater is invoked. Two updaters fired from the same render both start from the same snapshot.

### ForceUpdate

`ForceUpdate()` re-runs the render function and applies the diff even if the state has not changed — useful when the tree depends on external data that Veauty cannot observe. Called before the mount finished or while a render is running, it is deferred instead of executing immediately.

### Reentrancy and coalescing

State writes (from `setState`, hooks, or `ForceUpdate`) that happen **during** a render never start a nested render. They set a pending flag; after the current render completes, exactly one follow-up render runs. Any number of state writes within one render collapse into that single render.

## UIBase&lt;T&gt; control catalog

Each control in `Veauty.UIToolkit` is a `UIBase<T> : Node<VisualElement, T>` whose tag is `typeof(T).FullName`. Attributes are nested classes; each control also has an abstract `XxxAttribute<T>` base for writing custom attributes that target that element type (and silently no-op on any other element).

| Control node | Renders | Attributes (nested classes) |
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

`*` = applied via `SetValueWithoutNotify`, so setting the value does not fire the control's change event (see [Events](events.md)). `DropdownField.Index` is a plain `index` assignment and **does** notify.

Base-layer usage (explicit attribute lists):

```csharp
using Veauty.UIToolkit;

new Slider(new IAttribute<VisualElement>[] {
    new Slider.LowValue(0f),
    new Slider.HighValue(1f),
    new Slider.Value(0.5f),
    new Slider.OnValueChanged(evt => Debug.Log(evt.newValue)),
})
```

`UIBase<T>` also implements the host lifecycle (`Init`/`AfterRenderKids`/`Destroy`, all virtual no-ops) and provides `FindPart<T>()` for locating an `ISubComponent` child. No built-in control currently uses sub-components — UI Toolkit controls create their internal elements themselves.

## Presets: the V factory

`Veauty.UIToolkit.Presets.V` wraps the base controls in factory methods with named optional parameters. Omitted parameters add no attribute at all, leaving the control's own defaults. Every method ends with `params IAttribute<VisualElement>[] extras` for styles, classes, and any other attribute.

| Factory | Returns | Parameters (all optional unless noted) |
|---|---|---|
| `V.Label(text, className, extras)` | `IVTree` | `text` required |
| `V.Button(text, onClick, className, extras)` | `Element` | |
| `V.TextField(value, label, placeholder, multiline, isReadOnly, maxLength, onValueChanged, className, extras)` | `Element` | |
| `V.Toggle(value, label, onValueChanged, className, extras)` | `Element` | |
| `V.Slider(value, lowValue, highValue, label, direction, showInputField, onValueChanged, className, extras)` | `Element` | `value` applied after the range attributes |
| `V.ScrollView(mode, horizontalScrollerVisibility, verticalScrollerVisibility, className, extras)` | `Element` | |
| `V.Foldout(text, value, className, extras)` | `Element` | |
| `V.DropdownField(value, label, choices, index, onValueChanged, className, extras)` | `Element` | `value` applied after `choices`/`index` |
| `V.ProgressBar(value, title, lowValue, highValue, className, extras)` | `IVTree` | `value` applied after the range attributes |
| `V.Box(className, extras)` | `Element` | plain `VisualElement`, tag "Box" |
| `V.Row(className, extras)` | `Element` | `flex-direction: row` container |
| `V.Column(className, extras)` | `Element` | `flex-direction: column` container |

Helpers:

- `V.Children(params IVTree[])` / `V.Children(IEnumerable<IVTree>)` — filters out `null` entries; convenient for conditional children (`condition ? node : null`).
- `V.Classes(params string[])` / `V.Classes(IEnumerable<string>)` — builds a `ClassList`, dropping null/whitespace names.

```csharp
V.Column()[
    V.Children(
        V.Label("Always"),
        showDetails ? V.Label("Details") : null
    )
]
```

## The Element builder

`V` methods that can take children return an `Element` (an `IVTreeWrapper`). Apply the indexer to supply children:

```csharp
V.Row()[ V.Label("Left"), V.Label("Right") ]          // params IVTree[]
V.Column()[ items.Select(RenderItem) ]                 // IEnumerable<IVTree> overload
```

Both indexer overloads build a fresh node each time. Used directly as an `IVTree` **without** the indexer, the `Element` lazily builds a childless node on first access and caches it — so `V.Button("OK", onClick)` alone is a valid leaf node.

## See also

- [Styling](styling.md) — style attributes and classes to pass as `extras`.
- [Events](events.md) — how `onClick`/`onValueChanged` behave across re-renders.
- [API reference](api-reference.md) — full signatures.
