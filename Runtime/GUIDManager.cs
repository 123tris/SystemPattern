using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace System_Pattern
{
    public static class GUIDManager
    {
        private static readonly Dictionary<Guid,GUID> guidObjects = new Dictionary<Guid, GUID>(); //updated by the editor changing the referenced gameobject
        private static bool init = false;


        public static Object GetObject(Guid guid,Type objectType)
        {
            if (!init)
                Init();
            GUID instance = guidObjects.FirstOrDefault(o => o.Key == guid).Value;
            if (instance == null)
                return null;
            if (objectType == typeof(GameObject))
                return instance.gameObject;
            if (objectType == typeof(Component) && objectType != typeof(GUID))
                return instance.GetComponent(objectType);
            throw new Exception("Cannot find type of object");
        }

        private static void Init()
        {
            var guidsObjects = Object.FindObjectsOfType<GUID>();
            foreach (var guidObject in guidsObjects)
            {
                guidObjects[guidObject.guid] = guidObject;
            }
        }

        internal static void AddObject(Guid guid, GUID instance)
        {
            guidObjects[guid] = instance;
        }

        internal static void RemoveObject(Guid guid)
        {
            guidObjects.Remove(guid);
        }
    }
}