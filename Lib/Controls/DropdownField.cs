using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    /// <summary>
    /// Base class for attributes targeting <see cref="UnityEngine.UIElements.DropdownField"/> elements.
    /// Silently no-ops on any other element type.
    /// </summary>
    /// <typeparam name="T">The attribute's value type.</typeparam>
    public abstract class DropdownFieldAttribute<T> : UIAttributeBase<UnityEngine.UIElements.DropdownField, T>
    {
        /// <summary>Initializes the attribute with its diff key and value.</summary>
        /// <param name="key">The key used to identify the attribute during diffing.</param>
        /// <param name="value">The value to apply.</param>
        protected DropdownFieldAttribute(string key, T value) : base(key, value) { }
    }

    /// <summary>
    /// Virtual-tree node rendering a <see cref="UnityEngine.UIElements.DropdownField"/>.
    /// </summary>
    public partial class DropdownField : UIBase<UnityEngine.UIElements.DropdownField>
    {
        /// <summary>Initializes the dropdown node with attributes and children.</summary>
        /// <param name="attrs">Attributes applied to the rendered dropdown.</param>
        /// <param name="kids">Child virtual trees.</param>
        public DropdownField(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        /// <summary>Sets the selected value without firing a change event.</summary>
        /// <remarks>
        /// Uses <c>SetValueWithoutNotify</c> so re-applying the state-derived value does not
        /// re-trigger <see cref="OnValueChanged"/> and cause a feedback loop.
        /// </remarks>
        public partial class Value : DropdownFieldAttribute<string>
        {
            /// <summary>Initializes the attribute with the selected value.</summary>
            /// <param name="value">The value to select.</param>
            public Value(string value) : base("value", value) { }
            /// <summary>Applies the value to the dropdown without notification.</summary>
            /// <param name="element">The target dropdown.</param>
            protected override void Apply(UnityEngine.UIElements.DropdownField element)
            {
                element.SetValueWithoutNotify(GetValue());
            }
        }

        /// <summary>Sets the dropdown's label text.</summary>
        public partial class Label : DropdownFieldAttribute<string>
        {
            /// <summary>Initializes the attribute with the label text.</summary>
            /// <param name="value">The label text.</param>
            public Label(string value) : base("label", value) { }
            /// <summary>Applies the label to the dropdown.</summary>
            /// <param name="element">The target dropdown.</param>
            protected override void Apply(UnityEngine.UIElements.DropdownField element)
            {
                element.label = GetValue();
            }
        }

        /// <summary>Sets the list of selectable choices.</summary>
        public partial class Choices : DropdownFieldAttribute<List<string>>
        {
            /// <summary>Initializes the attribute with the choices.</summary>
            /// <param name="value">The selectable choices.</param>
            public Choices(List<string> value) : base("choices", value) { }
            /// <summary>Applies the choices to the dropdown.</summary>
            /// <param name="element">The target dropdown.</param>
            protected override void Apply(UnityEngine.UIElements.DropdownField element)
            {
                element.choices = GetValue();
            }
        }

        /// <summary>Sets the selected index.</summary>
        /// <remarks>Setting the index fires the dropdown's change event (unlike <see cref="Value"/>).</remarks>
        public partial class Index : DropdownFieldAttribute<int>
        {
            /// <summary>Initializes the attribute with the selected index.</summary>
            /// <param name="value">The index to select.</param>
            public Index(int value) : base("index", value) { }
            /// <summary>Applies the index to the dropdown.</summary>
            /// <param name="element">The target dropdown.</param>
            protected override void Apply(UnityEngine.UIElements.DropdownField element)
            {
                element.index = GetValue();
            }
        }

        /// <summary>Registers a handler for the dropdown's <see cref="ChangeEvent{T}"/> of string.</summary>
        /// <remarks>
        /// Registered through the callback store keyed by (element, "onValueChanged"): re-applying
        /// unregisters the previously stored handler first, so re-renders never stack duplicates.
        /// Silently no-ops when the element is not a <see cref="UnityEngine.UIElements.DropdownField"/>.
        /// </remarks>
        public partial class OnValueChanged : Attribute<VisualElement, EventCallback<ChangeEvent<string>>>
        {
            /// <summary>Initializes the attribute with the change handler.</summary>
            /// <param name="value">The handler invoked when the value changes.</param>
            public OnValueChanged(EventCallback<ChangeEvent<string>> value) : base("onValueChanged", value) { }
            /// <summary>Registers the handler, replacing any previously registered one.</summary>
            /// <param name="obj">The target element.</param>
            public override void Apply(VisualElement obj)
            {
                if (obj is UnityEngine.UIElements.DropdownField dropdown)
                {
                    CallbackStore.Set(dropdown, "onValueChanged", GetValue());
                }
            }
        }
    }
}
