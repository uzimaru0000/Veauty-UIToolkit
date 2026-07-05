# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.5.0] - 2026-07-01

### Added

- Initial release of the Veauty UI Toolkit package: `VisualElement`-based renderer (`Renderer`), patch applicator (`Patch`), and USS support.
- `VeautyElement<State>` mount point with three render-function overloads: state + value setter, state + `Func<State, State>` updater, and state-only; deferred/coalesced re-rendering and `ForceUpdate()`.
- `UIBase<T>` control nodes with host lifecycle support: `Button`, `DropdownField`, `Foldout`, `Label`, `ProgressBar`, `ScrollView`, `Slider`, `TextField`, `Toggle`, each with typed attribute classes.
- Feedback-loop-safe value attributes (`SetValueWithoutNotify`) and de-duplicated `OnValueChanged` callbacks via an internal callback store; `Button.OnClick` via `clickable` replacement.
- Common attributes: `ClassName`/`ClassList` with managed replace semantics, `StyleSheetAttr`, `PickingMode`, `Tooltip`, `Visible`, `Enabled`.
- Inline USS style attributes (flex, alignment, size, margin/padding, color, border, text, layout, position) and `StyleBuilder` fluent API (`Style.Set ... .Build()`).
- `Veauty.UIToolkit.Presets` layer: `V` factory (`Label`, `Button`, `TextField`, `Toggle`, `Slider`, `ScrollView`, `Foldout`, `DropdownField`, `ProgressBar`, `Box`, `Row`, `Column`, `Children`, `Classes`) and the `Element` builder with a single `params` indexer plus an `IEnumerable<IVTree>` indexer overload.
- English README with Japanese translation (`README.ja.md`).

### Changed

- Ported `UIBase` to the core `Node` + `IHostLifecycle` resolved host tree model.
- Replaced the earlier `Element.Of()` / multiple indexer overloads with the single `params` indexer.

[Unreleased]: https://github.com/uzimaru0000/Veauty-UIToolkit/compare/v0.5.0...HEAD
[0.5.0]: https://github.com/uzimaru0000/Veauty-UIToolkit/releases/tag/v0.5.0
