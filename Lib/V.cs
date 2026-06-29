using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit.Presets
{
    public static class V
    {
        public static IVTree Label(string text,
            string className = null,
            params IAttribute<VisualElement>[] extras)
        {
            var attrs = new List<IAttribute<VisualElement>> { new UIToolkit.Label.Text(text) };
            if (className != null) attrs.Add(new ClassName(className));
            attrs.AddRange(extras);
            return new UIToolkit.Label(attrs);
        }

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

        public static Element Box(string className = null, params IAttribute<VisualElement>[] extras)
        {
            var attrs = BuildAttrs(extras,
                className != null ? new ClassName(className) : null);
            return new Element(children =>
                new Node<VisualElement, VisualElement>("Box", attrs, children));
        }

        public static Element Row(string className = null, params IAttribute<VisualElement>[] extras)
        {
            var attrs = BuildAttrs(extras,
                new FlexDirection(UnityEngine.UIElements.FlexDirection.Row),
                className != null ? new ClassName(className) : null);
            return new Element(children =>
                new Node<VisualElement, VisualElement>("Row", attrs, children));
        }

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
