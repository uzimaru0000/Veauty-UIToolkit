using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    public abstract class ButtonAttribute<T> : UIAttributeBase<UnityEngine.UIElements.Button, T>
    {
        protected ButtonAttribute(string key, T value) : base(key, value) { }
    }

    public partial class Button : UIBase<UnityEngine.UIElements.Button>
    {
        public Button(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        public partial class Text : ButtonAttribute<string>
        {
            public Text(string value) : base("text", value) { }
            protected override void Apply(UnityEngine.UIElements.Button element)
            {
                element.text = GetValue();
            }
        }

        public partial class OnClick : Attribute<VisualElement, System.Action>
        {
            public OnClick(System.Action value) : base("onClick", value) { }
            public override void Apply(VisualElement obj)
            {
                if (obj is UnityEngine.UIElements.Button button)
                {
                    button.clickable = new Clickable(GetValue());
                }
            }
        }
    }
}
