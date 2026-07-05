# Getting Started

[日本語](ja/getting-started.md)

Install the package and build your first declarative UI Toolkit view.

## Install

Add both the core package and this package to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.uzimaru.veauty": "https://github.com/uzimaru0000/Veauty.git",
    "com.uzimaru.veauty-uitoolkit": "https://github.com/uzimaru0000/Veauty-UIToolkit.git"
  }
}
```

You also need a `UIDocument` in your scene (GameObject > UI Toolkit > UI Document) with a Panel Settings asset assigned.

## Minimal example

```csharp
using UnityEngine;
using UnityEngine.UIElements;
using Veauty;
using Veauty.UIToolkit;
using Veauty.UIToolkit.Presets;

public class HelloVeauty : MonoBehaviour
{
    record struct AppState(int Count);

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var app = new VeautyElement<AppState>(
            root,
            (state, setState) => V.Column()[
                V.Label($"Count: {state.Count}", className: "counter-label"),
                V.Row()[
                    V.Button("-", () => setState(s => s with { Count = s.Count - 1 })),
                    V.Button("+", () => setState(s => s with { Count = s.Count + 1 }))
                ]
            ],
            new AppState(0)
        );
    }
}
```

## Line by line

- `using Veauty.UIToolkit;` — the base layer: `VeautyElement<State>`, control nodes, attributes.
- `using Veauty.UIToolkit.Presets;` — the convenience layer: the `V` factory and `Element` builder.
- `record struct AppState(int Count);` — the state type. `VeautyElement<State>` constrains `State` to a value type (`where State : struct`), so use a `struct` or `record struct`, not a class.
- `GetComponent<UIDocument>().rootVisualElement` — any `VisualElement` works as a mount point; the `UIDocument` root is the usual one.
- `new VeautyElement<AppState>(root, renderFunc, initialState)` — mounts immediately: the render function runs synchronously, the resulting tree is rendered to a `VisualElement`, added under `root`, and given `flexGrow = 1` so it fills the mount area.
- `(state, setState) => ...` — the render function receives the current state and a setter. This overload's `setState` takes an update function `State => State`; another overload takes the new state value directly, and a third takes no setter at all (see [Elements](elements.md#veautyelementstate)).
- `V.Column()[ ... ]` — `V` factory methods return an `Element`; applying the indexer `[...]` with children produces the actual tree node. `V.Column` is a plain container with `flex-direction: column`.
- `V.Label(...)`, `V.Button(...)` — controls with named optional parameters. Only the parameters you pass become attributes.
- `setState(s => s with { Count = s.Count + 1 })` — computes the next state and schedules a re-render. Veauty diffs the new tree against the previous one and patches only what changed (here: the label text).

## Re-rendering

Every `setState` call triggers: render function → diff → patch. Calls made while a render is already running are deferred and coalesced into a single follow-up render — see [Architecture](architecture.md#re-render-scheduling).

## Next steps

- [Elements](elements.md) — full catalog of controls and the `V` factory.
- [Styling](styling.md) — inline styles, USS classes, stylesheets.
- [Events](events.md) — change events and click handlers.
