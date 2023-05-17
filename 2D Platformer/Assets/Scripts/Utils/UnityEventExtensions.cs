using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public static class UnityEventExtensions
    {
        private const BindingFlags BindingAttrs = BindingFlags.Instance | BindingFlags.NonPublic;

        // get target gameObjects from UnityEvent using reflection
        // Reflection - the pattern when we get fields/methods from class by its string name
        public static GameObject[] ToGameObjects(this UnityEvent unityEvent)
        {
            var gameObjects = new List<GameObject>();

            if (unityEvent == null) return gameObjects.ToArray();

            // get and check PersistentCalls from UnityEventBase
            var eventType = typeof(UnityEventBase);
            var persistentCallsField = eventType.GetField("m_PersistentCalls", BindingAttrs);
            if (persistentCallsField == null) return gameObjects.ToArray();
            var persistentCallsValue = persistentCallsField.GetValue(unityEvent);

            // get and check List<PersistentCall> from PersistentCalls
            var callGroupType = persistentCallsValue.GetType();
            var callGroupField = callGroupType.GetField("m_Calls", BindingAttrs);
            if (callGroupField == null) return gameObjects.ToArray();
            var callGroupValue = (IEnumerable) callGroupField.GetValue(persistentCallsValue);

            // get and check List<PersistentCall>
            var listType = callGroupField.GetValue(persistentCallsValue).GetType();
            if (!listType.IsGenericType || listType.GetGenericTypeDefinition() != typeof(List<>)) return gameObjects.ToArray();
            var itemType = listType.GetGenericArguments().Single();

            foreach (var pc in callGroupValue)
            {
                // get and check UnityEngine.Object
                var itemField = itemType.GetField("m_Target", BindingAttrs);
                if (itemField == null) continue;

                // get and check UnityEngine.GameObject from Object
                var itemValue = (Object) itemField.GetValue(pc);
                var propertyInfo = itemValue.GetType().GetProperty("gameObject");
                if (propertyInfo == null) continue;

                var gameObject = (GameObject) propertyInfo.GetValue(itemValue);
                gameObjects.Add(gameObject);
            }

            return gameObjects.ToArray();
        }
    }
}