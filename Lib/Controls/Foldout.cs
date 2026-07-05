using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    /// <summary>
    /// Base class for attributes targeting <see cref="UnityEngine.UIElements.Foldout"/> elements.
    /// Silently no-ops on any other element type.
    /// </summary>
    /// <typeparam name="T">The attribute's value type.</typeparam>
    public abstract class FoldoutAttribute<T> : UIAttributeBase<UnityEngine.UIElements.Foldout, T>
    {
        /// <summary>Initializes the attribute with its diff key and value.</summary>
        /// <param name="key">The key used to identify the attribute during diffing.</param>
        /// <param name="value">The value to apply.</param>
        protected FoldoutAttribute(string key, T value) : base(key, value) { }
    }

    /// <summary>
    /// Virtual-tree node rendering a <see cref="UnityEngine.UIElements.Foldout"/>.
    /// Children are added to the foldout's collapsible content area.
    /// </summary>
    public partial class Foldout : UIBase<UnityEngine.UIElements.Foldout>
    {
        /// <summary>Initializes the foldout node with attributes and children.</summary>
        /// <param name="attrs">Attributes applied to the rendered foldout.</param>
        /// <param name="kids">Child virtual trees placed in the collapsible content.</param>
        public Foldout(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        /// <summary>Sets the foldout's header text.</summary>
        public partial class Text : FoldoutAttribute<string>
        {
            /// <summary>Initializes the attribute with the header text.</summary>
            /// <param name="value">The header text.</param>
            public Text(string value) : base("text", value) { }
            /// <summary>Applies the text to the foldout.</summary>
            /// <param name="element">The target foldout.</param>
            protected override void Apply(UnityEngine.UIElements.Foldout element)
            {
                element.text = GetValue();
            }
        }

        /// <summary>Sets the expanded state without firing a change event.</summary>
        /// <remarks>
        /// Uses <c>SetValueWithoutNotify</c> so re-applying the state-derived value does not
        /// re-trigger change handlers and cause a feedback loop.
        /// </remarks>
        public partial class Value : FoldoutAttribute<bool>
        {
            /// <summary>Initializes the attribute with the expanded state.</summary>
            /// <param name="value"><see langword="true"/> to expand the foldout.</param>
            public Value(bool value) : base("value", value) { }
            /// <summary>Applies the expanded state to the foldout without notification.</summary>
            /// <param name="element">The target foldout.</param>
            protected override void Apply(UnityEngine.UIElements.Foldout element)
            {
                element.SetValueWithoutNotify(GetValue());
            }
        }
    }
}
