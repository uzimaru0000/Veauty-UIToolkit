using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    /// <summary>
    /// Base class for attributes targeting <see cref="UnityEngine.UIElements.Button"/> elements.
    /// Silently no-ops on any other element type.
    /// </summary>
    /// <typeparam name="T">The attribute's value type.</typeparam>
    public abstract class ButtonAttribute<T> : UIAttributeBase<UnityEngine.UIElements.Button, T>
    {
        /// <summary>Initializes the attribute with its diff key and value.</summary>
        /// <param name="key">The key used to identify the attribute during diffing.</param>
        /// <param name="value">The value to apply.</param>
        protected ButtonAttribute(string key, T value) : base(key, value) { }
    }

    /// <summary>
    /// Virtual-tree node rendering a <see cref="UnityEngine.UIElements.Button"/>.
    /// </summary>
    public partial class Button : UIBase<UnityEngine.UIElements.Button>
    {
        /// <summary>Initializes the button node with attributes and children.</summary>
        /// <param name="attrs">Attributes applied to the rendered button.</param>
        /// <param name="kids">Child virtual trees.</param>
        public Button(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        /// <summary>Sets the button's text.</summary>
        public partial class Text : ButtonAttribute<string>
        {
            /// <summary>Initializes the attribute with the button text.</summary>
            /// <param name="value">The button text.</param>
            public Text(string value) : base("text", value) { }
            /// <summary>Applies the text to the button.</summary>
            /// <param name="element">The target button.</param>
            protected override void Apply(UnityEngine.UIElements.Button element)
            {
                element.text = GetValue();
            }
        }

        /// <summary>Sets the button's click handler.</summary>
        /// <remarks>
        /// Applying this attribute replaces the button's <see cref="UnityEngine.UIElements.Button.clickable"/>
        /// with a new <see cref="Clickable"/> wrapping the action, so the previous handler is discarded
        /// rather than stacked — re-renders never register duplicate click handlers. Any manipulators or
        /// handlers attached to the old <c>clickable</c> outside Veauty are replaced as well.
        /// Silently no-ops when the element is not a <see cref="UnityEngine.UIElements.Button"/>.
        /// </remarks>
        public partial class OnClick : Attribute<VisualElement, System.Action>
        {
            /// <summary>Initializes the attribute with the click action.</summary>
            /// <param name="value">The action invoked on click.</param>
            public OnClick(System.Action value) : base("onClick", value) { }
            /// <summary>Replaces the button's <c>clickable</c> with one wrapping the configured action.</summary>
            /// <param name="obj">The target element.</param>
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
