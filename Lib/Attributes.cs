using UnityEngine;
using UnityEngine.UIElements;

namespace Veauty.UIToolkit
{
    /// <summary>
    /// Sets the USS classes managed by Veauty on an element from a space-separated string.
    /// </summary>
    /// <remarks>
    /// The value is split on single spaces and each token is added to the element's class list.
    /// Only classes previously added by Veauty (via <see cref="ClassName"/> or <see cref="ClassList"/>)
    /// are removed on re-apply; classes added externally (e.g. by UI Toolkit itself or user code)
    /// are preserved.
    /// </remarks>
    public class ClassName : Attribute<VisualElement, string>
    {
        /// <summary>Initializes the attribute with a space-separated list of USS class names.</summary>
        /// <param name="value">One or more class names separated by spaces.</param>
        public ClassName(string value) : base("ClassName", value) { }
        /// <summary>Replaces the Veauty-managed classes on the element with the configured ones.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj)
        {
            ManagedClassList.Replace(obj, GetValue().Split(' '));
        }
    }

    /// <summary>
    /// Sets the USS classes managed by Veauty on an element from an array of class names.
    /// </summary>
    /// <remarks>
    /// Only classes previously added by Veauty are removed on re-apply; externally added classes
    /// are preserved. Empty or null entries are ignored.
    /// </remarks>
    public class ClassList : Attribute<VisualElement, string[]>
    {
        /// <summary>Initializes the attribute with the USS class names to apply.</summary>
        /// <param name="value">The class names.</param>
        public ClassList(params string[] value) : base("ClassList", value) { }
        /// <summary>Replaces the Veauty-managed classes on the element with the configured ones.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj)
        {
            ManagedClassList.Replace(obj, GetValue());
        }
    }

    /// <summary>
    /// Attaches a <see cref="UnityEngine.UIElements.StyleSheet"/> asset to an element.
    /// </summary>
    /// <remarks>
    /// The stylesheet is only added when not already present, so re-applying is idempotent.
    /// Removing the attribute does not detach the stylesheet.
    /// </remarks>
    public class StyleSheetAttr : Attribute<VisualElement, UnityEngine.UIElements.StyleSheet>
    {
        /// <summary>Initializes the attribute with the stylesheet asset to attach.</summary>
        /// <param name="value">The stylesheet asset.</param>
        public StyleSheetAttr(UnityEngine.UIElements.StyleSheet value) : base("StyleSheet", value) { }
        /// <summary>Adds the stylesheet to the element when it is not attached yet.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj)
        {
            var ss = GetValue();
            if (!obj.styleSheets.Contains(ss))
                obj.styleSheets.Add(ss);
        }
    }

    /// <summary>Sets <see cref="VisualElement.pickingMode"/> (whether the element receives pointer events).</summary>
    public class PickingMode : Attribute<VisualElement, UnityEngine.UIElements.PickingMode>
    {
        /// <summary>Initializes the attribute with the picking mode.</summary>
        /// <param name="value">The picking mode.</param>
        public PickingMode(UnityEngine.UIElements.PickingMode value) : base("PickingMode", value) { }
        /// <summary>Applies the picking mode to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj)
        {
            obj.pickingMode = GetValue();
        }
    }

    /// <summary>Sets the element's tooltip text.</summary>
    public class Tooltip : Attribute<VisualElement, string>
    {
        /// <summary>Initializes the attribute with the tooltip text.</summary>
        /// <param name="value">The tooltip text.</param>
        public Tooltip(string value) : base("Tooltip", value) { }
        /// <summary>Applies the tooltip text to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj)
        {
            obj.tooltip = GetValue();
        }
    }

    /// <summary>Sets <see cref="VisualElement.visible"/> (hidden elements keep their layout space).</summary>
    public class Visible : Attribute<VisualElement, bool>
    {
        /// <summary>Initializes the attribute with the visibility flag.</summary>
        /// <param name="value"><see langword="true"/> to show the element.</param>
        public Visible(bool value) : base("Visible", value) { }
        /// <summary>Applies the visibility flag to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj)
        {
            obj.visible = GetValue();
        }
    }

    /// <summary>Enables or disables the element via <see cref="VisualElement.SetEnabled"/>.</summary>
    public class Enabled : Attribute<VisualElement, bool>
    {
        /// <summary>Initializes the attribute with the enabled flag.</summary>
        /// <param name="value"><see langword="true"/> to enable the element.</param>
        public Enabled(bool value) : base("Enabled", value) { }
        /// <summary>Applies the enabled flag to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj)
        {
            obj.SetEnabled(GetValue());
        }
    }

    // Style attributes

    /// <summary>Sets the inline <c>flex-grow</c> style.</summary>
    public class FlexGrow : StyleAttribute<float>
    {
        /// <summary>Initializes the attribute with the flex-grow factor.</summary>
        /// <param name="value">The flex-grow factor.</param>
        public FlexGrow(float value) : base("FlexGrow", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.flexGrow = GetValue(); }
    }

    /// <summary>Sets the inline <c>flex-shrink</c> style.</summary>
    public class FlexShrink : StyleAttribute<float>
    {
        /// <summary>Initializes the attribute with the flex-shrink factor.</summary>
        /// <param name="value">The flex-shrink factor.</param>
        public FlexShrink(float value) : base("FlexShrink", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.flexShrink = GetValue(); }
    }

    /// <summary>Sets the inline <c>flex-direction</c> style.</summary>
    public class FlexDirection : StyleAttribute<UnityEngine.UIElements.FlexDirection>
    {
        /// <summary>Initializes the attribute with the flex direction.</summary>
        /// <param name="value">The flex direction.</param>
        public FlexDirection(UnityEngine.UIElements.FlexDirection value) : base("FlexDirection", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.flexDirection = GetValue(); }
    }

    /// <summary>Sets the inline <c>flex-wrap</c> style.</summary>
    public class FlexWrap : StyleAttribute<Wrap>
    {
        /// <summary>Initializes the attribute with the wrap mode.</summary>
        /// <param name="value">The wrap mode.</param>
        public FlexWrap(Wrap value) : base("FlexWrap", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.flexWrap = GetValue(); }
    }

    /// <summary>Sets the inline <c>justify-content</c> style (main-axis alignment).</summary>
    public class JustifyContent : StyleAttribute<Justify>
    {
        /// <summary>Initializes the attribute with the justification mode.</summary>
        /// <param name="value">The justification mode.</param>
        public JustifyContent(Justify value) : base("JustifyContent", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.justifyContent = GetValue(); }
    }

    /// <summary>Sets the inline <c>align-items</c> style (cross-axis alignment of children).</summary>
    public class AlignItems : StyleAttribute<Align>
    {
        /// <summary>Initializes the attribute with the alignment mode.</summary>
        /// <param name="value">The alignment mode.</param>
        public AlignItems(Align value) : base("AlignItems", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.alignItems = GetValue(); }
    }

    /// <summary>Sets the inline <c>align-self</c> style (cross-axis alignment of this element).</summary>
    public class AlignSelf : StyleAttribute<Align>
    {
        /// <summary>Initializes the attribute with the alignment mode.</summary>
        /// <param name="value">The alignment mode.</param>
        public AlignSelf(Align value) : base("AlignSelf", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.alignSelf = GetValue(); }
    }

    /// <summary>Sets the inline <c>width</c> style.</summary>
    public class Width : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the width.</summary>
        /// <param name="value">The width.</param>
        public Width(StyleLength value) : base("Width", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.width = GetValue(); }
    }

    /// <summary>Sets the inline <c>height</c> style.</summary>
    public class Height : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the height.</summary>
        /// <param name="value">The height.</param>
        public Height(StyleLength value) : base("Height", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.height = GetValue(); }
    }

    /// <summary>Sets the inline <c>min-width</c> style.</summary>
    public class MinWidth : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the minimum width.</summary>
        /// <param name="value">The minimum width.</param>
        public MinWidth(StyleLength value) : base("MinWidth", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.minWidth = GetValue(); }
    }

    /// <summary>Sets the inline <c>min-height</c> style.</summary>
    public class MinHeight : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the minimum height.</summary>
        /// <param name="value">The minimum height.</param>
        public MinHeight(StyleLength value) : base("MinHeight", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.minHeight = GetValue(); }
    }

    /// <summary>Sets the inline <c>max-width</c> style.</summary>
    public class MaxWidth : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the maximum width.</summary>
        /// <param name="value">The maximum width.</param>
        public MaxWidth(StyleLength value) : base("MaxWidth", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.maxWidth = GetValue(); }
    }

    /// <summary>Sets the inline <c>max-height</c> style.</summary>
    public class MaxHeight : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the maximum height.</summary>
        /// <param name="value">The maximum height.</param>
        public MaxHeight(StyleLength value) : base("MaxHeight", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.maxHeight = GetValue(); }
    }

    /// <summary>Sets the inline <c>margin-top</c> style.</summary>
    public class MarginTop : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the top margin.</summary>
        /// <param name="value">The top margin.</param>
        public MarginTop(StyleLength value) : base("MarginTop", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.marginTop = GetValue(); }
    }

    /// <summary>Sets the inline <c>margin-bottom</c> style.</summary>
    public class MarginBottom : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the bottom margin.</summary>
        /// <param name="value">The bottom margin.</param>
        public MarginBottom(StyleLength value) : base("MarginBottom", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.marginBottom = GetValue(); }
    }

    /// <summary>Sets the inline <c>margin-left</c> style.</summary>
    public class MarginLeft : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the left margin.</summary>
        /// <param name="value">The left margin.</param>
        public MarginLeft(StyleLength value) : base("MarginLeft", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.marginLeft = GetValue(); }
    }

    /// <summary>Sets the inline <c>margin-right</c> style.</summary>
    public class MarginRight : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the right margin.</summary>
        /// <param name="value">The right margin.</param>
        public MarginRight(StyleLength value) : base("MarginRight", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.marginRight = GetValue(); }
    }

    /// <summary>Sets the inline <c>padding-top</c> style.</summary>
    public class PaddingTop : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the top padding.</summary>
        /// <param name="value">The top padding.</param>
        public PaddingTop(StyleLength value) : base("PaddingTop", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.paddingTop = GetValue(); }
    }

    /// <summary>Sets the inline <c>padding-bottom</c> style.</summary>
    public class PaddingBottom : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the bottom padding.</summary>
        /// <param name="value">The bottom padding.</param>
        public PaddingBottom(StyleLength value) : base("PaddingBottom", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.paddingBottom = GetValue(); }
    }

    /// <summary>Sets the inline <c>padding-left</c> style.</summary>
    public class PaddingLeft : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the left padding.</summary>
        /// <param name="value">The left padding.</param>
        public PaddingLeft(StyleLength value) : base("PaddingLeft", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.paddingLeft = GetValue(); }
    }

    /// <summary>Sets the inline <c>padding-right</c> style.</summary>
    public class PaddingRight : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the right padding.</summary>
        /// <param name="value">The right padding.</param>
        public PaddingRight(StyleLength value) : base("PaddingRight", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.paddingRight = GetValue(); }
    }

    /// <summary>Sets the inline <c>background-color</c> style.</summary>
    public class BackgroundColor : StyleAttribute<UnityEngine.Color>
    {
        /// <summary>Initializes the attribute with the background color.</summary>
        /// <param name="value">The background color.</param>
        public BackgroundColor(UnityEngine.Color value) : base("BackgroundColor", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.backgroundColor = GetValue(); }
    }

    /// <summary>Sets the inline border color on all four sides.</summary>
    public class BorderColor : StyleAttribute<UnityEngine.Color>
    {
        /// <summary>Initializes the attribute with the border color.</summary>
        /// <param name="value">The border color applied to all sides.</param>
        public BorderColor(UnityEngine.Color value) : base("BorderColor", value) { }
        /// <summary>Applies the color to the top, bottom, left, and right border.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj)
        {
            var c = GetValue();
            obj.style.borderTopColor = c;
            obj.style.borderBottomColor = c;
            obj.style.borderLeftColor = c;
            obj.style.borderRightColor = c;
        }
    }

    /// <summary>Sets the inline border width on all four sides.</summary>
    public class BorderWidth : StyleAttribute<float>
    {
        /// <summary>Initializes the attribute with the border width.</summary>
        /// <param name="value">The border width applied to all sides.</param>
        public BorderWidth(float value) : base("BorderWidth", value) { }
        /// <summary>Applies the width to the top, bottom, left, and right border.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj)
        {
            var w = GetValue();
            obj.style.borderTopWidth = w;
            obj.style.borderBottomWidth = w;
            obj.style.borderLeftWidth = w;
            obj.style.borderRightWidth = w;
        }
    }

    /// <summary>Sets the inline border radius on all four corners.</summary>
    public class BorderRadius : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the corner radius.</summary>
        /// <param name="value">The radius applied to all corners.</param>
        public BorderRadius(StyleLength value) : base("BorderRadius", value) { }
        /// <summary>Applies the radius to all four corners.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj)
        {
            var r = GetValue();
            obj.style.borderTopLeftRadius = r;
            obj.style.borderTopRightRadius = r;
            obj.style.borderBottomLeftRadius = r;
            obj.style.borderBottomRightRadius = r;
        }
    }

    /// <summary>Sets the inline <c>font-size</c> style.</summary>
    public class FontSize : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the font size.</summary>
        /// <param name="value">The font size.</param>
        public FontSize(StyleLength value) : base("FontSize", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.fontSize = GetValue(); }
    }

    /// <summary>Sets the inline text <c>color</c> style.</summary>
    public class TextColor : StyleAttribute<UnityEngine.Color>
    {
        /// <summary>Initializes the attribute with the text color.</summary>
        /// <param name="value">The text color.</param>
        public TextColor(UnityEngine.Color value) : base("Color", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.color = GetValue(); }
    }

    /// <summary>Sets the inline <c>opacity</c> style.</summary>
    public class Opacity : StyleAttribute<float>
    {
        /// <summary>Initializes the attribute with the opacity (0–1).</summary>
        /// <param name="value">The opacity.</param>
        public Opacity(float value) : base("Opacity", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.opacity = GetValue(); }
    }

    /// <summary>Sets the inline <c>display</c> style (<c>Flex</c> or <c>None</c>).</summary>
    public class Display : StyleAttribute<DisplayStyle>
    {
        /// <summary>Initializes the attribute with the display mode.</summary>
        /// <param name="value">The display mode.</param>
        public Display(DisplayStyle value) : base("Display", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.display = GetValue(); }
    }

    /// <summary>Sets the inline <c>overflow</c> style.</summary>
    public class Overflow : StyleAttribute<UnityEngine.UIElements.Overflow>
    {
        /// <summary>Initializes the attribute with the overflow mode.</summary>
        /// <param name="value">The overflow mode.</param>
        public Overflow(UnityEngine.UIElements.Overflow value) : base("Overflow", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.overflow = GetValue(); }
    }

    /// <summary>Sets the inline <c>position</c> style (<c>Relative</c> or <c>Absolute</c>).</summary>
    public class Position : StyleAttribute<UnityEngine.UIElements.Position>
    {
        /// <summary>Initializes the attribute with the position mode.</summary>
        /// <param name="value">The position mode.</param>
        public Position(UnityEngine.UIElements.Position value) : base("Position", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.position = GetValue(); }
    }

    /// <summary>Sets the inline <c>top</c> offset style.</summary>
    public class Top : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the top offset.</summary>
        /// <param name="value">The top offset.</param>
        public Top(StyleLength value) : base("Top", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.top = GetValue(); }
    }

    /// <summary>Sets the inline <c>bottom</c> offset style.</summary>
    public class Bottom : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the bottom offset.</summary>
        /// <param name="value">The bottom offset.</param>
        public Bottom(StyleLength value) : base("Bottom", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.bottom = GetValue(); }
    }

    /// <summary>Sets the inline <c>left</c> offset style.</summary>
    public class Left : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the left offset.</summary>
        /// <param name="value">The left offset.</param>
        public Left(StyleLength value) : base("Left", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.left = GetValue(); }
    }

    /// <summary>Sets the inline <c>right</c> offset style.</summary>
    public class Right : StyleAttribute<StyleLength>
    {
        /// <summary>Initializes the attribute with the right offset.</summary>
        /// <param name="value">The right offset.</param>
        public Right(StyleLength value) : base("Right", value) { }
        /// <summary>Applies the style value to the element.</summary>
        /// <param name="obj">The target element.</param>
        public override void Apply(VisualElement obj) { obj.style.right = GetValue(); }
    }
}
