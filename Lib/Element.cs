using System.Collections.Generic;
using System.Linq;

namespace Veauty.UIToolkit.Presets
{
    public class Element : IVTreeWrapper
    {
        private readonly System.Func<IVTree[], IVTree> _factory;
        private IVTree _cached;

        internal Element(System.Func<IVTree[], IVTree> factory)
        {
            _factory = factory;
        }

        public IVTree this[params IVTree[] children] => _factory(children);
        public IVTree this[IEnumerable<IVTree> children] => _factory(children.ToArray());

        public VTreeType GetNodeType() => Unwrap().GetNodeType();
        public int GetDescendantsCount() => Unwrap().GetDescendantsCount();

        public IVTree Unwrap()
        {
            if (_cached == null)
                _cached = _factory(System.Array.Empty<IVTree>());
            return _cached;
        }
    }
}
