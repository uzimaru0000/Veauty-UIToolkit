# Veauty-UIToolkit

[日本語](ja/com.uzimaru.veauty-uitoolkit.md)

Declarative, React-like UI construction for Unity UI Toolkit (UIElements), built on the Veauty virtual-tree core.

`com.uzimaru.veauty-uitoolkit` renders a Veauty virtual tree (`IVTree`) into a `VisualElement` hierarchy, then keeps it in sync with your state by diffing and patching — no manual element mutation, no UXML required.

## Requirements

- Unity 6000.5 or later (Unity 6)
- `com.uzimaru.veauty` (core package, pulled in automatically as a dependency)

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

## Quick start

```csharp
using UnityEngine;
using UnityEngine.UIElements;
using Veauty;
using Veauty.UIToolkit;
using Veauty.UIToolkit.Presets;

public class CounterPanel : MonoBehaviour
{
    record struct CounterState(int Count);

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var app = new VeautyElement<CounterState>(
            root,
            (state, setState) => V.Column()[
                V.Label($"Count: {state.Count}"),
                V.Button("Increment", () => setState(s => s with { Count = s.Count + 1 }))
            ],
            new CounterState(0)
        );
    }
}
```

`VeautyElement<State>` mounts the tree under `root` and re-renders whenever `setState` is called.

## Namespaces

| Namespace | Role | Key types |
|-----------|------|-----------|
| `Veauty.UIToolkit` | Base layer: entry point, renderer, patcher, control nodes, attributes | `VeautyElement<State>`, `Renderer`, `Patch`, `UIBase<T>`, `StyleAttribute<T>`, `StyleBuilder` |
| `Veauty.UIToolkit.Presets` | Convenience layer: factory with named optional parameters | `V`, `Element` |

## Documentation

- [Getting started](getting-started.md) — install and a minimal working example, explained line by line.
- [Architecture](architecture.md) — render cycle, diff/patch, host lifecycle ordering, and how this package differs from the uGUI package.
- [Elements](elements.md) — `VeautyElement<State>`, the `UIBase<T>` control catalog, the `V` factory, and the `Element` builder.
- [Styling](styling.md) — inline style attributes, the `StyleBuilder` fluent API, USS classes, and stylesheets.
- [Events](events.md) — event handling, callback de-duplication, and feedback-loop prevention.
- [API reference](api-reference.md) — every public type with signatures and member tables.
