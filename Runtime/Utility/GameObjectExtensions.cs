using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utility
{
    public static class GameObjectExtensions
    {
        ///<summary>Adds a component if it is missing</summary>
        public static Component GetAddComponent(this GameObject gameObject, Type type)
        {
            var component = gameObject.GetComponent(type);
            if (component == null)
                return gameObject.AddComponent(type);
            return component;
        }

        public static void RemoveComponents(this GameObject gameObject, Type type)
        {
            var components = gameObject.GetComponents(type);
            if (components == null)
                return;
            foreach (Component component in components)
            {
                Object.Destroy(component);
            }
        }
    }
}