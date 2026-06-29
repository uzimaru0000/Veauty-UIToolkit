using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Veauty.UIToolkit
{
    internal static class CallbackStore
    {
        private static readonly Dictionary<(VisualElement, string), object> store = new();

        internal static void Set<T>(VisualElement ve, string key, EventCallback<ChangeEvent<T>> newCallback)
        {
            var k = (ve, key);
            if (store.TryGetValue(k, out var old) && old is EventCallback<ChangeEvent<T>> oldCallback)
            {
                ve.UnregisterCallback(oldCallback);
            }
            ve.RegisterCallback(newCallback);
            store[k] = newCallback;
        }

        internal static void Remove(VisualElement ve, string key)
        {
            store.Remove((ve, key));
        }
    }
}
