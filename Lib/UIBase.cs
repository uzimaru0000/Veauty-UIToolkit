using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    public interface ISubComponent { }

    public abstract class UIBase<T> :
        Node<VisualElement, T>,
        IHostLifecycle<VisualElement>,
        IHostLifecycleTree<VisualElement>
        where T : VisualElement
    {
        protected UIBase(IEnumerable<IAttribute<VisualElement>> attrs, params IVTree[] kids)
            : base(typeof(T).FullName, attrs, kids)
        {
        }

        public virtual VisualElement Init(VisualElement ve) => ve;
        public virtual void Destroy(VisualElement ve) { }
        public virtual void AfterRenderKids(VisualElement ve) { }
        public IHostLifecycle<VisualElement>[] GetHostLifecycles()
            => new IHostLifecycle<VisualElement>[] { this };

        protected T FindPart<T>() where T : class
        {
            foreach (var kid in this.kids)
                if (kid is T part) return part;
            return null;
        }
    }

    public abstract class UIAttributeBase<TElement, TValue> : Attribute<VisualElement, TValue> where TElement : VisualElement
    {
        protected UIAttributeBase(string key, TValue value) : base(key, value) { }
        protected abstract void Apply(TElement element);
        public override void Apply(VisualElement obj)
        {
            if (obj is TElement element)
            {
                Apply(element);
            }
        }
    }

    public abstract class StyleAttribute<TValue> : Attribute<VisualElement, TValue>
    {
        protected StyleAttribute(string key, TValue value) : base(key, value) { }
    }
}
