using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public static class SystemBehaviourFactory
{
    public static readonly string systemFilePath = $"{Application.dataPath}/Resources/SystemManager.prefab";
    internal static List<TypeInfo> GetSystemBehavioursInAssembly()
    {
        //Get all types that derive from the abstract class System Behaviour
        List<TypeInfo> systemBehavioursInAssembly = new List<TypeInfo>();
        var definedTypes = Assembly.GetExecutingAssembly().DefinedTypes;
        foreach (TypeInfo definedType in definedTypes)
        {
            if (definedType.IsSubclassOf(typeof(SystemBehaviour)) && !definedType.IsAbstract)
            {
                systemBehavioursInAssembly.Add(definedType);
            }
        }

        return systemBehavioursInAssembly;
    }

#if UNITY_EDITOR
    public static void CreateSystemPrefab()
    {
        GameObject temp = new GameObject();
        foreach (TypeInfo typeInfo in GetSystemBehavioursInAssembly())
        {
            temp.AddComponent(typeInfo);
        }
        PrefabUtility.SaveAsPrefabAsset(temp, systemFilePath);
        Object.DestroyImmediate(temp);
    }
#endif
}
