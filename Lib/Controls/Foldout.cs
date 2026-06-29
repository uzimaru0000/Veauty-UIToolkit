using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    public abstract class FoldoutAttribute<T> : UIAttributeBase<UnityEngine.UIElements.Foldout, T>
    {
        protected FoldoutAttribute(string key, T value) : base(key, value) { }
    }

    public partial class Foldout : UIBase<UnityEngine.UIElements.Foldout>
    {
        public Foldout(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        public partial class Text : FoldoutAttribute<string>
        {
            public Text(string value) : base("text", value) { }
            protected override void Apply(UnityEngine.UIElements.Foldout element)
            {
                element.text = GetValue();
            }
        }

        public partial class Value : FoldoutAttribute<bool>
        {
            public Value(bool value) : base("value", value) { }
            protected override void Apply(UnityEngine.UIElements.Foldout element)
            {
                element.SetValueWithoutNotify(GetValue());
            }
        }
    }
}
