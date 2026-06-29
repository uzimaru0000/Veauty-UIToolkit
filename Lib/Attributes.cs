using UnityEngine;
using UnityEngine.UIElements;

namespace Veauty.UIToolkit
{
    public class ClassName : Attribute<VisualElement, string>
    {
        public ClassName(string value) : base("ClassName", value) { }
        public override void Apply(VisualElement obj)
        {
            ManagedClassList.Replace(obj, GetValue().Split(' '));
        }
    }

    public class ClassList : Attribute<VisualElement, string[]>
    {
        public ClassList(params string[] value) : base("ClassList", value) { }
        public override void Apply(VisualElement obj)
        {
            ManagedClassList.Replace(obj, GetValue());
        }
    }

    public class StyleSheetAttr : Attribute<VisualElement, UnityEngine.UIElements.StyleSheet>
    {
        public StyleSheetAttr(UnityEngine.UIElements.StyleSheet value) : base("StyleSheet", value) { }
        public override void Apply(VisualElement obj)
        {
            var ss = GetValue();
            if (!obj.styleSheets.Contains(ss))
                obj.styleSheets.Add(ss);
        }
    }

    public class PickingMode : Attribute<VisualElement, UnityEngine.UIElements.PickingMode>
    {
        public PickingMode(UnityEngine.UIElements.PickingMode value) : base("PickingMode", value) { }
        public override void Apply(VisualElement obj)
        {
            obj.pickingMode = GetValue();
        }
    }

    public class Tooltip : Attribute<VisualElement, string>
    {
        public Tooltip(string value) : base("Tooltip", value) { }
        public override void Apply(VisualElement obj)
        {
            obj.tooltip = GetValue();
        }
    }

    public class Visible : Attribute<VisualElement, bool>
    {
        public Visible(bool value) : base("Visible", value) { }
        public override void Apply(VisualElement obj)
        {
            obj.visible = GetValue();
        }
    }

    public class Enabled : Attribute<VisualElement, bool>
    {
        public Enabled(bool value) : base("Enabled", value) { }
        public override void Apply(VisualElement obj)
        {
            obj.SetEnabled(GetValue());
        }
    }

    // Style attributes

    public class FlexGrow : StyleAttribute<float>
    {
        public FlexGrow(float value) : base("FlexGrow", value) { }
        public override void Apply(VisualElement obj) { obj.style.flexGrow = GetValue(); }
    }

    public class FlexShrink : StyleAttribute<float>
    {
        public FlexShrink(float value) : base("FlexShrink", value) { }
        public override void Apply(VisualElement obj) { obj.style.flexShrink = GetValue(); }
    }

    public class FlexDirection : StyleAttribute<UnityEngine.UIElements.FlexDirection>
    {
        public FlexDirection(UnityEngine.UIElements.FlexDirection value) : base("FlexDirection", value) { }
        public override void Apply(VisualElement obj) { obj.style.flexDirection = GetValue(); }
    }

    public class FlexWrap : StyleAttribute<Wrap>
    {
        public FlexWrap(Wrap value) : base("FlexWrap", value) { }
        public override void Apply(VisualElement obj) { obj.style.flexWrap = GetValue(); }
    }

    public class JustifyContent : StyleAttribute<Justify>
    {
        public JustifyContent(Justify value) : base("JustifyContent", value) { }
        public override void Apply(VisualElement obj) { obj.style.justifyContent = GetValue(); }
    }

    public class AlignItems : StyleAttribute<Align>
    {
        public AlignItems(Align value) : base("AlignItems", value) { }
        public override void Apply(VisualElement obj) { obj.style.alignItems = GetValue(); }
    }

    public class AlignSelf : StyleAttribute<Align>
    {
        public AlignSelf(Align value) : base("AlignSelf", value) { }
        public override void Apply(VisualElement obj) { obj.style.alignSelf = GetValue(); }
    }

    public class Width : StyleAttribute<StyleLength>
    {
        public Width(StyleLength value) : base("Width", value) { }
        public override void Apply(VisualElement obj) { obj.style.width = GetValue(); }
    }

    public class Height : StyleAttribute<StyleLength>
    {
        public Height(StyleLength value) : base("Height", value) { }
        public override void Apply(VisualElement obj) { obj.style.height = GetValue(); }
    }

    public class MinWidth : StyleAttribute<StyleLength>
    {
        public MinWidth(StyleLength value) : base("MinWidth", value) { }
        public override void Apply(VisualElement obj) { obj.style.minWidth = GetValue(); }
    }

    public class MinHeight : StyleAttribute<StyleLength>
    {
        public MinHeight(StyleLength value) : base("MinHeight", value) { }
        public override void Apply(VisualElement obj) { obj.style.minHeight = GetValue(); }
    }

    public class MaxWidth : StyleAttribute<StyleLength>
    {
        public MaxWidth(StyleLength value) : base("MaxWidth", value) { }
        public override void Apply(VisualElement obj) { obj.style.maxWidth = GetValue(); }
    }

    public class MaxHeight : StyleAttribute<StyleLength>
    {
        public MaxHeight(StyleLength value) : base("MaxHeight", value) { }
        public override void Apply(VisualElement obj) { obj.style.maxHeight = GetValue(); }
    }

    public class MarginTop : StyleAttribute<StyleLength>
    {
        public MarginTop(StyleLength value) : base("MarginTop", value) { }
        public override void Apply(VisualElement obj) { obj.style.marginTop = GetValue(); }
    }

    public class MarginBottom : StyleAttribute<StyleLength>
    {
        public MarginBottom(StyleLength value) : base("MarginBottom", value) { }
        public override void Apply(VisualElement obj) { obj.style.marginBottom = GetValue(); }
    }

    public class MarginLeft : StyleAttribute<StyleLength>
    {
        public MarginLeft(StyleLength value) : base("MarginLeft", value) { }
        public override void Apply(VisualElement obj) { obj.style.marginLeft = GetValue(); }
    }

    public class MarginRight : StyleAttribute<StyleLength>
    {
        public MarginRight(StyleLength value) : base("MarginRight", value) { }
        public override void Apply(VisualElement obj) { obj.style.marginRight = GetValue(); }
    }

    public class PaddingTop : StyleAttribute<StyleLength>
    {
        public PaddingTop(StyleLength value) : base("PaddingTop", value) { }
        public override void Apply(VisualElement obj) { obj.style.paddingTop = GetValue(); }
    }

    public class PaddingBottom : StyleAttribute<StyleLength>
    {
        public PaddingBottom(StyleLength value) : base("PaddingBottom", value) { }
        public override void Apply(VisualElement obj) { obj.style.paddingBottom = GetValue(); }
    }

    public class PaddingLeft : StyleAttribute<StyleLength>
    {
        public PaddingLeft(StyleLength value) : base("PaddingLeft", value) { }
        public override void Apply(VisualElement obj) { obj.style.paddingLeft = GetValue(); }
    }

    public class PaddingRight : StyleAttribute<StyleLength>
    {
        public PaddingRight(StyleLength value) : base("PaddingRight", value) { }
        public override void Apply(VisualElement obj) { obj.style.paddingRight = GetValue(); }
    }

    public class BackgroundColor : StyleAttribute<UnityEngine.Color>
    {
        public BackgroundColor(UnityEngine.Color value) : base("BackgroundColor", value) { }
        public override void Apply(VisualElement obj) { obj.style.backgroundColor = GetValue(); }
    }

    public class BorderColor : StyleAttribute<UnityEngine.Color>
    {
        public BorderColor(UnityEngine.Color value) : base("BorderColor", value) { }
        public override void Apply(VisualElement obj)
        {
            var c = GetValue();
            obj.style.borderTopColor = c;
            obj.style.borderBottomColor = c;
            obj.style.borderLeftColor = c;
            obj.style.borderRightColor = c;
        }
    }

    public class BorderWidth : StyleAttribute<float>
    {
        public BorderWidth(float value) : base("BorderWidth", value) { }
        public override void Apply(VisualElement obj)
        {
            var w = GetValue();
            obj.style.borderTopWidth = w;
            obj.style.borderBottomWidth = w;
            obj.style.borderLeftWidth = w;
            obj.style.borderRightWidth = w;
        }
    }

    public class BorderRadius : StyleAttribute<StyleLength>
    {
        public BorderRadius(StyleLength value) : base("BorderRadius", value) { }
        public override void Apply(VisualElement obj)
        {
            var r = GetValue();
            obj.style.borderTopLeftRadius = r;
            obj.style.borderTopRightRadius = r;
            obj.style.borderBottomLeftRadius = r;
            obj.style.borderBottomRightRadius = r;
        }
    }

    public class FontSize : StyleAttribute<StyleLength>
    {
        public FontSize(StyleLength value) : base("FontSize", value) { }
        public override void Apply(VisualElement obj) { obj.style.fontSize = GetValue(); }
    }

    public class TextColor : StyleAttribute<UnityEngine.Color>
    {
        public TextColor(UnityEngine.Color value) : base("Color", value) { }
        public override void Apply(VisualElement obj) { obj.style.color = GetValue(); }
    }

    public class Opacity : StyleAttribute<float>
    {
        public Opacity(float value) : base("Opacity", value) { }
        public override void Apply(VisualElement obj) { obj.style.opacity = GetValue(); }
    }

    public class Display : StyleAttribute<DisplayStyle>
    {
        public Display(DisplayStyle value) : base("Display", value) { }
        public override void Apply(VisualElement obj) { obj.style.display = GetValue(); }
    }

    public class Overflow : StyleAttribute<UnityEngine.UIElements.Overflow>
    {
        public Overflow(UnityEngine.UIElements.Overflow value) : base("Overflow", value) { }
        public override void Apply(VisualElement obj) { obj.style.overflow = GetValue(); }
    }

    public class Position : StyleAttribute<UnityEngine.UIElements.Position>
    {
        public Position(UnityEngine.UIElements.Position value) : base("Position", value) { }
        public override void Apply(VisualElement obj) { obj.style.position = GetValue(); }
    }

    public class Top : StyleAttribute<StyleLength>
    {
        public Top(StyleLength value) : base("Top", value) { }
        public override void Apply(VisualElement obj) { obj.style.top = GetValue(); }
    }

    public class Bottom : StyleAttribute<StyleLength>
    {
        public Bottom(StyleLength value) : base("Bottom", value) { }
        public override void Apply(VisualElement obj) { obj.style.bottom = GetValue(); }
    }

    public class Left : StyleAttribute<StyleLength>
    {
        public Left(StyleLength value) : base("Left", value) { }
        public override void Apply(VisualElement obj) { obj.style.left = GetValue(); }
    }

    public class Right : StyleAttribute<StyleLength>
    {
        public Right(StyleLength value) : base("Right", value) { }
        public override void Apply(VisualElement obj) { obj.style.right = GetValue(); }
    }
}
