using System;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    public static class Renderer
    {
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
