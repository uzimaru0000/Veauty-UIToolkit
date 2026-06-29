using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    public abstract class DropdownFieldAttribute<T> : UIAttributeBase<UnityEngine.UIElements.DropdownField, T>
    {
        protected DropdownFieldAttribute(string key, T value) : base(key, value) { }
    }

    public partial class DropdownField : UIBase<UnityEngine.UIElements.DropdownField>
    {
        public DropdownField(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        public partial class Value : DropdownFieldAttribute<string>
        {
            public Value(string value) : base("value", value) { }
            protected override void Apply(UnityEngine.UIElements.DropdownField element)
            {
                element.SetValueWithoutNotify(GetValue());
            }
        }

        public partial class Label : DropdownFieldAttribute<string>
        {
            public Label(string value) : base("label", value) { }
            protected override void Apply(UnityEngine.UIElements.DropdownField element)
            {
                element.label = GetValue();
            }
        }

        public partial class Choices : DropdownFieldAttribute<List<string>>
        {
            public Choices(List<string> value) : base("choices", value) { }
            protected override void Apply(UnityEngine.UIElements.DropdownField element)
            {
                element.choices = GetValue();
            }
        }

        public partial class Index : DropdownFieldAttribute<int>
        {
            public Index(int value) : base("index", value) { }
            protected override void Apply(UnityEngine.UIElements.DropdownField element)
            {
                element.index = GetValue();
            }
        }

        public partial class OnValueChanged : Attribute<VisualElement, EventCallback<ChangeEvent<string>>>
        {
            public OnValueChanged(EventCallback<ChangeEvent<string>> value) : base("onValueChanged", value) { }
            public override void Apply(VisualElement obj)
            {
                if (obj is UnityEngine.UIElements.DropdownField dropdown)
                {
                    CallbackStore.Set(dropdown, "onValueChanged", GetValue());
                }
            }
        }
    }
}
