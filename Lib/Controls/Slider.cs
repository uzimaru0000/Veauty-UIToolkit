using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    public abstract class SliderAttribute<T> : UIAttributeBase<UnityEngine.UIElements.Slider, T>
    {
        protected SliderAttribute(string key, T value) : base(key, value) { }
    }

    public partial class Slider : UIBase<UnityEngine.UIElements.Slider>
    {
        public Slider(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        public partial class Value : SliderAttribute<float>
        {
            public Value(float value) : base("value", value) { }
            protected override void Apply(UnityEngine.UIElements.Slider element)
            {
                element.SetValueWithoutNotify(GetValue());
            }
        }

        public partial class LowValue : SliderAttribute<float>
        {
            public LowValue(float value) : base("lowValue", value) { }
            protected override void Apply(UnityEngine.UIElements.Slider element)
            {
                element.lowValue = GetValue();
            }
        }

        public partial class HighValue : SliderAttribute<float>
        {
            public HighValue(float value) : base("highValue", value) { }
            protected override void Apply(UnityEngine.UIElements.Slider element)
            {
                element.highValue = GetValue();
            }
        }

        public partial class Direction : SliderAttribute<SliderDirection>
        {
            public Direction(SliderDirection value) : base("direction", value) { }
            protected override void Apply(UnityEngine.UIElements.Slider element)
            {
                element.direction = GetValue();
            }
        }

        public partial class ShowInputField : SliderAttribute<bool>
        {
            public ShowInputField(bool value) : base("showInputField", value) { }
            protected override void Apply(UnityEngine.UIElements.Slider element)
            {
                element.showInputField = GetValue();
            }
        }

        public partial class Label : SliderAttribute<string>
        {
            public Label(string value) : base("label", value) { }
            protected override void Apply(UnityEngine.UIElements.Slider element)
            {
                element.label = GetValue();
            }
        }

        public partial class OnValueChanged : Attribute<VisualElement, EventCallback<ChangeEvent<float>>>
        {
            public OnValueChanged(EventCallback<ChangeEvent<float>> value) : base("onValueChanged", value) { }
            public override void Apply(VisualElement obj)
            {
                if (obj is UnityEngine.UIElements.Slider slider)
                {
                    CallbackStore.Set(slider, "onValueChanged", GetValue());
                }
            }
        }
    }
}
