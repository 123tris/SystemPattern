using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace System_Pattern
{
    [Serializable]
    public class SystemBehaviourMetaData
    {
        //Key = name of the system field, Value = gameobject GUID
        public Dictionary<string,Guid> gameObjectReferences = new Dictionary<string, Guid>();

        public int index;

        public List<Object> GetUnityObjects(Type systemBehaviourType)
        {
            List<Object> output = new List<Object>(gameObjectReferences.Count);
            foreach (KeyValuePair<string, Guid> keyValuePair in gameObjectReferences)
            {
                var field = systemBehaviourType.GetField(keyValuePair.Key);
                var @object = GUIDManager.GetObject(keyValuePair.Value, field.FieldType);
                output.Add(@object);
            }

            return output;
        }
    }
}