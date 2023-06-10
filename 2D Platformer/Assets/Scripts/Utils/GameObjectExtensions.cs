using UnityEngine;

namespace Utils
{
    public static class GameObjectExtensions
    {
        public static T GetInterfaceExtension<T>(this GameObject gameObject)
        {
            var components = gameObject.GetComponents<Component>();

            foreach (var component in components)
            {
                if (component is T type)
                {
                    return type;
                }
            }

            return default;
        }
    }
}