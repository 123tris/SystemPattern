#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System_Pattern;
using OdinSerializer;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class SystemBehaviourEditorWindow : EditorWindow
{
    [MenuItem("Tools/Open System Behaviour Editor")]
    static void OpenWindow()
    {
        GetWindow<SystemBehaviourEditorWindow>().Show();
    }


    private List<SystemBehaviour> systemBehaviours = new List<SystemBehaviour>();

    private Vector2 scrollPosition;

    private void OnEnable()
    {
        if (!Directory.Exists($"{Application.dataPath}/Resources"))
        {
            Directory.CreateDirectory($"{Application.dataPath}/Resources");
        }

        systemBehaviours = SystemBehaviourFactory.CreateSystemBehaviourInstances();
    }


    private void OnGUI()
    {
        EditorGUILayout.BeginScrollView(scrollPosition);
        foreach (SystemBehaviour systemBehaviour in systemBehaviours)
        {
            EditorGUILayout.LabelField(systemBehaviour.GetType().Name, EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("Box");
            if (systemBehaviour.data.gameObjectReferences.Count > 0 &&
                systemBehaviour.persistence == SystemBehaviour.Persistence.Global)
            {
                EditorGUILayout.HelpBox("Are you sure you want a system with global persistence reference a object in a scene?", MessageType.Warning);
            }
            DrawSystemBehaviour(systemBehaviour);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();
    }

    private void SaveData(SystemBehaviour systemBehaviour)
    {
        var data = SerializationUtility.SerializeValue(systemBehaviour, DataFormat.JSON);
        File.WriteAllBytes(SystemBehaviourFactory.GetSystemFilePath(systemBehaviour.GetType()), data);
    }

    void DrawSystemBehaviour(SystemBehaviour systemBehaviour)
    {
        foreach (FieldInfo field in systemBehaviour.GetType().GetFields())
        {
            if (field.FieldType == typeof(SystemBehaviourMetaData)) continue;
            var changedValue = InspectField(field.GetValue(systemBehaviour), field.Name, field.FieldType, systemBehaviour);
            field.SetValue(systemBehaviour, changedValue);
            if (GUI.changed)
            {
                Debug.Log($"Save {systemBehaviour.GetType().Name} data");
                SaveData(systemBehaviour);
                GUI.changed = false;
            }
        }
    }

    /// <summary> Draw corresponding inspector GUI for the type of the object passed </summary>
    object InspectField(object value, string label, Type type, SystemBehaviour systemBehaviour)
    {
        if (type.IsPrimitive)
        {
            switch (value)
            {
                case int integer:
                    return EditorGUILayout.IntField(label, integer);
                case float floatingPoint:
                    return EditorGUILayout.FloatField(label, floatingPoint);
                case bool boolean:
                    return EditorGUILayout.Toggle(label, boolean);
                case string text:
                    return EditorGUILayout.TextField(label, text);
                case double doubleFloatingPoint:
                    return EditorGUILayout.DoubleField(label, doubleFloatingPoint);
                default:
                    throw new System.Exception("Unexpected primitive type");
            }
        }

        if (type.IsSubclassOf(typeof(Object)))
        {
            var obj = (Object)value;
            var selectedObject = EditorGUILayout.ObjectField(label, obj, type, obj);
            if (selectedObject != null)
            {
                GameObject gameObject = null;
                if (selectedObject is Component component)
                {
                    gameObject = component.gameObject;
                }
                else if (selectedObject is GameObject o)
                {
                    gameObject = o;
                }
                else
                {
                    throw new Exception("How do you even have a Object Type that isn't a component or gameobject");
                }

                GUID guid = gameObject.GetComponent<GUID>();
                if (guid == null)
                {
                    guid = gameObject.AddComponent<GUID>();
                    guid.guid = Guid.NewGuid();
                }

                systemBehaviour.data.gameObjectReferences[label] = guid.guid;
            }
            return selectedObject;
        }

        switch (value)
        {
            case Vector2 vec2:
                return EditorGUILayout.Vector2Field(label, vec2);
            case Vector3 vec3:
                return EditorGUILayout.Vector3Field(label, vec3);
            case Enum enumValue:
                return EditorGUILayout.EnumPopup(label, enumValue);
            default:
                Debug.Log($"Can't identify type: {type.Name} of variable name \"{label}\"");
                return null;
        }
    }
}
#endif