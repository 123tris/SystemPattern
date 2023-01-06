using System;
using System_Pattern;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

// ReSharper disable InconsistentNaming

public class GUID : MonoBehaviour
{
    [SerializeField]
    public SerializableGuid guid;

    void OnEnable()
    {
        GUIDManager.AddObject(guid, this);
    }

    private void OnDisable()
    {
        GUIDManager.RemoveObject(guid);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GUID))]
public class GUIDEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUID guid = (GUID) target;
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField("GUID",guid.guid.ToString());
        EditorGUI.EndDisabledGroup();
    }
}
#endif
