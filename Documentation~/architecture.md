# Architecture

[日本語](ja/architecture.md)

How the package turns a virtual tree into a live `VisualElement` hierarchy and keeps it in sync.

## Data flow

```
new VeautyElement<State>(mounter, renderFunc, initialState)
        │
        ▼
┌─ Initial mount (synchronous, in the constructor) ──────────────┐
│  1. renderFunc(state, setState)      → IVTree                  │
│  2. hookRuntime.Resolve<VisualElement>(tree)                   │
│       resolves [Component] function components, runs hooks     │
│  3. Renderer.Render(resolvedTree)    → VisualElement           │
│  4. mounter.Add(rootElement); rootElement.style.flexGrow = 1   │
│  5. hookRuntime.CommitEffects()      (UseEffect callbacks)     │
└─────────────────────────────────────────────────────────────────┘
        │  setState / hook state change / ForceUpdate()
        ▼
┌─ Update ────────────────────────────────────────────────────────┐
│  1. renderFunc(state, setState)      → new IVTree               │
│  2. hookRuntime.Resolve<VisualElement>(newTree)                 │
│  3. Diff<VisualElement>.Calc(oldResolvedTree, newResolvedTree)  │
│                                       → IPatch<VisualElement>[] │
│  4. Patch.Apply(rootElement, oldResolvedTree, patches)          │
│  5. oldResolvedTree = newResolvedTree                           │
│  6. hookRuntime.CommitEffects()                                 │
└─────────────────────────────────────────────────────────────────┘
```

The core package (`com.uzimaru.veauty`) supplies the tree types (`Node`, `KeyedNode`, `FunctionComponentNode`), the hook runtime, and the generic diff. This package supplies the `VisualElement`-specific renderer (`Render.cs`) and patch applicator (`Patch.cs`), plus the control/attribute layer.

## Re-render scheduling

`VeautyElement<State>` guards against reentrancy with an `isRendering` flag:

- Setting state (or a hook requesting a render) **while a render is running** does not start a nested render. Instead a `renderRequested` flag is set.
- When the current render finishes, `FlushPendingRender` runs exactly one follow-up render. Multiple state writes during one render are **coalesced** into that single render.
- `ForceUpdate()` follows the same rule: called mid-render (or before the mount completed), it defers; otherwise it re-renders immediately even if the state did not change.

## Renderer

`Renderer.Render(IVTree)` produces a fresh `VisualElement` subtree:

1. `IVTreeWrapper` values (e.g. a Presets `Element` used without its indexer) are unwrapped first.
2. A bare `FunctionComponentNode` is resolved with a **fresh, throwaway** `HookRuntime` — hook state is not preserved across renders through this path. `VeautyElement` avoids this by resolving whole trees with its persistent runtime *before* rendering or diffing.
3. For a resolved node the order is:
   - **Create** — `ITypedNode.GetComponentType()` is instantiated via `Activator.CreateInstance` (must inherit `VisualElement`, needs a parameterless constructor); untyped nodes become a plain `VisualElement`. The element's `name` is the node's tag.
   - **Init** — each `IHostLifecycle<VisualElement>.Init(ve)` runs (a `UIBase<T>` node is its own single lifecycle handler).
   - **ApplyAttrs** — every attribute's `Apply(ve)` runs.
   - **RenderKids** — children are rendered recursively and `Add`ed.
   - **AfterRenderKids** — each lifecycle's `AfterRenderKids(ve)` runs.

Any other `IVTree` type throws `ArgumentException`.

## Patch application

`Patch.Apply(rootElement, oldVTree, patches)`:

1. **Target binding** — the old virtual tree is walked in step with the real hierarchy; each patch (identified by tree index) is bound to its `VisualElement` via `SetTarget`. A `childOffset = element.childCount - kids.Length` (clamped to 0) maps virtual children onto real children when the control created internal elements of its own (e.g. `ScrollView` scrollers).
2. **Lifecycle teardown** — for `Redraw`, `Remove` (without move entry), and `RemoveLast`, `IHostLifecycle.Destroy` is invoked over the removed subtrees, children first, before the elements are detached.
3. **Application** — each patch mutates the hierarchy:
   - `Attrs` — re-applies changed attributes. Entries that are `null` (attribute removed from the tree) are **skipped**, so a removed attribute's last value stays on the element (reset-not-supported semantics).
   - `Redraw` — renders the new subtree and swaps it in at the same index.
   - `Append` / `RemoveLast` — adds/removes trailing children.
   - `Remove` / `Reorder` — keyed-list removal, insertion, and movement (moved elements are re-parented, not re-created).
   - `Attach` — replaces the element with a new instance of a different `VisualElement` type, keeping the `name`.

If a patch replaces the root element, `Apply` returns the new root and `VeautyElement` stores it.

## Host lifecycle ordering

For a `UIBase<T>` node: `Init` → attributes → children → `AfterRenderKids`, and `Destroy` when its element is removed or redrawn (invoked children-first during teardown). All three default to no-ops.

## Comparison with the uGUI package

Both packages consume the same core (`Diff`, `HookRuntime`, tree types), but the host layer differs structurally:

| | `com.uzimaru.veauty-ugui` | `com.uzimaru.veauty-uitoolkit` |
|---|---|---|
| Host object | `GameObject` + components | `VisualElement` (the element *is* the control) |
| Node → host | `AttachComponent` adds a `Component` of type `U` to a GameObject | `Activator.CreateInstance` of the `VisualElement` subclass itself |
| Typed attribute miss | `GuiAttributeBase` looks up `TComponent`; some component types are **auto-added** when missing (LayoutElement, CanvasGroup, ...) | `UIAttributeBase<TElement, TValue>` **silently no-ops** — there is no component model, nothing is ever auto-added |
| Layout | Layout Group components + RectTransform attributes | Flexbox via inline USS styles (`StyleAttribute<T>`) |
| Internal structure | Complex widgets require explicit sub-components (`Slider.Fill()`, `Toggle.Checkmark()`, ...) read in `Init` | UI Toolkit controls build their own internals; `ISubComponent`/`FindPart<T>()` exist for parity but **no built-in control defines sub-components** |
| Styling | Attribute-per-component-property | USS classes (`ClassName`/`ClassList`), stylesheets (`StyleSheetAttr`), inline styles |
| Entry point | `VeautyObject<State>` (mounts on a GameObject) | `VeautyElement<State>` (mounts on a VisualElement) |

The `childOffset` trick for skipping internal children exists in both patchers; in this package it also covers elements a UI Toolkit control creates for itself.

## See also

- [Elements](elements.md) — the entry point and control catalog.
- [API reference](api-reference.md) — full signatures.
