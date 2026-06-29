using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    public abstract class ProgressBarAttribute<T> : UIAttributeBase<UnityEngine.UIElements.ProgressBar, T>
    {
        protected ProgressBarAttribute(string key, T value) : base(key, value) { }
    }

    public partial class ProgressBar : UIBase<UnityEngine.UIElements.ProgressBar>
    {
        public ProgressBar(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        public partial class Value : ProgressBarAttribute<float>
        {
            public Value(float value) : base("value", value) { }
            protected override void Apply(UnityEngine.UIElements.ProgressBar element)
            {
                element.value = GetValue();
            }
        }

        public partial class Title : ProgressBarAttribute<string>
        {
            public Title(string value) : base("title", value) { }
            protected override void Apply(UnityEngine.UIElements.ProgressBar element)
            {
                element.title = GetValue();
            }
        }

        public partial class LowValue : ProgressBarAttribute<float>
        {
            public LowValue(float value) : base("lowValue", value) { }
            protected override void Apply(UnityEngine.UIElements.ProgressBar element)
            {
                element.lowValue = GetValue();
            }
        }

        public partial class HighValue : ProgressBarAttribute<float>
        {
            public HighValue(float value) : base("highValue", value) { }
            protected override void Apply(UnityEngine.UIElements.ProgressBar element)
            {
                element.highValue = GetValue();
            }
        }
    }
}
