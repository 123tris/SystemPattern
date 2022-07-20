using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System_Pattern;
using OdinSerializer;
using UnityEngine;
using Object = UnityEngine.Object;

public static class SystemBehaviourFactory
{
    public static string GetSystemFilePath(Type systemBehaviour)
    {
        return $"{Application.dataPath}/Resources/{systemBehaviour.FullName}.json";
    }

    internal static List<SystemBehaviour> CreateSystemBehaviourInstances()
    {
        List<SystemBehaviour> output = new List<SystemBehaviour>();

        List<TypeInfo> systemBehavioursInAssembly = GetSystemBehavioursInAssembly();
        //Create system behaviour instances
        foreach (TypeInfo systemBehaviour in systemBehavioursInAssembly)
        {
            string filepath = GetSystemFilePath(systemBehaviour);
            if (!File.Exists(filepath)) //if file doesnt exist no need to deserialize
            {
                var newInstance = (SystemBehaviour)Activator.CreateInstance(systemBehaviour);
                output.Add(newInstance);

                //Serialize new instance
                var bytes = SerializationUtility.SerializeValue(newInstance, DataFormat.JSON);
                File.WriteAllBytes(filepath, bytes);
                continue;
            }

            //Deserialize data into our new instance
            var systemData = File.ReadAllBytes(filepath);
            var deserializeMethod = typeof(SerializationUtility).GetMethod("DeserializeValue", new[] { typeof(byte[]), typeof(DataFormat), typeof(List<Object>), typeof(DeserializationContext) });
            deserializeMethod = deserializeMethod.MakeGenericMethod(systemBehaviour);

            var instance = (SystemBehaviour)deserializeMethod.Invoke(null, new object[] { systemData, DataFormat.JSON, null, null });

            //Load system meta data to create references to UnityEngine.Object instance
            SetUnityObjectReferences(instance, systemBehaviour);

            output.Add(instance);
        }
        return output;
    }

    public static void SetUnityObjectReferences(SystemBehaviour instance, Type systemBehaviourType)
    {
        if (instance.data == null)
        {
            instance.data = new SystemBehaviourMetaData();
            return;
        }
        foreach (KeyValuePair<string, Guid> keyValuePair in instance.data.gameObjectReferences)
        {
            var field = systemBehaviourType.GetField(keyValuePair.Key);
            field.SetValue(instance, GUIDManager.GetObject(keyValuePair.Value, field.FieldType));
        }
    }

    internal static List<TypeInfo> GetSystemBehavioursInAssembly()
    {
        //Get all types that derive from the abstract class System Behaviour
        List<TypeInfo> systemBehavioursInAssembly = new List<TypeInfo>();
        var definedTypes = Assembly.GetExecutingAssembly().DefinedTypes;
        foreach (TypeInfo definedType in definedTypes)
        {
            if (definedType.IsSubclassOf(typeof(SystemBehaviour)))
            {
                systemBehavioursInAssembly.Add(definedType);
            }
        }

        return systemBehavioursInAssembly;
    }
}
