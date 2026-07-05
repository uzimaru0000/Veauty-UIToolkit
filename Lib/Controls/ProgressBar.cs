using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    /// <summary>
    /// Base class for attributes targeting <see cref="UnityEngine.UIElements.ProgressBar"/> elements.
    /// Silently no-ops on any other element type.
    /// </summary>
    /// <typeparam name="T">The attribute's value type.</typeparam>
    public abstract class ProgressBarAttribute<T> : UIAttributeBase<UnityEngine.UIElements.ProgressBar, T>
    {
        /// <summary>Initializes the attribute with its diff key and value.</summary>
        /// <param name="key">The key used to identify the attribute during diffing.</param>
        /// <param name="value">The value to apply.</param>
        protected ProgressBarAttribute(string key, T value) : base(key, value) { }
    }

    /// <summary>
    /// Virtual-tree node rendering a <see cref="UnityEngine.UIElements.ProgressBar"/>.
    /// </summary>
    public partial class ProgressBar : UIBase<UnityEngine.UIElements.ProgressBar>
    {
        /// <summary>Initializes the progress bar node with attributes and children.</summary>
        /// <param name="attrs">Attributes applied to the rendered progress bar.</param>
        /// <param name="kids">Child virtual trees.</param>
        public ProgressBar(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        /// <summary>Sets the current progress value.</summary>
        public partial class Value : ProgressBarAttribute<float>
        {
            /// <summary>Initializes the attribute with the progress value.</summary>
            /// <param name="value">The progress value.</param>
            public Value(float value) : base("value", value) { }
            /// <summary>Applies the value to the progress bar.</summary>
            /// <param name="element">The target progress bar.</param>
            protected override void Apply(UnityEngine.UIElements.ProgressBar element)
            {
                element.value = GetValue();
            }
        }

        /// <summary>Sets the title displayed on the bar.</summary>
        public partial class Title : ProgressBarAttribute<string>
        {
            /// <summary>Initializes the attribute with the title text.</summary>
            /// <param name="value">The title text.</param>
            public Title(string value) : base("title", value) { }
            /// <summary>Applies the title to the progress bar.</summary>
            /// <param name="element">The target progress bar.</param>
            protected override void Apply(UnityEngine.UIElements.ProgressBar element)
            {
                element.title = GetValue();
            }
        }

        /// <summary>Sets the minimum of the progress range.</summary>
        public partial class LowValue : ProgressBarAttribute<float>
        {
            /// <summary>Initializes the attribute with the range minimum.</summary>
            /// <param name="value">The range minimum.</param>
            public LowValue(float value) : base("lowValue", value) { }
            /// <summary>Applies the range minimum to the progress bar.</summary>
            /// <param name="element">The target progress bar.</param>
            protected override void Apply(UnityEngine.UIElements.ProgressBar element)
            {
                element.lowValue = GetValue();
            }
        }

        /// <summary>Sets the maximum of the progress range.</summary>
        public partial class HighValue : ProgressBarAttribute<float>
        {
            /// <summary>Initializes the attribute with the range maximum.</summary>
            /// <param name="value">The range maximum.</param>
            public HighValue(float value) : base("highValue", value) { }
            /// <summary>Applies the range maximum to the progress bar.</summary>
            /// <param name="element">The target progress bar.</param>
            protected override void Apply(UnityEngine.UIElements.ProgressBar element)
            {
                element.highValue = GetValue();
            }
        }
    }
}
