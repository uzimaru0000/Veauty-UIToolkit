using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    public abstract class ToggleAttribute<T> : UIAttributeBase<UnityEngine.UIElements.Toggle, T>
    {
        protected ToggleAttribute(string key, T value) : base(key, value) { }
    }

    public partial class Toggle : UIBase<UnityEngine.UIElements.Toggle>
    {
        public Toggle(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        public partial class Value : ToggleAttribute<bool>
        {
            public Value(bool value) : base("value", value) { }
            protected override void Apply(UnityEngine.UIElements.Toggle element)
            {
                element.SetValueWithoutNotify(GetValue());
            }
        }

        public partial class Label : ToggleAttribute<string>
        {
            public Label(string value) : base("label", value) { }
            protected override void Apply(UnityEngine.UIElements.Toggle element)
            {
                element.label = GetValue();
            }
        }

        public partial class OnValueChanged : Attribute<VisualElement, EventCallback<ChangeEvent<bool>>>
        {
            public OnValueChanged(EventCallback<ChangeEvent<bool>> value) : base("onValueChanged", value) { }
            public override void Apply(VisualElement obj)
            {
                if (obj is UnityEngine.UIElements.Toggle toggle)
                {
                    CallbackStore.Set(toggle, "onValueChanged", GetValue());
                }
            }
        }
    }
}
