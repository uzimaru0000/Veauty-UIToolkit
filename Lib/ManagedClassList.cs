using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Veauty.UIToolkit
{
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
