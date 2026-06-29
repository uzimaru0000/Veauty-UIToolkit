using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    public abstract class ScrollViewAttribute<T> : UIAttributeBase<UnityEngine.UIElements.ScrollView, T>
    {
        protected ScrollViewAttribute(string key, T value) : base(key, value) { }
    }

    public partial class ScrollView : UIBase<UnityEngine.UIElements.ScrollView>
    {
        public ScrollView(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        public partial class Mode : ScrollViewAttribute<ScrollViewMode>
        {
            public Mode(ScrollViewMode value) : base("mode", value) { }
            protected override void Apply(UnityEngine.UIElements.ScrollView element)
            {
                element.mode = GetValue();
            }
        }

        public partial class HorizontalScrollerVisibility : ScrollViewAttribute<ScrollerVisibility>
        {
            public HorizontalScrollerVisibility(ScrollerVisibility value) : base("horizontalScrollerVisibility", value) { }
            protected override void Apply(UnityEngine.UIElements.ScrollView element)
            {
                element.horizontalScrollerVisibility = GetValue();
            }
        }

        public partial class VerticalScrollerVisibility : ScrollViewAttribute<ScrollerVisibility>
        {
            public VerticalScrollerVisibility(ScrollerVisibility value) : base("verticalScrollerVisibility", value) { }
            protected override void Apply(UnityEngine.UIElements.ScrollView element)
            {
                element.verticalScrollerVisibility = GetValue();
            }
        }

        public partial class Elasticity : ScrollViewAttribute<float>
        {
            public Elasticity(float value) : base("elasticity", value) { }
            protected override void Apply(UnityEngine.UIElements.ScrollView element)
            {
                element.elasticity = GetValue();
            }
        }
    }
}
