using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Veauty.UIToolkit
{
    // Tracks, per element, the USS classes that Veauty added via ClassName/ClassList.
    // Replace removes only those tracked classes before adding the new set, so classes
    // added by UI Toolkit itself or by user code outside Veauty are never touched.
    internal static class ManagedClassList
    {
        private static readonly Dictionary<VisualElement, HashSet<string>> managed = new();

        internal static void Replace(VisualElement ve, string[] newClasses)
        {
            if (managed.TryGetValue(ve, out var old))
            {
                foreach (var cls in old)
                    ve.RemoveFromClassList(cls);
                old.Clear();
            }
            else
            {
                old = new HashSet<string>();
                managed[ve] = old;
            }

            foreach (var cls in newClasses)
            {
                if (string.IsNullOrEmpty(cls)) continue;
                ve.AddToClassList(cls);
                old.Add(cls);
            }
        }
    }
}
