using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    public abstract class LabelAttribute<T> : UIAttributeBase<UnityEngine.UIElements.Label, T>
    {
        protected LabelAttribute(string key, T value) : base(key, value) { }
    }

    public partial class Label : UIBase<UnityEngine.UIElements.Label>
    {
        public Label(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        public partial class Text : LabelAttribute<string>
        {
            public Text(string value) : base("text", value) { }
            protected override void Apply(UnityEngine.UIElements.Label element)
            {
                element.text = GetValue();
            }
        }

        public partial class EnableRichText : LabelAttribute<bool>
        {
            public EnableRichText(bool value) : base("enableRichText", value) { }
            protected override void Apply(UnityEngine.UIElements.Label element)
            {
                element.enableRichText = GetValue();
            }
        }
    }
}
