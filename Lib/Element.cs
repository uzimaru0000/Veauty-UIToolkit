using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit.Presets
{
    public class Element : Widget<VisualElement>
    {
        private readonly System.Func<IVTree[], IVTree> _factory;

        internal Element(System.Func<IVTree[], IVTree> factory)
            : base(System.Array.Empty<IAttribute<VisualElement>>())
        {
            _factory = factory;
        }

        public override IVTree Render() => _factory(System.Array.Empty<IVTree>());
        public override VisualElement Init(VisualElement ve) => ve;
        public override void Destroy(VisualElement ve) { }

        public IVTree Of(params IVTree[] children) => _factory(children);

        public IVTree this[IVTree[] children] => _factory(children);
        public IVTree this[IVTree c0] => _factory(new[] { c0 });
        public IVTree this[IVTree c0, IVTree c1] => _factory(new[] { c0, c1 });
        public IVTree this[IVTree c0, IVTree c1, IVTree c2] => _factory(new[] { c0, c1, c2 });
        public IVTree this[IVTree c0, IVTree c1, IVTree c2, IVTree c3] => _factory(new[] { c0, c1, c2, c3 });
    }
}
