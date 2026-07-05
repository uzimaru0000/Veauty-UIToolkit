using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    /// <summary>
    /// Base class for attributes targeting <see cref="UnityEngine.UIElements.TextField"/> elements.
    /// Silently no-ops on any other element type.
    /// </summary>
    /// <typeparam name="T">The attribute's value type.</typeparam>
    public abstract class TextFieldAttribute<T> : UIAttributeBase<UnityEngine.UIElements.TextField, T>
    {
        /// <summary>Initializes the attribute with its diff key and value.</summary>
        /// <param name="key">The key used to identify the attribute during diffing.</param>
        /// <param name="value">The value to apply.</param>
        protected TextFieldAttribute(string key, T value) : base(key, value) { }
    }

    /// <summary>
    /// Virtual-tree node rendering a <see cref="UnityEngine.UIElements.TextField"/>.
    /// </summary>
    public partial class TextField : UIBase<UnityEngine.UIElements.TextField>
    {
        /// <summary>Initializes the text field node with attributes and children.</summary>
        /// <param name="attrs">Attributes applied to the rendered text field.</param>
        /// <param name="kids">Child virtual trees.</param>
        public TextField(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        /// <summary>Sets the field's text without firing a change event.</summary>
        /// <remarks>
        /// Uses <c>SetValueWithoutNotify</c> so writing the state-derived value back during a
        /// re-render does not re-trigger <see cref="OnValueChanged"/> and cause a feedback loop
        /// while the user is typing.
        /// </remarks>
        public partial class Value : TextFieldAttribute<string>
        {
            /// <summary>Initializes the attribute with the field text.</summary>
            /// <param name="value">The text to set.</param>
            public Value(string value) : base("value", value) { }
            /// <summary>Applies the text to the field without notification.</summary>
            /// <param name="element">The target text field.</param>
            protected override void Apply(UnityEngine.UIElements.TextField element)
            {
                element.SetValueWithoutNotify(GetValue());
            }
        }

        /// <summary>Sets the field's label text.</summary>
        public partial class Label : TextFieldAttribute<string>
        {
            /// <summary>Initializes the attribute with the label text.</summary>
            /// <param name="value">The label text.</param>
            public Label(string value) : base("label", value) { }
            /// <summary>Applies the label to the field.</summary>
            /// <param name="element">The target text field.</param>
            protected override void Apply(UnityEngine.UIElements.TextField element)
            {
                element.label = GetValue();
            }
        }

        /// <summary>Makes the field read-only or editable.</summary>
        public partial class IsReadOnly : TextFieldAttribute<bool>
        {
            /// <summary>Initializes the attribute with the read-only flag.</summary>
            /// <param name="value"><see langword="true"/> to make the field read-only.</param>
            public IsReadOnly(bool value) : base("isReadOnly", value) { }
            /// <summary>Applies the read-only flag to the field.</summary>
            /// <param name="element">The target text field.</param>
            protected override void Apply(UnityEngine.UIElements.TextField element)
            {
                element.isReadOnly = GetValue();
            }
        }

        /// <summary>Enables or disables multiline input.</summary>
        public partial class Multiline : TextFieldAttribute<bool>
        {
            /// <summary>Initializes the attribute with the multiline flag.</summary>
            /// <param name="value"><see langword="true"/> to allow multiple lines.</param>
            public Multiline(bool value) : base("multiline", value) { }
            /// <summary>Applies the multiline flag to the field.</summary>
            /// <param name="element">The target text field.</param>
            protected override void Apply(UnityEngine.UIElements.TextField element)
            {
                element.multiline = GetValue();
            }
        }

        /// <summary>Limits the maximum number of characters.</summary>
        public partial class MaxLength : TextFieldAttribute<int>
        {
            /// <summary>Initializes the attribute with the maximum length.</summary>
            /// <param name="value">The maximum character count.</param>
            public MaxLength(int value) : base("maxLength", value) { }
            /// <summary>Applies the maximum length to the field.</summary>
            /// <param name="element">The target text field.</param>
            protected override void Apply(UnityEngine.UIElements.TextField element)
            {
                element.maxLength = GetValue();
            }
        }

        /// <summary>Sets the placeholder text shown while the field is empty.</summary>
        public partial class Placeholder : TextFieldAttribute<string>
        {
            /// <summary>Initializes the attribute with the placeholder text.</summary>
            /// <param name="value">The placeholder text.</param>
            public Placeholder(string value) : base("placeholder", value) { }
            /// <summary>Applies the placeholder to the field's text edition settings.</summary>
            /// <param name="element">The target text field.</param>
            protected override void Apply(UnityEngine.UIElements.TextField element)
            {
                element.textEdition.placeholder = GetValue();
            }
        }

        /// <summary>Registers a handler for the field's <see cref="ChangeEvent{T}"/> of string.</summary>
        /// <remarks>
        /// Registered through the callback store keyed by (element, "onValueChanged"): re-applying
        /// unregisters the previously stored handler first, so re-renders never stack duplicates.
        /// Silently no-ops when the element is not a <see cref="UnityEngine.UIElements.TextField"/>.
        /// </remarks>
        public partial class OnValueChanged : Attribute<VisualElement, EventCallback<ChangeEvent<string>>>
        {
            /// <summary>Initializes the attribute with the change handler.</summary>
            /// <param name="value">The handler invoked when the text changes.</param>
            public OnValueChanged(EventCallback<ChangeEvent<string>> value) : base("onValueChanged", value) { }
            /// <summary>Registers the handler, replacing any previously registered one.</summary>
            /// <param name="obj">The target element.</param>
            public override void Apply(VisualElement obj)
            {
                if (obj is UnityEngine.UIElements.TextField tf)
                {
                    CallbackStore.Set(tf, "onValueChanged", GetValue());
                }
            }
        }
    }
}
