#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            return;
        }
        if (Resources.Load("SystemManager") == null)
        {
            SystemBehaviourFactory.CreateSystemPrefab();
        }
        GameObject gameObject = (GameObject)Resources.Load("SystemManager");
        systemBehaviours = gameObject.GetComponents<SystemBehaviour>().ToList();
    }


    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        foreach (SystemBehaviour systemBehaviour in systemBehaviours)
        {
            EditorGUILayout.LabelField(systemBehaviour.GetType().Name, EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("Box");
            Editor editor = Editor.CreateEditor(systemBehaviour);
            editor.OnInspectorGUI();
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();
    }
}
#endif