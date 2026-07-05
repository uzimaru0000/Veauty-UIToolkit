using System.Collections.Generic;
using System.Linq;

namespace Veauty.UIToolkit.Presets
{
    /// <summary>
    /// Deferred node builder returned by <see cref="V"/> factory methods. Apply the indexer with
    /// children to produce the final tree node, or use the <see cref="Element"/> itself as an
    /// <see cref="IVTree"/> for a node with no children.
    /// </summary>
    /// <remarks>
    /// When used directly as an <see cref="IVTree"/> (without the indexer), the underlying node is
    /// built lazily with zero children on first access and cached; the same instance is returned
    /// afterwards. Each indexer application, by contrast, builds a fresh node and does not affect
    /// the cache.
    /// </remarks>
    /// <example>
    /// <code>
    /// V.Row()[
    ///     V.Label("Left"),
    ///     V.Label("Right")
    /// ]
    /// </code>
    /// </example>
    public class Element : IVTreeWrapper
    {
        private readonly System.Func<IVTree[], IVTree> _factory;
        private IVTree _cached;

        internal Element(System.Func<IVTree[], IVTree> factory)
        {
            _factory = factory;
        }

        /// <summary>Builds the node with the given children.</summary>
        /// <param name="children">The child virtual trees.</param>
        /// <returns>The constructed tree node.</returns>
        public IVTree this[params IVTree[] children] => _factory(children);

        /// <summary>Builds the node with the given child sequence.</summary>
        /// <param name="children">The child virtual trees.</param>
        /// <returns>The constructed tree node.</returns>
        public IVTree this[IEnumerable<IVTree> children] => _factory(children.ToArray());

        /// <summary>Returns the node type of the underlying (childless) node.</summary>
        public VTreeType GetNodeType() => Unwrap().GetNodeType();

        /// <summary>Returns the descendant count of the underlying (childless) node.</summary>
        public int GetDescendantsCount() => Unwrap().GetDescendantsCount();

        /// <summary>
        /// Returns the underlying node built with no children, creating and caching it on first call.
        /// </summary>
        /// <returns>The cached childless tree node.</returns>
        public IVTree Unwrap()
        {
            if (_cached == null)
                _cached = _factory(System.Array.Empty<IVTree>());
            return _cached;
        }
    }
}
