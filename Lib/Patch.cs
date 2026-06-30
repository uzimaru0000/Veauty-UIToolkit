using System.Collections.Generic;
using UnityEngine.UIElements;
using Veauty.VTree;
using Veauty.Patch;

namespace Veauty.UIToolkit
{
    public static class Patch
    {
        public static VisualElement Apply(VisualElement rootElement, IVTree oldVTree, IPatch<VisualElement>[] patches)
        {
            if (patches.Length == 0)
            {
                return rootElement;
            }

            AddElementNodes(rootElement, oldVTree, ref patches);

            return Helper(rootElement, patches);
        }

        private static void AddElementNodes(in VisualElement element, IVTree vTree, ref IPatch<VisualElement>[] patches)
        {
            AddElementNodesHelper(element, vTree, ref patches, 0, 0, vTree.GetDescendantsCount());
        }

        private static int AddElementNodesHelper(
            in VisualElement element,
            IVTree vTree,
            ref IPatch<VisualElement>[] patches,
            int i,
            int low,
            int high
        )
        {
            var patch = patches[i];
            var index = patch.GetIndex();

            while (index == low)
            {
                switch (patch)
                {
                    case Redraw<VisualElement> redraw:
                        DestroyHostLifecyclesInSubtree(vTree, element);
                        redraw.SetTarget(element);
                        break;
                    case RemoveLast<VisualElement> removeLast:
                        DestroyRemovedLastHostLifecycles(element, vTree, removeLast);
                        removeLast.SetTarget(element);
                        break;
                    case Reorder<VisualElement> reorder:
                        reorder.SetTarget(element);
                        var subPatches = reorder.patches;
                        if (subPatches.Length > 0)
                        {
                            AddElementNodesHelper(element, vTree, ref subPatches, 0, low, high);
                        }
                        break;
                    case Remove<VisualElement> remove:
                        remove.SetTarget(element);
                        if (remove.patches != null && remove.entry != null)
                        {
                            remove.entry.data = element;
                            var removeSubPatches = remove.patches;
                            if (removeSubPatches.Length > 0)
                            {
                                AddElementNodesHelper(element, vTree, ref removeSubPatches, 0, low, high);
                            }
                        }
                        else
                        {
                            DestroyHostLifecyclesInSubtree(vTree, element);
                        }
                        break;
                    default:
                        patch.SetTarget(element);
                        break;
                }

                i++;

                if (i < patches.Length)
                {
                    patch = patches[i];
                    index = patch.GetIndex();
                    if (index > high)
                    {
                        return i;
                    }
                }
                else
                {
                    return i;
                }
            }

            if (vTree is IParent parent)
            {
                var kids = parent.GetKids();
                var childOffset = element.childCount - kids.Length;
                if (childOffset < 0) childOffset = 0;
                for (var j = 0; j < kids.Length; j++)
                {
                    low++;
                    var kid = kids[j];
                    var nextLow = low + kid.GetDescendantsCount();
                    if (low <= index && index <= nextLow)
                    {
                        i = AddElementNodesHelper(element[childOffset + j], kid, ref patches, i, low, nextLow);

                        if (i < patches.Length)
                        {
                            patch = patches[i];
                            index = patch.GetIndex();
                            if (index > high)
                            {
                                return i;
                            }
                        }
                        else
                        {
                            return i;
                        }
                    }

                    low = nextLow;
                }
            }

            return i;
        }

        private static void DestroyRemovedLastHostLifecycles(
            VisualElement ve,
            IVTree vTree,
            RemoveLast<VisualElement> removeLast
        )
        {
            if (!(vTree is IParent parent)) return;

            var kids = parent.GetKids();
            var childOffset = ve.childCount - kids.Length;
            if (childOffset < 0) childOffset = 0;
            for (var i = removeLast.length; i < removeLast.length + removeLast.diff; i++)
            {
                DestroyHostLifecyclesInSubtree(kids[i], ve[childOffset + i]);
            }
        }

        private static void DestroyHostLifecyclesInSubtree(IVTree vTree, VisualElement ve)
        {
            if (vTree is IHostLifecycleTree<VisualElement> lifecycleTree)
            {
                var lifecycles = lifecycleTree.GetHostLifecycles();
                for (var i = lifecycles.Length - 1; i >= 0; i--)
                {
                    lifecycles[i].Destroy(ve);
                }
            }

            DestroyChildHostLifecycles(vTree, ve);
        }

        private static void DestroyChildHostLifecycles(IVTree vTree, VisualElement ve)
        {
            if (!(vTree is IParent parent)) return;

            var kids = parent.GetKids();
            var childOffset = ve.childCount - kids.Length;
            if (childOffset < 0) childOffset = 0;
            var childCount = kids.Length < (ve.childCount - childOffset) ? kids.Length : (ve.childCount - childOffset);
            for (var i = 0; i < childCount; i++)
            {
                DestroyHostLifecyclesInSubtree(kids[i], ve[childOffset + i]);
            }
        }

        private static VisualElement Helper(VisualElement rootElement, IPatch<VisualElement>[] patches)
        {
            foreach (var patch in patches)
            {
                var localElement = patch.GetTarget();
                var newElement = ApplyPatch(localElement, patch);
                if (localElement == rootElement)
                {
                    rootElement = newElement;
                }
            }

            return rootElement;
        }

        private static VisualElement ApplyPatch(VisualElement ve, IPatch<VisualElement> patch)
        {
            switch (patch)
            {
                case Redraw<VisualElement> redraw:
                    return ApplyPatchRedraw(ve, redraw.vTree);
                case Attrs<VisualElement> attrs:
                    return ApplyAttrs(ve, attrs.attrs);
                case RemoveLast<VisualElement> removeLast:
                {
                    var totalChildren = ve.childCount;
                    var initOffset = totalChildren - (removeLast.length + removeLast.diff);
                    if (initOffset < 0) initOffset = 0;
                    for (var i = 0; i < removeLast.diff; i++)
                    {
                        ve.RemoveAt(initOffset + removeLast.length);
                    }
                    return ve;
                }
                case Append<VisualElement> append:
                {
                    var kids = append.kids;
                    for (var i = append.length; i < kids.Length; i++)
                    {
                        ve.Add(Renderer.Render(kids[i]));
                    }
                    return ve;
                }
                case Remove<VisualElement> remove:
                {
                    if (remove.entry == null && remove.patches == null)
                    {
                        ve.RemoveFromHierarchy();
                        return null;
                    }

                    if (remove.entry.tag == Entry.Type.Move)
                    {
                        ve.RemoveFromHierarchy();
                    }

                    remove.entry.data = Helper(ve, remove.patches);
                    return ve;
                }
                case Reorder<VisualElement> reorder:
                    return ApplyPatchReorder(ve, reorder);
                case Attach<VisualElement> attach:
                    return ApplyPatchAttach(ve, attach);
            }
            return ve;
        }

        private static VisualElement ApplyPatchRedraw(VisualElement ve, IVTree vTree)
        {
            var parent = ve.parent;
            var index = parent?.IndexOf(ve) ?? -1;
            var newElement = Renderer.Render(vTree);

            if (newElement != ve)
            {
                if (parent != null)
                {
                    parent.Insert(index, newElement);
                    ve.RemoveFromHierarchy();
                }
            }

            return newElement;
        }

        private static VisualElement ApplyAttrs(VisualElement ve, Dictionary<string, IAttribute<VisualElement>> attrs)
        {
            foreach (var attr in attrs)
            {
                if (attr.Value != null)
                {
                    attr.Value.Apply(ve);
                }
            }

            return ve;
        }

        private static VisualElement ApplyPatchReorder(VisualElement ve, Reorder<VisualElement> patch)
        {
            var frag = ApplyPatchReorderEndInsertsHelper(patch.endInserts, patch);

            ve = Helper(ve, patch.patches);

            foreach (var insert in patch.inserts)
            {
                var entry = insert.entry;
                var node = entry.tag == Entry.Type.Move ? entry.data as VisualElement : Renderer.Render(entry.vTree);
                if (insert.index >= ve.childCount)
                {
                    ve.Add(node);
                }
                else
                {
                    ve.Insert(insert.index, node);
                }
            }

            if (frag != null)
            {
                foreach (var child in frag)
                {
                    ve.Add(child);
                }
            }

            return ve;
        }

        private static VisualElement[] ApplyPatchReorderEndInsertsHelper(Reorder<VisualElement>.Insert[] endInserts, Reorder<VisualElement> patch)
        {
            if (endInserts.Length == 0)
            {
                return null;
            }

            var frag = new VisualElement[endInserts.Length];
            for (var i = 0; i < endInserts.Length; i++)
            {
                var insert = endInserts[i];
                var entry = insert.entry;
                frag[i] = entry.tag == Entry.Type.Move ? entry.data as VisualElement : Renderer.Render(entry.vTree);
            }

            return frag;
        }

        private static VisualElement ApplyPatchAttach(VisualElement ve, Attach<VisualElement> attach)
        {
            var parent = ve.parent;
            var index = parent?.IndexOf(ve) ?? -1;

            if (!typeof(VisualElement).IsAssignableFrom(attach.newComponent))
            {
                throw new System.ArgumentException(attach.newComponent.FullName + " must inherit from VisualElement.");
            }

            var newElement = (VisualElement)System.Activator.CreateInstance(attach.newComponent);
            newElement.name = ve.name;

            if (parent != null)
            {
                parent.Insert(index, newElement);
                ve.RemoveFromHierarchy();
            }

            return newElement;
        }
    }
}
