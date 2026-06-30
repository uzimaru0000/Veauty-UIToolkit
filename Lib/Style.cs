using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Veauty.UIToolkit
{
    public class StyleBuilder
    {
        private readonly List<IAttribute<VisualElement>> _attrs = new();

        // Flex
        public StyleBuilder FlexGrow(float v) { _attrs.Add(new UIToolkit.FlexGrow(v)); return this; }
        public StyleBuilder FlexShrink(float v) { _attrs.Add(new UIToolkit.FlexShrink(v)); return this; }
        public StyleBuilder FlexDirection(UnityEngine.UIElements.FlexDirection v) { _attrs.Add(new UIToolkit.FlexDirection(v)); return this; }
        public StyleBuilder FlexWrap(Wrap v) { _attrs.Add(new UIToolkit.FlexWrap(v)); return this; }

        // Alignment
        public StyleBuilder JustifyContent(Justify v) { _attrs.Add(new UIToolkit.JustifyContent(v)); return this; }
        public StyleBuilder AlignItems(Align v) { _attrs.Add(new UIToolkit.AlignItems(v)); return this; }
        public StyleBuilder AlignSelf(Align v) { _attrs.Add(new UIToolkit.AlignSelf(v)); return this; }

        // Size
        public StyleBuilder Width(StyleLength v) { _attrs.Add(new UIToolkit.Width(v)); return this; }
        public StyleBuilder Height(StyleLength v) { _attrs.Add(new UIToolkit.Height(v)); return this; }
        public StyleBuilder MinWidth(StyleLength v) { _attrs.Add(new UIToolkit.MinWidth(v)); return this; }
        public StyleBuilder MinHeight(StyleLength v) { _attrs.Add(new UIToolkit.MinHeight(v)); return this; }
        public StyleBuilder MaxWidth(StyleLength v) { _attrs.Add(new UIToolkit.MaxWidth(v)); return this; }
        public StyleBuilder MaxHeight(StyleLength v) { _attrs.Add(new UIToolkit.MaxHeight(v)); return this; }

        // Margin
        public StyleBuilder MarginTop(StyleLength v) { _attrs.Add(new UIToolkit.MarginTop(v)); return this; }
        public StyleBuilder MarginBottom(StyleLength v) { _attrs.Add(new UIToolkit.MarginBottom(v)); return this; }
        public StyleBuilder MarginLeft(StyleLength v) { _attrs.Add(new UIToolkit.MarginLeft(v)); return this; }
        public StyleBuilder MarginRight(StyleLength v) { _attrs.Add(new UIToolkit.MarginRight(v)); return this; }
        public StyleBuilder Margin(StyleLength v)
        {
            _attrs.Add(new UIToolkit.MarginTop(v));
            _attrs.Add(new UIToolkit.MarginBottom(v));
            _attrs.Add(new UIToolkit.MarginLeft(v));
            _attrs.Add(new UIToolkit.MarginRight(v));
            return this;
        }

        // Padding
        public StyleBuilder PaddingTop(StyleLength v) { _attrs.Add(new UIToolkit.PaddingTop(v)); return this; }
        public StyleBuilder PaddingBottom(StyleLength v) { _attrs.Add(new UIToolkit.PaddingBottom(v)); return this; }
        public StyleBuilder PaddingLeft(StyleLength v) { _attrs.Add(new UIToolkit.PaddingLeft(v)); return this; }
        public StyleBuilder PaddingRight(StyleLength v) { _attrs.Add(new UIToolkit.PaddingRight(v)); return this; }
        public StyleBuilder Padding(StyleLength v)
        {
            _attrs.Add(new UIToolkit.PaddingTop(v));
            _attrs.Add(new UIToolkit.PaddingBottom(v));
            _attrs.Add(new UIToolkit.PaddingLeft(v));
            _attrs.Add(new UIToolkit.PaddingRight(v));
            return this;
        }

        // Color
        public StyleBuilder BackgroundColor(Color v) { _attrs.Add(new UIToolkit.BackgroundColor(v)); return this; }
        public StyleBuilder TextColor(Color v) { _attrs.Add(new UIToolkit.TextColor(v)); return this; }
        public StyleBuilder Opacity(float v) { _attrs.Add(new UIToolkit.Opacity(v)); return this; }

        // Border
        public StyleBuilder BorderColor(Color v) { _attrs.Add(new UIToolkit.BorderColor(v)); return this; }
        public StyleBuilder BorderWidth(float v) { _attrs.Add(new UIToolkit.BorderWidth(v)); return this; }
        public StyleBuilder BorderRadius(StyleLength v) { _attrs.Add(new UIToolkit.BorderRadius(v)); return this; }

        // Text
        public StyleBuilder FontSize(StyleLength v) { _attrs.Add(new UIToolkit.FontSize(v)); return this; }

        // Layout
        public StyleBuilder Display(DisplayStyle v) { _attrs.Add(new UIToolkit.Display(v)); return this; }
        public StyleBuilder Overflow(UnityEngine.UIElements.Overflow v) { _attrs.Add(new UIToolkit.Overflow(v)); return this; }
        public StyleBuilder Position(UnityEngine.UIElements.Position v) { _attrs.Add(new UIToolkit.Position(v)); return this; }
        public StyleBuilder Top(StyleLength v) { _attrs.Add(new UIToolkit.Top(v)); return this; }
        public StyleBuilder Bottom(StyleLength v) { _attrs.Add(new UIToolkit.Bottom(v)); return this; }
        public StyleBuilder Left(StyleLength v) { _attrs.Add(new UIToolkit.Left(v)); return this; }
        public StyleBuilder Right(StyleLength v) { _attrs.Add(new UIToolkit.Right(v)); return this; }

        public IAttribute<VisualElement>[] Build() => _attrs.ToArray();
        public static implicit operator IAttribute<VisualElement>[](StyleBuilder b) => b.Build();
    }

    public static class Style
    {
        public static StyleBuilder Set => new StyleBuilder();
    }
}
