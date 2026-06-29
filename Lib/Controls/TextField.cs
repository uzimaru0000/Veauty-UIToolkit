using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    public abstract class TextFieldAttribute<T> : UIAttributeBase<UnityEngine.UIElements.TextField, T>
    {
        protected TextFieldAttribute(string key, T value) : base(key, value) { }
    }

    public partial class TextField : UIBase<UnityEngine.UIElements.TextField>
    {
        public TextField(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        public partial class Value : TextFieldAttribute<string>
        {
            public Value(string value) : base("value", value) { }
            protected override void Apply(UnityEngine.UIElements.TextField element)
            {
                element.SetValueWithoutNotify(GetValue());
            }
        }

        public partial class Label : TextFieldAttribute<string>
        {
            public Label(string value) : base("label", value) { }
            protected override void Apply(UnityEngine.UIElements.TextField element)
            {
                element.label = GetValue();
            }
        }

        public partial class IsReadOnly : TextFieldAttribute<bool>
        {
            public IsReadOnly(bool value) : base("isReadOnly", value) { }
            protected override void Apply(UnityEngine.UIElements.TextField element)
            {
                element.isReadOnly = GetValue();
            }
        }

        public partial class Multiline : TextFieldAttribute<bool>
        {
            public Multiline(bool value) : base("multiline", value) { }
            protected override void Apply(UnityEngine.UIElements.TextField element)
            {
                element.multiline = GetValue();
            }
        }

        public partial class MaxLength : TextFieldAttribute<int>
        {
            public MaxLength(int value) : base("maxLength", value) { }
            protected override void Apply(UnityEngine.UIElements.TextField element)
            {
                element.maxLength = GetValue();
            }
        }

        public partial class Placeholder : TextFieldAttribute<string>
        {
            public Placeholder(string value) : base("placeholder", value) { }
            protected override void Apply(UnityEngine.UIElements.TextField element)
            {
                element.textEdition.placeholder = GetValue();
            }
        }

        public partial class OnValueChanged : Attribute<VisualElement, EventCallback<ChangeEvent<string>>>
        {
            public OnValueChanged(EventCallback<ChangeEvent<string>> value) : base("onValueChanged", value) { }
            public override void Apply(VisualElement obj)
            {
                if (obj is UnityEngine.UIElements.TextField tf)
                {
                    CallbackStore.Set(tf, "onValueChanged", GetValue());
                }
            }
        }
    }
}
