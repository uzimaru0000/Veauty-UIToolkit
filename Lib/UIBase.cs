using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    /// <summary>
    /// Marker interface for sub-component data carriers passed as children to a control.
    /// </summary>
    /// <remarks>
    /// Provided for structural parity with the uGUI package's composition model. No built-in
    /// control in this package currently defines sub-components; UI Toolkit controls create
    /// their internal structure themselves.
    /// </remarks>
    public interface ISubComponent { }

    /// <summary>
    /// Base class for virtual-tree nodes that render a concrete UI Toolkit control of type
    /// <typeparamref name="T"/> and participate in the host lifecycle
    /// (<c>Init</c> / <c>AfterRenderKids</c> / <c>Destroy</c>).
    /// </summary>
    /// <typeparam name="T">The <see cref="VisualElement"/> subclass instantiated for this node.</typeparam>
    /// <remarks>
    /// The node's tag is <c>typeof(T).FullName</c>, so nodes backed by different element types
    /// never match during diffing and are redrawn instead. The rendered element is created with
    /// <see cref="System.Activator"/>, so <typeparamref name="T"/> needs a public parameterless
    /// constructor.
    /// </remarks>
    public abstract class UIBase<T> :
        Node<VisualElement, T>,
        IHostLifecycle<VisualElement>,
        IHostLifecycleTree<VisualElement>
        where T : VisualElement
    {
        /// <summary>
        /// Initializes the node with attributes and virtual-tree children.
        /// </summary>
        /// <param name="attrs">Attributes applied to the rendered element.</param>
        /// <param name="kids">Child virtual trees rendered as child elements.</param>
        protected UIBase(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids)
            : base(typeof(T).FullName, attrs, kids)
        {
        }

        /// <summary>
        /// Called after the element is created and before attributes and children are applied.
        /// Override to set up internal structure. The default implementation is a no-op.
        /// </summary>
        /// <param name="ve">The freshly created element.</param>
        /// <returns>The element to continue rendering with.</returns>
        public virtual VisualElement Init(VisualElement ve) => ve;

        /// <summary>
        /// Called when the element is about to be removed from the hierarchy (remove/redraw).
        /// Override to release resources. The default implementation is a no-op.
        /// </summary>
        /// <param name="ve">The element being destroyed.</param>
        public virtual void Destroy(VisualElement ve) { }

        /// <summary>
        /// Called after all virtual-tree children have been rendered and added.
        /// The default implementation is a no-op.
        /// </summary>
        /// <param name="ve">The rendered element.</param>
        public virtual void AfterRenderKids(VisualElement ve) { }

        /// <summary>
        /// Returns this node as its own single host lifecycle handler.
        /// </summary>
        public IHostLifecycle<VisualElement>[] GetHostLifecycles()
            => new IHostLifecycle<VisualElement>[] { this };

        /// <summary>
        /// Finds the first child of this node that is of type <typeparamref name="T"/>,
        /// typically an <see cref="ISubComponent"/> configuration carrier.
        /// </summary>
        /// <typeparam name="T">The child type to look for.</typeparam>
        /// <returns>The first matching child, or <see langword="null"/> when none matches.</returns>
        protected T FindPart<T>() where T : class
        {
            foreach (var kid in this.kids)
                if (kid is T part) return part;
            return null;
        }
    }

    /// <summary>
    /// Base class for attributes that only apply to a specific <see cref="VisualElement"/> subclass.
    /// </summary>
    /// <typeparam name="TElement">The element type the attribute targets.</typeparam>
    /// <typeparam name="TValue">The attribute's value type.</typeparam>
    /// <remarks>
    /// When applied to an element that is not a <typeparamref name="TElement"/>, the attribute
    /// silently does nothing — there is no error and no component is added (unlike the uGUI
    /// package, UI Toolkit has no component model to auto-add to).
    /// </remarks>
    public abstract class UIAttributeBase<TElement, TValue> : Attribute<VisualElement, TValue> where TElement : VisualElement
    {
        /// <summary>
        /// Initializes the attribute with its diff key and value.
        /// </summary>
        /// <param name="key">The key used to identify the attribute during diffing.</param>
        /// <param name="value">The value to apply.</param>
        protected UIAttributeBase(string key, TValue value) : base(key, value) { }

        /// <summary>
        /// Applies the value to a correctly typed element.
        /// </summary>
        /// <param name="element">The target element, already checked to be a <typeparamref name="TElement"/>.</param>
        protected abstract void Apply(TElement element);

        /// <summary>
        /// Applies the attribute when the element is a <typeparamref name="TElement"/>;
        /// otherwise silently no-ops.
        /// </summary>
        /// <param name="obj">The element the attribute is applied to.</param>
        public override void Apply(VisualElement obj)
        {
            if (obj is TElement element)
            {
                Apply(element);
            }
        }
    }

    /// <summary>
    /// Base class for attributes that set inline USS style properties
    /// (<see cref="VisualElement.style"/>) on any element.
    /// </summary>
    /// <typeparam name="TValue">The style value type.</typeparam>
    /// <remarks>
    /// Removing a style attribute from the tree does not reset the style property: the diff emits
    /// a null entry for removed attributes and the patcher skips it, so the last applied inline
    /// value remains on the element.
    /// </remarks>
    public abstract class StyleAttribute<TValue> : Attribute<VisualElement, TValue>
    {
        /// <summary>
        /// Initializes the style attribute with its diff key and value.
        /// </summary>
        /// <param name="key">The key used to identify the attribute during diffing.</param>
        /// <param name="value">The style value to apply.</param>
        protected StyleAttribute(string key, TValue value) : base(key, value) { }
    }
}
