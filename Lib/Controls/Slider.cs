using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    /// <summary>
    /// Base class for attributes targeting <see cref="UnityEngine.UIElements.Slider"/> elements.
    /// Silently no-ops on any other element type.
    /// </summary>
    /// <typeparam name="T">The attribute's value type.</typeparam>
    public abstract class SliderAttribute<T> : UIAttributeBase<UnityEngine.UIElements.Slider, T>
    {
        /// <summary>Initializes the attribute with its diff key and value.</summary>
        /// <param name="key">The key used to identify the attribute during diffing.</param>
        /// <param name="value">The value to apply.</param>
        protected SliderAttribute(string key, T value) : base(key, value) { }
    }

    /// <summary>
    /// Virtual-tree node rendering a <see cref="UnityEngine.UIElements.Slider"/> (float).
    /// </summary>
    public partial class Slider : UIBase<UnityEngine.UIElements.Slider>
    {
        /// <summary>Initializes the slider node with attributes and children.</summary>
        /// <param name="attrs">Attributes applied to the rendered slider.</param>
        /// <param name="kids">Child virtual trees.</param>
        public Slider(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids) : base(attrs, kids) { }

        /// <summary>Sets the slider's current value without firing a change event.</summary>
        /// <remarks>
        /// Uses <c>SetValueWithoutNotify</c> so re-applying the state-derived value does not
        /// re-trigger <see cref="OnValueChanged"/> and cause a feedback loop. The value is clamped
        /// by the slider to the current low/high range, so apply range attributes first.
        /// </remarks>
        public partial class Value : SliderAttribute<float>
        {
            /// <summary>Initializes the attribute with the slider value.</summary>
            /// <param name="value">The value to set.</param>
            public Value(float value) : base("value", value) { }
            /// <summary>Applies the value to the slider without notification.</summary>
            /// <param name="element">The target slider.</param>
            protected override void Apply(UnityEngine.UIElements.Slider element)
            {
                element.SetValueWithoutNotify(GetValue());
            }
        }

        /// <summary>Sets the slider's range minimum.</summary>
        public partial class LowValue : SliderAttribute<float>
        {
            /// <summary>Initializes the attribute with the range minimum.</summary>
            /// <param name="value">The range minimum.</param>
            public LowValue(float value) : base("lowValue", value) { }
            /// <summary>Applies the range minimum to the slider.</summary>
            /// <param name="element">The target slider.</param>
            protected override void Apply(UnityEngine.UIElements.Slider element)
            {
                element.lowValue = GetValue();
            }
        }

        /// <summary>Sets the slider's range maximum.</summary>
        public partial class HighValue : SliderAttribute<float>
        {
            /// <summary>Initializes the attribute with the range maximum.</summary>
            /// <param name="value">The range maximum.</param>
            public HighValue(float value) : base("highValue", value) { }
            /// <summary>Applies the range maximum to the slider.</summary>
            /// <param name="element">The target slider.</param>
            protected override void Apply(UnityEngine.UIElements.Slider element)
            {
                element.highValue = GetValue();
            }
        }

        /// <summary>Sets the slider's orientation (horizontal or vertical).</summary>
        public partial class Direction : SliderAttribute<SliderDirection>
        {
            /// <summary>Initializes the attribute with the orientation.</summary>
            /// <param name="value">The orientation.</param>
            public Direction(SliderDirection value) : base("direction", value) { }
            /// <summary>Applies the orientation to the slider.</summary>
            /// <param name="element">The target slider.</param>
            protected override void Apply(UnityEngine.UIElements.Slider element)
            {
                element.direction = GetValue();
            }
        }

        /// <summary>Shows or hides the numeric input field next to the slider.</summary>
        public partial class ShowInputField : SliderAttribute<bool>
        {
            /// <summary>Initializes the attribute with the input field flag.</summary>
            /// <param name="value"><see langword="true"/> to show the input field.</param>
            public ShowInputField(bool value) : base("showInputField", value) { }
            /// <summary>Applies the input field flag to the slider.</summary>
            /// <param name="element">The target slider.</param>
            protected override void Apply(UnityEngine.UIElements.Slider element)
            {
                element.showInputField = GetValue();
            }
        }

        /// <summary>Sets the slider's label text.</summary>
        public partial class Label : SliderAttribute<string>
        {
            /// <summary>Initializes the attribute with the label text.</summary>
            /// <param name="value">The label text.</param>
            public Label(string value) : base("label", value) { }
            /// <summary>Applies the label to the slider.</summary>
            /// <param name="element">The target slider.</param>
            protected override void Apply(UnityEngine.UIElements.Slider element)
            {
                element.label = GetValue();
            }
        }

        /// <summary>Registers a handler for the slider's <see cref="ChangeEvent{T}"/> of float.</summary>
        /// <remarks>
        /// Registered through the callback store keyed by (element, "onValueChanged"): re-applying
        /// unregisters the previously stored handler first, so re-renders never stack duplicates.
        /// Silently no-ops when the element is not a <see cref="UnityEngine.UIElements.Slider"/>.
        /// </remarks>
        public partial class OnValueChanged : Attribute<VisualElement, EventCallback<ChangeEvent<float>>>
        {
            /// <summary>Initializes the attribute with the change handler.</summary>
            /// <param name="value">The handler invoked when the value changes.</param>
            public OnValueChanged(EventCallback<ChangeEvent<float>> value) : base("onValueChanged", value) { }
            /// <summary>Registers the handler, replacing any previously registered one.</summary>
            /// <param name="obj">The target element.</param>
            public override void Apply(VisualElement obj)
            {
                if (obj is UnityEngine.UIElements.Slider slider)
                {
                    CallbackStore.Set(slider, "onValueChanged", GetValue());
                }
            }
        }
    }
}
