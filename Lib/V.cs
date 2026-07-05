using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit.Presets
{
    /// <summary>
    /// Static factory for building UI Toolkit virtual trees with named optional parameters.
    /// Most methods return an <see cref="Element"/> whose indexer accepts children.
    /// </summary>
    /// <remarks>
    /// Only the parameters you pass become attributes; omitted parameters add nothing, leaving the
    /// control's own defaults intact. Extra attributes (styles, classes, tooltips, ...) go in the
    /// trailing <c>extras</c> array.
    /// </remarks>
    /// <example>
    /// <code>
    /// V.Column()[
    ///     V.Label("Hello", className: "title"),
    ///     V.Button("Click", onClick: () => Debug.Log("clicked")),
    ///     V.Slider(value: 0.5f, lowValue: 0f, highValue: 1f)
    /// ]
    /// </code>
    /// </example>
    public static class V
    {
        /// <summary>Filters null entries out of a child sequence and returns the rest as an array.</summary>
        /// <param name="children">The candidate children; may be <see langword="null"/>.</param>
        /// <returns>The non-null children, or an empty array.</returns>
        public static IVTree[] Children(IEnumerable<IVTree> children) =>
            children?.Where(child => child != null).ToArray() ?? Array.Empty<IVTree>();

        /// <summary>Filters null entries out of the given children and returns the rest as an array.</summary>
        /// <param name="children">The candidate children.</param>
        /// <returns>The non-null children, or an empty array.</returns>
        public static IVTree[] Children(params IVTree[] children) =>
            Children((IEnumerable<IVTree>)children);

        /// <summary>Builds a <see cref="ClassList"/> attribute, dropping null or whitespace-only names.</summary>
        /// <param name="classNames">The USS class names.</param>
        /// <returns>The class list attribute.</returns>
        public static ClassList Classes(params string[] classNames) =>
            new ClassList(classNames?.Where(c => !string.IsNullOrWhiteSpace(c)).ToArray() ?? Array.Empty<string>());

        /// <summary>Builds a <see cref="ClassList"/> attribute, dropping null or whitespace-only names.</summary>
        /// <param name="classNames">The USS class names; may be <see langword="null"/>.</param>
        /// <returns>The class list attribute.</returns>
        public static ClassList Classes(IEnumerable<string> classNames) =>
            new ClassList(classNames?.Where(c => !string.IsNullOrWhiteSpace(c)).ToArray() ?? Array.Empty<string>());

        /// <summary>Creates a <see cref="UnityEngine.UIElements.Label"/> node displaying text.</summary>
        /// <param name="text">The label text.</param>
        /// <param name="className">Optional USS class name(s), space-separated.</param>
        /// <param name="extras">Additional attributes.</param>
        /// <returns>The label tree node (labels take no children).</returns>
        public static IVTree Label(string text,
            string className = null,
            params IAttribute<VisualElement>[] extras)
        {
            var attrs = new List<IAttribute<VisualElement>> { new UIToolkit.Label.Text(text) };
            if (className != null) attrs.Add(new ClassName(className));
            attrs.AddRange(extras);
            return new UIToolkit.Label(attrs);
        }

        /// <summary>Creates a <see cref="UnityEngine.UIElements.Button"/>. Use the indexer for children.</summary>
        /// <param name="text">Optional button text.</param>
        /// <param name="onClick">Optional click handler; replaces the button's <c>clickable</c>.</param>
        /// <param name="className">Optional USS class name(s), space-separated.</param>
        /// <param name="extras">Additional attributes.</param>
        /// <returns>An <see cref="Element"/> accepting children via its indexer.</returns>
        public static Element Button(
            string text = null,
            Action onClick = null,
            string className = null,
            params IAttribute<VisualElement>[] extras)
        {
            var attrs = BuildAttrs(extras,
                text != null ? new UIToolkit.Button.Text(text) : null,
                onClick != null ? new UIToolkit.Button.OnClick(onClick) : null,
                className != null ? new ClassName(className) : null);
            return new Element(children => new UIToolkit.Button(attrs, children));
        }

        /// <summary>Creates a <see cref="UnityEngine.UIElements.TextField"/>. Use the indexer for children.</summary>
        /// <param name="value">Optional current text, set without firing change events.</param>
        /// <param name="label">Optional field label.</param>
        /// <param name="placeholder">Optional placeholder text shown when empty.</param>
        /// <param name="multiline">Optional multiline flag.</param>
        /// <param name="isReadOnly">Optional read-only flag.</param>
        /// <param name="maxLength">Optional maximum text length.</param>
        /// <param name="onValueChanged">Optional change handler, de-duplicated across re-renders.</param>
        /// <param name="className">Optional USS class name(s), space-separated.</param>
        /// <param name="extras">Additional attributes.</param>
        /// <returns>An <see cref="Element"/> accepting children via its indexer.</returns>
        public static Element TextField(
            string value = null,
            string label = null,
            string placeholder = null,
            bool? multiline = null,
            bool? isReadOnly = null,
            int? maxLength = null,
            EventCallback<ChangeEvent<string>> onValueChanged = null,
            string className = null,
            params IAttribute<VisualElement>[] extras)
        {
            var attrs = BuildAttrs(extras,
                value != null ? new UIToolkit.TextField.Value(value) : null,
                label != null ? new UIToolkit.TextField.Label(label) : null,
                placeholder != null ? new UIToolkit.TextField.Placeholder(placeholder) : null,
                multiline.HasValue ? new UIToolkit.TextField.Multiline(multiline.Value) : null,
                isReadOnly.HasValue ? new UIToolkit.TextField.IsReadOnly(isReadOnly.Value) : null,
                maxLength.HasValue ? new UIToolkit.TextField.MaxLength(maxLength.Value) : null,
                onValueChanged != null ? new UIToolkit.TextField.OnValueChanged(onValueChanged) : null,
                className != null ? new ClassName(className) : null);
            return new Element(children => new UIToolkit.TextField(attrs, children));
        }

        /// <summary>Creates a <see cref="UnityEngine.UIElements.Toggle"/>. Use the indexer for children.</summary>
        /// <param name="value">Optional checked state, set without firing change events.</param>
        /// <param name="label">Optional toggle label.</param>
        /// <param name="onValueChanged">Optional change handler, de-duplicated across re-renders.</param>
        /// <param name="className">Optional USS class name(s), space-separated.</param>
        /// <param name="extras">Additional attributes.</param>
        /// <returns>An <see cref="Element"/> accepting children via its indexer.</returns>
        public static Element Toggle(
            bool? value = null,
            string label = null,
            EventCallback<ChangeEvent<bool>> onValueChanged = null,
            string className = null,
            params IAttribute<VisualElement>[] extras)
        {
            var attrs = BuildAttrs(extras,
                value.HasValue ? new UIToolkit.Toggle.Value(value.Value) : null,
                label != null ? new UIToolkit.Toggle.Label(label) : null,
                onValueChanged != null ? new UIToolkit.Toggle.OnValueChanged(onValueChanged) : null,
                className != null ? new ClassName(className) : null);
            return new Element(children => new UIToolkit.Toggle(attrs, children));
        }

        /// <summary>Creates a <see cref="UnityEngine.UIElements.Slider"/>. Use the indexer for children.</summary>
        /// <param name="value">Optional current value, set without firing change events. Applied after range attributes.</param>
        /// <param name="lowValue">Optional range minimum.</param>
        /// <param name="highValue">Optional range maximum.</param>
        /// <param name="label">Optional slider label.</param>
        /// <param name="direction">Optional orientation.</param>
        /// <param name="showInputField">Optional flag to show a numeric input field.</param>
        /// <param name="onValueChanged">Optional change handler, de-duplicated across re-renders.</param>
        /// <param name="className">Optional USS class name(s), space-separated.</param>
        /// <param name="extras">Additional attributes.</param>
        /// <returns>An <see cref="Element"/> accepting children via its indexer.</returns>
        public static Element Slider(
            float? value = null,
            float? lowValue = null,
            float? highValue = null,
            string label = null,
            SliderDirection? direction = null,
            bool? showInputField = null,
            EventCallback<ChangeEvent<float>> onValueChanged = null,
            string className = null,
            params IAttribute<VisualElement>[] extras)
        {
            var attrs = BuildAttrs(extras,
                lowValue.HasValue ? new UIToolkit.Slider.LowValue(lowValue.Value) : null,
                highValue.HasValue ? new UIToolkit.Slider.HighValue(highValue.Value) : null,
                label != null ? new UIToolkit.Slider.Label(label) : null,
                direction.HasValue ? new UIToolkit.Slider.Direction(direction.Value) : null,
                showInputField.HasValue ? new UIToolkit.Slider.ShowInputField(showInputField.Value) : null,
                onValueChanged != null ? new UIToolkit.Slider.OnValueChanged(onValueChanged) : null,
                value.HasValue ? new UIToolkit.Slider.Value(value.Value) : null,
                className != null ? new ClassName(className) : null);
            return new Element(children => new UIToolkit.Slider(attrs, children));
        }

        /// <summary>Creates a <see cref="UnityEngine.UIElements.ScrollView"/>. Use the indexer for scrollable content.</summary>
        /// <param name="mode">Optional scroll mode (vertical, horizontal, both).</param>
        /// <param name="horizontalScrollerVisibility">Optional horizontal scroller visibility.</param>
        /// <param name="verticalScrollerVisibility">Optional vertical scroller visibility.</param>
        /// <param name="className">Optional USS class name(s), space-separated.</param>
        /// <param name="extras">Additional attributes.</param>
        /// <returns>An <see cref="Element"/> accepting children via its indexer.</returns>
        public static Element ScrollView(
            ScrollViewMode? mode = null,
            ScrollerVisibility? horizontalScrollerVisibility = null,
            ScrollerVisibility? verticalScrollerVisibility = null,
            string className = null,
            params IAttribute<VisualElement>[] extras)
        {
            var attrs = BuildAttrs(extras,
                mode.HasValue ? new UIToolkit.ScrollView.Mode(mode.Value) : null,
                horizontalScrollerVisibility.HasValue ? new UIToolkit.ScrollView.HorizontalScrollerVisibility(horizontalScrollerVisibility.Value) : null,
                verticalScrollerVisibility.HasValue ? new UIToolkit.ScrollView.VerticalScrollerVisibility(verticalScrollerVisibility.Value) : null,
                className != null ? new ClassName(className) : null);
            return new Element(children => new UIToolkit.ScrollView(attrs, children));
        }

        /// <summary>Creates a <see cref="UnityEngine.UIElements.Foldout"/>. Use the indexer for the collapsible content.</summary>
        /// <param name="text">Optional header text.</param>
        /// <param name="value">Optional expanded state, set without firing change events.</param>
        /// <param name="className">Optional USS class name(s), space-separated.</param>
        /// <param name="extras">Additional attributes.</param>
        /// <returns>An <see cref="Element"/> accepting children via its indexer.</returns>
        public static Element Foldout(
            string text = null,
            bool? value = null,
            string className = null,
            params IAttribute<VisualElement>[] extras)
        {
            var attrs = BuildAttrs(extras,
                text != null ? new UIToolkit.Foldout.Text(text) : null,
                value.HasValue ? new UIToolkit.Foldout.Value(value.Value) : null,
                className != null ? new ClassName(className) : null);
            return new Element(children => new UIToolkit.Foldout(attrs, children));
        }

        /// <summary>Creates a <see cref="UnityEngine.UIElements.DropdownField"/>. Use the indexer for children.</summary>
        /// <param name="value">Optional selected value, set without firing change events. Applied after <paramref name="choices"/> and <paramref name="index"/>.</param>
        /// <param name="label">Optional field label.</param>
        /// <param name="choices">Optional list of selectable choices.</param>
        /// <param name="index">Optional selected index.</param>
        /// <param name="onValueChanged">Optional change handler, de-duplicated across re-renders.</param>
        /// <param name="className">Optional USS class name(s), space-separated.</param>
        /// <param name="extras">Additional attributes.</param>
        /// <returns>An <see cref="Element"/> accepting children via its indexer.</returns>
        public static Element DropdownField(
            string value = null,
            string label = null,
            List<string> choices = null,
            int? index = null,
            EventCallback<ChangeEvent<string>> onValueChanged = null,
            string className = null,
            params IAttribute<VisualElement>[] extras)
        {
            var attrs = BuildAttrs(extras,
                choices != null ? new UIToolkit.DropdownField.Choices(choices) : null,
                label != null ? new UIToolkit.DropdownField.Label(label) : null,
                index.HasValue ? new UIToolkit.DropdownField.Index(index.Value) : null,
                onValueChanged != null ? new UIToolkit.DropdownField.OnValueChanged(onValueChanged) : null,
                value != null ? new UIToolkit.DropdownField.Value(value) : null,
                className != null ? new ClassName(className) : null);
            return new Element(children => new UIToolkit.DropdownField(attrs, children));
        }

        /// <summary>Creates a <see cref="UnityEngine.UIElements.ProgressBar"/> node.</summary>
        /// <param name="value">Optional progress value. Applied after the range attributes.</param>
        /// <param name="title">Optional title displayed on the bar.</param>
        /// <param name="lowValue">Optional range minimum.</param>
        /// <param name="highValue">Optional range maximum.</param>
        /// <param name="className">Optional USS class name(s), space-separated.</param>
        /// <param name="extras">Additional attributes.</param>
        /// <returns>The progress bar tree node (takes no children).</returns>
        public static IVTree ProgressBar(
            float? value = null,
            string title = null,
            float? lowValue = null,
            float? highValue = null,
            string className = null,
            params IAttribute<VisualElement>[] extras)
        {
            var attrs = BuildAttrs(extras,
                title != null ? new UIToolkit.ProgressBar.Title(title) : null,
                lowValue.HasValue ? new UIToolkit.ProgressBar.LowValue(lowValue.Value) : null,
                highValue.HasValue ? new UIToolkit.ProgressBar.HighValue(highValue.Value) : null,
                value.HasValue ? new UIToolkit.ProgressBar.Value(value.Value) : null,
                className != null ? new ClassName(className) : null);
            return new UIToolkit.ProgressBar(attrs);
        }

        /// <summary>Creates a plain <see cref="VisualElement"/> container (tag "Box"). Use the indexer for children.</summary>
        /// <param name="className">Optional USS class name(s), space-separated.</param>
        /// <param name="extras">Additional attributes.</param>
        /// <returns>An <see cref="Element"/> accepting children via its indexer.</returns>
        public static Element Box(string className = null, params IAttribute<VisualElement>[] extras)
        {
            var attrs = BuildAttrs(extras,
                className != null ? new ClassName(className) : null);
            return new Element(children =>
                new Node<VisualElement, VisualElement>("Box", attrs, children));
        }

        /// <summary>Creates a container with <c>flex-direction: row</c> (tag "Row"). Use the indexer for children.</summary>
        /// <param name="className">Optional USS class name(s), space-separated.</param>
        /// <param name="extras">Additional attributes; may override the flex direction.</param>
        /// <returns>An <see cref="Element"/> accepting children via its indexer.</returns>
        public static Element Row(string className = null, params IAttribute<VisualElement>[] extras)
        {
            var attrs = BuildAttrs(extras,
                new FlexDirection(UnityEngine.UIElements.FlexDirection.Row),
                className != null ? new ClassName(className) : null);
            return new Element(children =>
                new Node<VisualElement, VisualElement>("Row", attrs, children));
        }

        /// <summary>Creates a container with <c>flex-direction: column</c> (tag "Column"). Use the indexer for children.</summary>
        /// <param name="className">Optional USS class name(s), space-separated.</param>
        /// <param name="extras">Additional attributes; may override the flex direction.</param>
        /// <returns>An <see cref="Element"/> accepting children via its indexer.</returns>
        public static Element Column(string className = null, params IAttribute<VisualElement>[] extras)
        {
            var attrs = BuildAttrs(extras,
                new FlexDirection(UnityEngine.UIElements.FlexDirection.Column),
                className != null ? new ClassName(className) : null);
            return new Element(children =>
                new Node<VisualElement, VisualElement>("Column", attrs, children));
        }

        private static IAttribute<VisualElement>[] BuildAttrs(
            IAttribute<VisualElement>[] extras,
            params IAttribute<VisualElement>[] candidates)
        {
            var attrs = new List<IAttribute<VisualElement>>();
            foreach (var c in candidates)
                if (c != null) attrs.Add(c);
            if (extras != null) attrs.AddRange(extras);
            return attrs.ToArray();
        }
    }
}
