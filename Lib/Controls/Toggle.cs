using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    /// <summary>
    /// Base class for attributes targeting <see cref="UnityEngine.UIElements.Toggle"/> elements.
    /// Silently no-ops on any other element type.
    /// </summary>
    /// <typeparam name="T">The attribute's value type.</typeparam>
    public abstract class ToggleAttribute<T> : UIAttributeBase<UnityEngine.UIElements.Toggle, T>
    {
        /// <summary>Initializes the attribute with its diff key and value.</summary>
        /// <param name="key">The key used to identify the attribute during diffing.</param>
        /// <param name="value">The value to apply.</param>
        protected ToggleAttribute(string key, T value) : base(key, value) { }
    }

    /// <summary>
    /// Virtual-tree node rendering a <see cref="UnityEngine.UIElements.Toggle"/>.
    /// </summary>
    public partial class Toggle : UIBase<UnityEngine.UIElements.Toggle>
    {
        /// <summary>Initializes the toggle node with attributes and children.</summary>
        /// <param name="attrs">Attributes applied to the rendered toggle.</param>
        /// <param name="kids">Child virtual trees.</param>
        public Toggle(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        /// <summary>Sets the checked state without firing a change event.</summary>
        /// <remarks>
        /// Uses <c>SetValueWithoutNotify</c> so re-applying the state-derived value does not
        /// re-trigger <see cref="OnValueChanged"/> and cause a feedback loop.
        /// </remarks>
        public partial class Value : ToggleAttribute<bool>
        {
            /// <summary>Initializes the attribute with the checked state.</summary>
            /// <param name="value"><see langword="true"/> to check the toggle.</param>
            public Value(bool value) : base("value", value) { }
            /// <summary>Applies the checked state to the toggle without notification.</summary>
            /// <param name="element">The target toggle.</param>
            protected override void Apply(UnityEngine.UIElements.Toggle element)
            {
                element.SetValueWithoutNotify(GetValue());
            }
        }

        /// <summary>Sets the toggle's label text.</summary>
        public partial class Label : ToggleAttribute<string>
        {
            /// <summary>Initializes the attribute with the label text.</summary>
            /// <param name="value">The label text.</param>
            public Label(string value) : base("label", value) { }
            /// <summary>Applies the label to the toggle.</summary>
            /// <param name="element">The target toggle.</param>
            protected override void Apply(UnityEngine.UIElements.Toggle element)
            {
                element.label = GetValue();
            }
        }

        /// <summary>Registers a handler for the toggle's <see cref="ChangeEvent{T}"/> of bool.</summary>
        /// <remarks>
        /// Registered through the callback store keyed by (element, "onValueChanged"): re-applying
        /// unregisters the previously stored handler first, so re-renders never stack duplicates.
        /// Silently no-ops when the element is not a <see cref="UnityEngine.UIElements.Toggle"/>.
        /// </remarks>
        public partial class OnValueChanged : Attribute<VisualElement, EventCallback<ChangeEvent<bool>>>
        {
            /// <summary>Initializes the attribute with the change handler.</summary>
            /// <param name="value">The handler invoked when the value changes.</param>
            public OnValueChanged(EventCallback<ChangeEvent<bool>> value) : base("onValueChanged", value) { }
            /// <summary>Registers the handler, replacing any previously registered one.</summary>
            /// <param name="obj">The target element.</param>
            public override void Apply(VisualElement obj)
            {
                if (obj is UnityEngine.UIElements.Toggle toggle)
                {
                    CallbackStore.Set(toggle, "onValueChanged", GetValue());
                }
            }
        }
    }
}
