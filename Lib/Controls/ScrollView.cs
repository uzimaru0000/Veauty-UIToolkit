using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    /// <summary>
    /// Base class for attributes targeting <see cref="UnityEngine.UIElements.ScrollView"/> elements.
    /// Silently no-ops on any other element type.
    /// </summary>
    /// <typeparam name="T">The attribute's value type.</typeparam>
    public abstract class ScrollViewAttribute<T> : UIAttributeBase<UnityEngine.UIElements.ScrollView, T>
    {
        /// <summary>Initializes the attribute with its diff key and value.</summary>
        /// <param name="key">The key used to identify the attribute during diffing.</param>
        /// <param name="value">The value to apply.</param>
        protected ScrollViewAttribute(string key, T value) : base(key, value) { }
    }

    /// <summary>
    /// Virtual-tree node rendering a <see cref="UnityEngine.UIElements.ScrollView"/>.
    /// Children added to a ScrollView are placed in its scrollable content container.
    /// </summary>
    public partial class ScrollView : UIBase<UnityEngine.UIElements.ScrollView>
    {
        /// <summary>Initializes the scroll view node with attributes and children.</summary>
        /// <param name="attrs">Attributes applied to the rendered scroll view.</param>
        /// <param name="kids">Child virtual trees placed in the scrollable content.</param>
        public ScrollView(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        /// <summary>Sets the scroll mode (vertical, horizontal, or both).</summary>
        public partial class Mode : ScrollViewAttribute<ScrollViewMode>
        {
            /// <summary>Initializes the attribute with the scroll mode.</summary>
            /// <param name="value">The scroll mode.</param>
            public Mode(ScrollViewMode value) : base("mode", value) { }
            /// <summary>Applies the scroll mode to the scroll view.</summary>
            /// <param name="element">The target scroll view.</param>
            protected override void Apply(UnityEngine.UIElements.ScrollView element)
            {
                element.mode = GetValue();
            }
        }

        /// <summary>Sets the visibility of the horizontal scroller.</summary>
        public partial class HorizontalScrollerVisibility : ScrollViewAttribute<ScrollerVisibility>
        {
            /// <summary>Initializes the attribute with the scroller visibility.</summary>
            /// <param name="value">The scroller visibility.</param>
            public HorizontalScrollerVisibility(ScrollerVisibility value) : base("horizontalScrollerVisibility", value) { }
            /// <summary>Applies the visibility to the horizontal scroller.</summary>
            /// <param name="element">The target scroll view.</param>
            protected override void Apply(UnityEngine.UIElements.ScrollView element)
            {
                element.horizontalScrollerVisibility = GetValue();
            }
        }

        /// <summary>Sets the visibility of the vertical scroller.</summary>
        public partial class VerticalScrollerVisibility : ScrollViewAttribute<ScrollerVisibility>
        {
            /// <summary>Initializes the attribute with the scroller visibility.</summary>
            /// <param name="value">The scroller visibility.</param>
            public VerticalScrollerVisibility(ScrollerVisibility value) : base("verticalScrollerVisibility", value) { }
            /// <summary>Applies the visibility to the vertical scroller.</summary>
            /// <param name="element">The target scroll view.</param>
            protected override void Apply(UnityEngine.UIElements.ScrollView element)
            {
                element.verticalScrollerVisibility = GetValue();
            }
        }

        /// <summary>Sets the elastic (overscroll) behavior amount.</summary>
        public partial class Elasticity : ScrollViewAttribute<float>
        {
            /// <summary>Initializes the attribute with the elasticity value.</summary>
            /// <param name="value">The elasticity value.</param>
            public Elasticity(float value) : base("elasticity", value) { }
            /// <summary>Applies the elasticity to the scroll view.</summary>
            /// <param name="element">The target scroll view.</param>
            protected override void Apply(UnityEngine.UIElements.ScrollView element)
            {
                element.elasticity = GetValue();
            }
        }
    }
}
