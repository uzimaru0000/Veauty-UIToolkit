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

        public IVTree this[params IVTree[] children] => _factory(children);
    }
}
