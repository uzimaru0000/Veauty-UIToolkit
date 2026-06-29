using System;
using UnityEngine.UIElements;
using Veauty.VTree;

namespace Veauty.UIToolkit
{
    public static class Renderer
    {
        public static VisualElement Render(IVTree vTree)
        {
            switch (vTree)
            {
                case NodeBase<VisualElement> vNode:
                    var ve = CreateElement(vNode);
                    ApplyAttrs(ve, vNode);
                    RenderKids(ve, vNode);
                    return ve;
                case Widget<VisualElement> widget:
                    return RenderWidget(widget);
                default:
                    throw new ArgumentException($"Unsupported IVTree type: {vTree.GetType()}");
            }
        }

        private static VisualElement RenderWidget(Widget<VisualElement> widget)
        {
            var tree = widget.Render();
            switch (tree)
            {
                case Widget<VisualElement> nest:
                    var ve = Render(nest);
                    widget.Init(ve);
                    widget.AfterRenderKids(ve);
                    return ve;
                case BaseNode<VisualElement> node:
                    var nodeVe = CreateElement(node);
                    widget.Init(nodeVe);
                    ApplyAttrs(nodeVe, node);
                    RenderKids(nodeVe, node);
                    widget.AfterRenderKids(nodeVe);
                    return nodeVe;
                default:
                    throw new ArgumentException($"Unsupported IVTree type: {tree.GetType()}");
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
    }
}
