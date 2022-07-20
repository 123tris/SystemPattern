using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    private static List<SystemBehaviour> systemBehaviours = new List<SystemBehaviour>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadSystemManager()
    {
        GameObject systemBehaviourManager = new GameObject("System Behaviour Manager");
        //Set gameobject to inactive to avoid invoking Awake/OnEnable on AddComponent calls
        systemBehaviourManager.SetActive(false);
        systemBehaviourManager.AddComponent<SystemManager>();

        systemBehaviours = SystemBehaviourFactory.CreateSystemBehaviourInstances();

        DontDestroyOnLoad(systemBehaviourManager);
        systemBehaviourManager.SetActive(true);
    }

    void Start()
    {
        systemBehaviours.ForEach(s => s.Start());
    }

    void Update()
    {
        systemBehaviours.ForEach(s => s.Update());
    }

    void OnDrawGizmos()
    {
        systemBehaviours.ForEach(s => s.OnDrawGizmos());
    }

    //Should only be called by the SystemBehaviourFactory
    internal void Add(SystemBehaviour system)
    {
        systemBehaviours.Add(system);
    }

    public static T Get<T>() where T : SystemBehaviour
    {
        foreach (SystemBehaviour system in systemBehaviours)
        {
            if (system is T behaviour)
                return behaviour;
        }

        Debug.LogError($"Cannot find system of type: {typeof(T).FullName}");
        return null;
    }
}
