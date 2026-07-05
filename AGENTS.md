# Veauty-UIToolkit — Agent Guide

`com.uzimaru.veauty-uitoolkit` renders Veauty virtual trees (`IVTree` from `com.uzimaru.veauty`) into Unity UI Toolkit `VisualElement` hierarchies and keeps them in sync via diff/patch. It is the UI Toolkit counterpart of `com.uzimaru.veauty-ugui`: same core, but the host is a `VisualElement` (no GameObjects, no components, flexbox layout, USS styling).

## Package map

| Path | Role |
|---|---|
| `Lib/Veauty.cs` | `VeautyElement<State>` — mount point, re-render scheduling (reentrancy guard + coalescing) |
| `Lib/Render.cs` | `Renderer` — IVTree → fresh `VisualElement` subtree (create → Init → attrs → kids → AfterRenderKids) |
| `Lib/Patch.cs` | `Patch.Apply` — binds diff patches to elements, runs lifecycle `Destroy`, mutates the hierarchy |
| `Lib/UIBase.cs` | `UIBase<T>` control-node base, `UIAttributeBase<TElement,TValue>`, `StyleAttribute<TValue>`, `ISubComponent` |
| `Lib/Attributes.cs` | Common attributes (`ClassName`, `ClassList`, `StyleSheetAttr`, `Visible`, ...) + all inline style attributes |
| `Lib/Style.cs` | `StyleBuilder` fluent API + `Style.Set` |
| `Lib/Controls/*.cs` | One file per control: `Button`, `DropdownField`, `Foldout`, `Label`, `ProgressBar`, `ScrollView`, `Slider`, `TextField`, `Toggle` |
| `Lib/CallbackStore.cs` | internal — de-duplicates ChangeEvent callbacks per (element, key) |
| `Lib/ManagedClassList.cs` | internal — tracks Veauty-added USS classes so replaces never touch external classes |
| `Lib/V.cs`, `Lib/Element.cs` | `Veauty.UIToolkit.Presets` — `V` factory + `Element` deferred builder |
| `Tests/Editor/` | `Veauty.UIToolkit.Editor.Tests` (NUnit coverage of V factory, render, diff, patch) |
| `Documentation~/` | Manual (English at root, Japanese under `ja/`) |

## Public API surface

```csharp
using Veauty;                    // IVTree, IAttribute<T>, Hooks, [Component] (core package)
using Veauty.UIToolkit;          // VeautyElement<State>, Renderer, Patch, controls, attributes, Style
using Veauty.UIToolkit.Presets;  // V, Element
```

- `VeautyElement<State>` (`State : struct`) — the only mount point. 3 ctors: `(VisualElement, Func<State, Action<State>, IVTree>, State)`, `(VisualElement, Func<State, Action<Func<State,State>>, IVTree>, State)`, `(VisualElement, Func<State, IVTree>, State)`; plus `ForceUpdate()`.
- `V.Label/Button/TextField/Toggle/Slider/ScrollView/Foldout/DropdownField/ProgressBar/Box/Row/Column`, `V.Children`, `V.Classes` — preferred tree-building API.
- Base controls (`Veauty.UIToolkit.Button` etc.) with nested attribute classes (`Button.Text`, `Slider.Value`, ...) for explicit attribute lists.
- `Style.Set...Build()` / individual `StyleAttribute` classes for inline styles; `ClassName`/`ClassList`/`StyleSheetAttr` for USS.
- `Renderer.Render` / `Patch.Apply` — host plumbing; normally only called by `VeautyElement`.

## Usage rules

1. `State` must be a value type — use `struct` or `record struct`, never a class (`where State : struct`).
2. Build trees through `V` (namespace `Veauty.UIToolkit.Presets`) or the control constructors; do not construct `FunctionComponentNode` and render it via `Renderer.Render` directly — that path uses a throwaway `HookRuntime` and loses hook state between renders.
3. Set control values through the `Value` attributes / `value:` factory parameters — they use `SetValueWithoutNotify`, which is what prevents change-event feedback loops. Do not write `element.value = x` in render code.
4. Register change handlers only via `OnValueChanged` attributes / `onValueChanged:` parameters; they de-duplicate per (element, key). Never call `RegisterCallback` from inside a render function.
5. Typed attributes (`UIAttributeBase<TElement,TValue>` subclasses) silently no-op on the wrong element type — putting `Slider.Value` on a `Button` compiles and does nothing. Match attributes to their control.
6. Do not rely on removing an attribute to reset anything: the diff emits `null` for removed attributes and the patcher skips them. Apply the neutral value explicitly instead.
7. Custom element types used with `Node<VisualElement, T>` / `UIBase<T>` must inherit `VisualElement` and have a public parameterless constructor (`Activator.CreateInstance`), or rendering throws `ArgumentException`.
8. Hooks (`Hooks.UseState` etc.) may only be called inside a `[Component]`-attributed method resolved through the tree; calling them from plain render code throws `InvalidOperationException`.
9. Git operations for this package happen inside `Packages/com.uzimaru.veauty-uitoolkit/` (its own repo), not the Unity project root.

## Common patterns

Mount:

```csharp
var root = GetComponent<UIDocument>().rootVisualElement;
var app = new VeautyElement<AppState>(root, RenderApp, new AppState(0));

static IVTree RenderApp(AppState state, Action<Func<AppState, AppState>> setState) =>
    V.Column()[
        V.Label($"Count: {state.Count}"),
        V.Button("+", () => setState(s => s with { Count = s.Count + 1 }))
    ];
```

Controlled text input (no feedback loop, no duplicate handlers):

```csharp
V.TextField(
    value: state.Name,
    placeholder: "Your name",
    onValueChanged: evt => setState(s => s with { Name = evt.newValue })
)
```

Conditional children:

```csharp
V.Column()[
    V.Children(
        V.Label("Header"),
        state.ShowDetail ? V.Label("Detail") : null
    )
]
```

Keyed list rendering (stable identity across reorders):

```csharp
using Veauty.VTree;
new KeyedNode<VisualElement>("list", new IAttribute<VisualElement>[] { },
    items.Select(i => (i.Id, (IVTree)V.Label(i.Title))).ToArray())
```

Inline styling:

```csharp
V.Box(extras: Style.Set
    .FlexDirection(UnityEngine.UIElements.FlexDirection.Row)
    .Padding(new StyleLength(8))
    .BackgroundColor(Color.gray)
    .Build())[ ... ]
```

USS class + stylesheet:

```csharp
V.Column(className: "panel", extras: new IAttribute<VisualElement>[] {
    new StyleSheetAttr(myStyleSheet)
})[ ... ]
```

## Pitfalls

- **Silent no-op attributes.** Wrong: `V.Box(extras: new IAttribute<VisualElement>[]{ new Slider.LowValue(0) })` — compiles, does nothing. Right: put control-typed attributes on their own control only.
- **Style removal does not reset.** Wrong: dropping `new BackgroundColor(...)` from the next render expecting the default back. Right: apply `new BackgroundColor(defaultColor)` explicitly, or control the look with USS classes (classes DO get removed via the managed class list).
- **`ClassName` splits on spaces** and replaces only Veauty-managed classes; UI Toolkit's own classes (`unity-button`, ...) and externally added ones survive. Don't try to remove external classes by changing the attribute value.
- **`Button.OnClick` replaces `clickable`.** Any custom `Clickable` configuration is discarded on every apply. Configure custom clickables outside Veauty and skip `OnClick`.
- **`DropdownField.Index` notifies** (plain `index` assignment) while `DropdownField.Value` does not. Prefer `Value` for state-driven selection.
- **Slider value clamping.** `Slider.Value` is clamped to the element's current range; `V.Slider` already orders `LowValue`/`HighValue` before `Value` — keep that order in hand-built attribute lists too.
- **Reentrancy.** `setState` during a render never renders recursively; all writes coalesce into one follow-up render. Don't expect the element tree to change until your render function returns.
- **`Element` caching.** An `Element` used directly as `IVTree` caches its childless node on first `Unwrap`; each indexer use builds a fresh node. Don't reuse one `Element` instance expecting different children.
- **updater snapshot.** In the `Func<State,State>` overload, the updater applies to the state captured at render time — two updaters fired from the same rendered frame both start from the same snapshot; the last write wins.

## Extending

- **New control:** create `Lib/Controls/Foo.cs` with `public abstract class FooAttribute<T> : UIAttributeBase<UnityEngine.UIElements.Foo, T>` and `public partial class Foo : UIBase<UnityEngine.UIElements.Foo>` whose constructor is `(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids)`. Add nested attribute classes; use `SetValueWithoutNotify` for value-like properties and `CallbackStore.Set(element, key, callback)` for ChangeEvent handlers. Optionally add a `V.Foo(...)` factory in `Lib/V.cs` returning `new Element(children => new UIToolkit.Foo(attrs, children))`.
- **New attribute for an existing control:** add a nested class extending the control's `XxxAttribute<T>` with a unique diff key, implementing `protected override void Apply(Xxx element)`.
- **New style attribute:** extend `StyleAttribute<TValue>` in `Lib/Attributes.cs` and add the matching chain method to `StyleBuilder` in `Lib/Style.cs`.
- **Custom internal structure:** override `Init`/`AfterRenderKids`/`Destroy` on the control node; remember `Patch` maps virtual children to real children with `childOffset = childCount - kids.Length`, so internal elements must precede virtual children.

## Build & test

Run from the Unity project root (`uloop` needs the Unity Editor running):

```bash
npx --yes uloop-cli@2.2.0 compile --force-recompile true --wait-for-domain-reload true
npx --yes uloop-cli@2.2.0 run-tests --test-mode EditMode   # includes Veauty.UIToolkit.Editor.Tests
npx --yes uloop-cli@2.2.0 run-tests --test-mode PlayMode   # includes Veauty.UIToolkit.Runtime.Tests
```

Test assemblies: `Veauty.UIToolkit.Editor.Tests` (`Tests/Editor/`), `Veauty.UIToolkit.Runtime.Tests` (`Tests/Runtime/`, currently empty).
