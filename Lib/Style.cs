using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Veauty.UIToolkit
{
    /// <summary>
    /// Fluent builder that collects inline style attributes and produces an
    /// <see cref="IAttribute{T}"/> array to pass to a node's attribute list.
    /// </summary>
    /// <remarks>
    /// Each call appends one (or, for the <see cref="Margin"/>/<see cref="Padding"/> shorthands,
    /// four) attribute instances in call order. The builder converts implicitly to
    /// <c>IAttribute&lt;VisualElement&gt;[]</c>, so an explicit <see cref="Build"/> call is optional.
    /// </remarks>
    /// <example>
    /// <code>
    /// V.Box(extras: Style.Set
    ///     .FlexDirection(UnityEngine.UIElements.FlexDirection.Row)
    ///     .Padding(new StyleLength(16))
    ///     .BackgroundColor(Color.gray)
    ///     .Build())
    /// </code>
    /// </example>
    public class StyleBuilder
    {
        private readonly List<IAttribute<VisualElement>> _attrs = new();

        // Flex
        /// <summary>Adds a <see cref="UIToolkit.FlexGrow"/> attribute.</summary>
        /// <param name="v">The flex-grow factor.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder FlexGrow(float v) { _attrs.Add(new UIToolkit.FlexGrow(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.FlexShrink"/> attribute.</summary>
        /// <param name="v">The flex-shrink factor.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder FlexShrink(float v) { _attrs.Add(new UIToolkit.FlexShrink(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.FlexDirection"/> attribute.</summary>
        /// <param name="v">The flex direction.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder FlexDirection(UnityEngine.UIElements.FlexDirection v) { _attrs.Add(new UIToolkit.FlexDirection(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.FlexWrap"/> attribute.</summary>
        /// <param name="v">The wrap mode.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder FlexWrap(Wrap v) { _attrs.Add(new UIToolkit.FlexWrap(v)); return this; }

        // Alignment
        /// <summary>Adds a <see cref="UIToolkit.JustifyContent"/> attribute.</summary>
        /// <param name="v">The justification mode.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder JustifyContent(Justify v) { _attrs.Add(new UIToolkit.JustifyContent(v)); return this; }
        /// <summary>Adds an <see cref="UIToolkit.AlignItems"/> attribute.</summary>
        /// <param name="v">The alignment mode.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder AlignItems(Align v) { _attrs.Add(new UIToolkit.AlignItems(v)); return this; }
        /// <summary>Adds an <see cref="UIToolkit.AlignSelf"/> attribute.</summary>
        /// <param name="v">The alignment mode.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder AlignSelf(Align v) { _attrs.Add(new UIToolkit.AlignSelf(v)); return this; }

        // Size
        /// <summary>Adds a <see cref="UIToolkit.Width"/> attribute.</summary>
        /// <param name="v">The width.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder Width(StyleLength v) { _attrs.Add(new UIToolkit.Width(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.Height"/> attribute.</summary>
        /// <param name="v">The height.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder Height(StyleLength v) { _attrs.Add(new UIToolkit.Height(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.MinWidth"/> attribute.</summary>
        /// <param name="v">The minimum width.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder MinWidth(StyleLength v) { _attrs.Add(new UIToolkit.MinWidth(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.MinHeight"/> attribute.</summary>
        /// <param name="v">The minimum height.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder MinHeight(StyleLength v) { _attrs.Add(new UIToolkit.MinHeight(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.MaxWidth"/> attribute.</summary>
        /// <param name="v">The maximum width.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder MaxWidth(StyleLength v) { _attrs.Add(new UIToolkit.MaxWidth(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.MaxHeight"/> attribute.</summary>
        /// <param name="v">The maximum height.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder MaxHeight(StyleLength v) { _attrs.Add(new UIToolkit.MaxHeight(v)); return this; }

        // Margin
        /// <summary>Adds a <see cref="UIToolkit.MarginTop"/> attribute.</summary>
        /// <param name="v">The top margin.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder MarginTop(StyleLength v) { _attrs.Add(new UIToolkit.MarginTop(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.MarginBottom"/> attribute.</summary>
        /// <param name="v">The bottom margin.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder MarginBottom(StyleLength v) { _attrs.Add(new UIToolkit.MarginBottom(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.MarginLeft"/> attribute.</summary>
        /// <param name="v">The left margin.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder MarginLeft(StyleLength v) { _attrs.Add(new UIToolkit.MarginLeft(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.MarginRight"/> attribute.</summary>
        /// <param name="v">The right margin.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder MarginRight(StyleLength v) { _attrs.Add(new UIToolkit.MarginRight(v)); return this; }
        /// <summary>Adds margin attributes for all four sides with the same value.</summary>
        /// <param name="v">The margin applied to every side.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder Margin(StyleLength v)
        {
            _attrs.Add(new UIToolkit.MarginTop(v));
            _attrs.Add(new UIToolkit.MarginBottom(v));
            _attrs.Add(new UIToolkit.MarginLeft(v));
            _attrs.Add(new UIToolkit.MarginRight(v));
            return this;
        }

        // Padding
        /// <summary>Adds a <see cref="UIToolkit.PaddingTop"/> attribute.</summary>
        /// <param name="v">The top padding.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder PaddingTop(StyleLength v) { _attrs.Add(new UIToolkit.PaddingTop(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.PaddingBottom"/> attribute.</summary>
        /// <param name="v">The bottom padding.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder PaddingBottom(StyleLength v) { _attrs.Add(new UIToolkit.PaddingBottom(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.PaddingLeft"/> attribute.</summary>
        /// <param name="v">The left padding.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder PaddingLeft(StyleLength v) { _attrs.Add(new UIToolkit.PaddingLeft(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.PaddingRight"/> attribute.</summary>
        /// <param name="v">The right padding.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder PaddingRight(StyleLength v) { _attrs.Add(new UIToolkit.PaddingRight(v)); return this; }
        /// <summary>Adds padding attributes for all four sides with the same value.</summary>
        /// <param name="v">The padding applied to every side.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder Padding(StyleLength v)
        {
            _attrs.Add(new UIToolkit.PaddingTop(v));
            _attrs.Add(new UIToolkit.PaddingBottom(v));
            _attrs.Add(new UIToolkit.PaddingLeft(v));
            _attrs.Add(new UIToolkit.PaddingRight(v));
            return this;
        }

        // Color
        /// <summary>Adds a <see cref="UIToolkit.BackgroundColor"/> attribute.</summary>
        /// <param name="v">The background color.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder BackgroundColor(Color v) { _attrs.Add(new UIToolkit.BackgroundColor(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.TextColor"/> attribute.</summary>
        /// <param name="v">The text color.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder TextColor(Color v) { _attrs.Add(new UIToolkit.TextColor(v)); return this; }
        /// <summary>Adds an <see cref="UIToolkit.Opacity"/> attribute.</summary>
        /// <param name="v">The opacity (0–1).</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder Opacity(float v) { _attrs.Add(new UIToolkit.Opacity(v)); return this; }

        // Border
        /// <summary>Adds a <see cref="UIToolkit.BorderColor"/> attribute (all four sides).</summary>
        /// <param name="v">The border color.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder BorderColor(Color v) { _attrs.Add(new UIToolkit.BorderColor(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.BorderWidth"/> attribute (all four sides).</summary>
        /// <param name="v">The border width.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder BorderWidth(float v) { _attrs.Add(new UIToolkit.BorderWidth(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.BorderRadius"/> attribute (all four corners).</summary>
        /// <param name="v">The corner radius.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder BorderRadius(StyleLength v) { _attrs.Add(new UIToolkit.BorderRadius(v)); return this; }

        // Text
        /// <summary>Adds a <see cref="UIToolkit.FontSize"/> attribute.</summary>
        /// <param name="v">The font size.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder FontSize(StyleLength v) { _attrs.Add(new UIToolkit.FontSize(v)); return this; }

        // Layout
        /// <summary>Adds a <see cref="UIToolkit.Display"/> attribute.</summary>
        /// <param name="v">The display mode.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder Display(DisplayStyle v) { _attrs.Add(new UIToolkit.Display(v)); return this; }
        /// <summary>Adds an <see cref="UIToolkit.Overflow"/> attribute.</summary>
        /// <param name="v">The overflow mode.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder Overflow(UnityEngine.UIElements.Overflow v) { _attrs.Add(new UIToolkit.Overflow(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.Position"/> attribute.</summary>
        /// <param name="v">The position mode.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder Position(UnityEngine.UIElements.Position v) { _attrs.Add(new UIToolkit.Position(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.Top"/> attribute.</summary>
        /// <param name="v">The top offset.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder Top(StyleLength v) { _attrs.Add(new UIToolkit.Top(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.Bottom"/> attribute.</summary>
        /// <param name="v">The bottom offset.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder Bottom(StyleLength v) { _attrs.Add(new UIToolkit.Bottom(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.Left"/> attribute.</summary>
        /// <param name="v">The left offset.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder Left(StyleLength v) { _attrs.Add(new UIToolkit.Left(v)); return this; }
        /// <summary>Adds a <see cref="UIToolkit.Right"/> attribute.</summary>
        /// <param name="v">The right offset.</param>
        /// <returns>This builder for chaining.</returns>
        public StyleBuilder Right(StyleLength v) { _attrs.Add(new UIToolkit.Right(v)); return this; }

        /// <summary>Returns the collected attributes as an array, in the order they were added.</summary>
        /// <returns>The attribute array.</returns>
        public IAttribute<VisualElement>[] Build() => _attrs.ToArray();
        /// <summary>Converts the builder to its attribute array, equivalent to calling <see cref="Build"/>.</summary>
        /// <param name="b">The builder to convert.</param>
        public static implicit operator IAttribute<VisualElement>[](StyleBuilder b) => b.Build();
    }

    /// <summary>
    /// Entry point for the fluent inline-style API.
    /// </summary>
    public static class Style
    {
        /// <summary>Starts a new <see cref="StyleBuilder"/> chain. Returns a fresh builder on each access.</summary>
        public static StyleBuilder Set => new StyleBuilder();
    }
}
