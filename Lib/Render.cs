using System;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    /// <summary>
    /// Renders a Veauty virtual tree into a fresh UI Toolkit <see cref="VisualElement"/> hierarchy.
    /// </summary>
    /// <remarks>
    /// Used by <see cref="VeautyElement{State}"/> for the initial mount and by <see cref="Patch"/>
    /// when a subtree must be (re)created (redraw, append, reorder inserts).
    /// </remarks>
    public static class Renderer
    {
        /// <summary>
        /// Builds a <see cref="VisualElement"/> hierarchy from the given virtual tree.
        /// </summary>
        /// <param name="vTree">The virtual tree to render. Wrappers (<see cref="IVTreeWrapper"/>) are unwrapped first.</param>
        /// <returns>The root <see cref="VisualElement"/> of the rendered hierarchy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the tree contains a node type this renderer does not support, or when a typed
        /// node's component type does not inherit from <see cref="VisualElement"/>.
        /// </exception>
        /// <remarks>
        /// For each node the order is: create the element (named after the node's tag), run
        /// <c>IHostLifecycle.Init</c>, apply attributes, render children, then run
        /// <c>IHostLifecycle.AfterRenderKids</c>.
        /// A bare <see cref="FunctionComponentNode"/> passed here is resolved with a fresh, throwaway
        /// <see cref="HookRuntime"/>, so its hook state is not preserved across renders. To keep hook
        /// state, mount through <see cref="VeautyElement{State}"/>, which resolves trees with a
        /// persistent runtime before rendering.
        /// </remarks>
        public static VisualElement Render(IVTree vTree)
        {
            if (vTree is IVTreeWrapper wrapper)
            {
                return Render(wrapper.Unwrap());
            }

            if (vTree is FunctionComponentNode)
            {
                var runtime = new HookRuntime();
                var resolved = runtime.Resolve<VisualElement>(vTree);
                var rendered = RenderResolved(resolved);
                runtime.CommitEffects();
                return rendered;
            }

            return RenderResolved(vTree);
        }

        private static VisualElement RenderResolved(IVTree vTree)
        {
            switch (vTree)
            {
                case NodeBase<VisualElement> vNode:
                    var ve = CreateElement(vNode);
                    InitHostLifecycles(ve, vNode);
                    ApplyAttrs(ve, vNode);
                    RenderKids(ve, vNode);
                    AfterRenderKids(ve, vNode);
                    return ve;
                default:
                    throw new ArgumentException($"Unsupported IVTree type: {vTree.GetType()}");
            }
        }

        private static VisualElement CreateElement(NodeBase<VisualElement> node)
        {
            if (node is ITypedNode typedNode)
            {
                var elementType = typedNode.GetComponentType();
                if (!typeof(VisualElement).IsAssignableFrom(elementType))
                {
                    throw new ArgumentException(elementType.FullName + " must inherit from VisualElement.");
                }

                var ve = (VisualElement)Activator.CreateInstance(elementType);
                ve.name = node.tag;
                return ve;
            }

            var element = new VisualElement();
            element.name = node.tag;
            return element;
        }

        private static void RenderKids(VisualElement parent, IVTree tree)
        {
            if (!(tree is IParent p)) return;
            foreach (var kid in p.GetKids())
            {
                parent.Add(Render(kid));
            }
        }

        private static void ApplyAttrs(VisualElement ve, IVTree tree)
        {
            if (!(tree is NodeBase<VisualElement> node)) return;
            foreach (var attr in node.attrs.attrs)
            {
                attr.Value.Apply(ve);
            }
        }

        private static void InitHostLifecycles(VisualElement ve, IVTree tree)
        {
            if (!(tree is IHostLifecycleTree<VisualElement> lifecycleTree)) return;
            foreach (var lifecycle in lifecycleTree.GetHostLifecycles())
            {
                lifecycle.Init(ve);
            }
        }

        private static void AfterRenderKids(VisualElement ve, IVTree tree)
        {
            if (!(tree is IHostLifecycleTree<VisualElement> lifecycleTree)) return;
            foreach (var lifecycle in lifecycleTree.GetHostLifecycles())
            {
                lifecycle.AfterRenderKids(ve);
            }
        }
    }
}
