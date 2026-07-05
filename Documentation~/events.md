# Events

[日本語](ja/events.md)

How Veauty-UIToolkit wires event handlers so re-renders never duplicate callbacks or echo state back into themselves.

## The problem

In a virtual-DOM system, attributes are re-applied on every render where they changed. Naively calling `RegisterCallback` each time would stack a new handler per render; naively writing `value` back would fire the change event, mutate state, and re-render — an infinite feedback loop. This package addresses both.

## Change events: callback de-duplication

`OnValueChanged` attributes (`TextField`, `Toggle`, `Slider`, `DropdownField`) register through an internal callback store keyed by **(element, key)** — the key being the attribute's diff key, e.g. `"onValueChanged"`:

1. If a callback is already stored for this element/key pair, it is **unregistered** first.
2. The new callback is registered and stored under the pair.

So across any number of re-renders, an element holds at most one Veauty-registered handler per key. Handlers you register on the element yourself (different delegate, outside the store) are unaffected.

```csharp
V.TextField(
    value: state.Name,
    onValueChanged: evt => setState(s => s with { Name = evt.newValue })
)
// re-rendering with a new lambda replaces the old handler — no stacking
```

Note: because a fresh lambda is a different delegate with a different hash, an `OnValueChanged` attribute typically diffs as "changed" every render; the store's replace semantics make that cheap and safe.

## Button.OnClick: clickable replacement

`Button.OnClick` takes a plain `System.Action` and applies it by **replacing** the button's `clickable`:

```csharp
button.clickable = new Clickable(action);
```

Same effect as the callback store — the previous handler is discarded, never stacked — but with a stronger caveat: anything else attached to the old `Clickable` (extra activators, manipulator state) is discarded too. If you need custom `Clickable` behavior, set it up outside Veauty and skip `OnClick`.

## Value writes: SetValueWithoutNotify

The value attributes of interactive controls apply via `SetValueWithoutNotify`:

| Attribute | Effect |
|---|---|
| `TextField.Value` | sets text without firing `ChangeEvent<string>` |
| `Toggle.Value` | sets checked state without firing `ChangeEvent<bool>` |
| `Slider.Value` | sets value without firing `ChangeEvent<float>` |
| `DropdownField.Value` | sets selection without firing `ChangeEvent<string>` |
| `Foldout.Value` | sets expanded state without firing `ChangeEvent<bool>` |

This breaks the feedback loop for controlled inputs: user types → `ChangeEvent` → `setState` → re-render writes `state.Name` back into the field → **no** new `ChangeEvent`, because the write is silent.

```
user input ──ChangeEvent──▶ onValueChanged ──setState──▶ re-render
     ▲                                                        │
     └────────── SetValueWithoutNotify (no event) ◀───────────┘
```

Exception: `DropdownField.Index` assigns `element.index` directly, which **does** notify. Prefer `DropdownField.Value` for state-driven selection; use `Index` for one-shot initialization.

## Lifetime

Callback-store entries are keyed by element reference. When an element is removed and re-created (e.g. a `Redraw` patch), the new element starts with no stored callbacks and handlers are registered fresh by the attribute apply.

## See also

- [Elements](elements.md) — which factory parameters map to these attributes.
- [Architecture](architecture.md) — when attributes are (re-)applied.
