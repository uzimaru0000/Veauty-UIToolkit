using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    /// <summary>
    /// Base class for attributes targeting <see cref="UnityEngine.UIElements.Label"/> elements.
    /// Silently no-ops on any other element type.
    /// </summary>
    /// <typeparam name="T">The attribute's value type.</typeparam>
    public abstract class LabelAttribute<T> : UIAttributeBase<UnityEngine.UIElements.Label, T>
    {
        /// <summary>Initializes the attribute with its diff key and value.</summary>
        /// <param name="key">The key used to identify the attribute during diffing.</param>
        /// <param name="value">The value to apply.</param>
        protected LabelAttribute(string key, T value) : base(key, value) { }
    }

    /// <summary>
    /// Virtual-tree node rendering a <see cref="UnityEngine.UIElements.Label"/>.
    /// </summary>
    public partial class Label : UIBase<UnityEngine.UIElements.Label>
    {
        /// <summary>Initializes the label node with attributes and children.</summary>
        /// <param name="attrs">Attributes applied to the rendered label.</param>
        /// <param name="kids">Child virtual trees.</param>
        public Label(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        /// <summary>Sets the label's text.</summary>
        public partial class Text : LabelAttribute<string>
        {
            /// <summary>Initializes the attribute with the label text.</summary>
            /// <param name="value">The label text.</param>
            public Text(string value) : base("text", value) { }
            /// <summary>Applies the text to the label.</summary>
            /// <param name="element">The target label.</param>
            protected override void Apply(UnityEngine.UIElements.Label element)
            {
                element.text = GetValue();
            }
        }

        /// <summary>Enables or disables rich text markup in the label.</summary>
        public partial class EnableRichText : LabelAttribute<bool>
        {
            /// <summary>Initializes the attribute with the rich text flag.</summary>
            /// <param name="value"><see langword="true"/> to enable rich text tags.</param>
            public EnableRichText(bool value) : base("enableRichText", value) { }
            /// <summary>Applies the rich text flag to the label.</summary>
            /// <param name="element">The target label.</param>
            protected override void Apply(UnityEngine.UIElements.Label element)
            {
                element.enableRichText = GetValue();
            }
        }
    }
}
