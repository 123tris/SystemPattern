using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
using static Utility.CooldownManager;
using Object = UnityEngine.Object;

public class SystemManager
{
    private static List<SystemBehaviour> globalSystems = new List<SystemBehaviour>();
    private static List<SceneSystem> sceneSystems = new List<SceneSystem>();

    private static GameObject Prefab
    {
        get
        {
            if (_prefab == null)
            {
                _prefab = Resources.Load<GameObject>("SystemManager");
            }

            return _prefab;
        }
    }
    private static GameObject _prefab;

    private static GameObject systemBehaviourManager;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadSystemManager()
    {
        globalSystems = new List<SystemBehaviour>();
        sceneSystems = new List<SceneSystem>();
        systemBehaviourManager = Resources.Load<GameObject>("SystemManager");
        if (systemBehaviourManager == null)
            systemBehaviourManager = new GameObject("System Behaviour Manager");
        else
        {
            systemBehaviourManager.SetActive(false);
            systemBehaviourManager = Object.Instantiate(systemBehaviourManager);
        }

        //Set gameobject to inactive to avoid invoking Awake/OnEnable on AddComponent calls
        systemBehaviourManager.SetActive(false);

        foreach (TypeInfo systemType in SystemBehaviourFactory.GetSystemBehavioursInAssembly())
        {
            if (typeof(SceneSystem).IsAssignableFrom(systemType))
                continue;
            SystemBehaviour instance = (SystemBehaviour)systemBehaviourManager.GetAddComponent(systemType);
            globalSystems.Add(instance);
        }

        Object.DontDestroyOnLoad(systemBehaviourManager);
        systemBehaviourManager.SetActive(true);

        SceneManager.sceneLoaded -= SceneLoaded; //In case domain reload is disabled substract the function from the delegate
        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.sceneUnloaded -= SceneUnloaded;
        SceneManager.sceneUnloaded += SceneUnloaded;
    }

    static SceneSystem GetSceneSystem(Scene scene)
    {
        SceneSystem[] systems = Prefab.GetComponents<SceneSystem>();
        return systems.FirstOrDefault(s => s.sceneToLoad == scene.name);
    }

    private static void SceneUnloaded(Scene scene)
    {
        var system = GetSceneSystem(scene);
        if (system != null)
        {
            SceneSystem instance = (SceneSystem) systemBehaviourManager.GetComponent(system.GetType());
            sceneSystems.Remove(instance);
            Object.Destroy(instance);
        }
    }

    private static void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        var system = GetSceneSystem(scene);
        if (system != null)
        {
            OnNextFrame(() =>
            {
                var instance = (SceneSystem) systemBehaviourManager.GetAddComponent(system.GetType());
                if (sceneSystems.Contains(instance))
                {

                }
            });
            
        }
    }

    ///<summary> Retrieves the system behaviour of the specified type. SceneSystem type arguments will return null if their corresponding scene isn't loaded </summary>
    public static T Get<T>() where T : SystemBehaviour
    {
        T system = sceneSystems.OfType<T>().FirstOrDefault();
        if (system != null)
            return system;
        return globalSystems.OfType<T>().FirstOrDefault();
        //if (behaviour is SceneSystem sceneSystem)
        //{
        //    var scenes = SceneManagerExtensions.GetActiveScenes();
        //    if (scenes.Any(scene => scene.name == sceneSystem.sceneToLoad))
        //        return behaviour;

        //    Debug.LogWarning("Attempted to retrieve scene system whilst its corresponding scene has not been loaded yet");
        //    return null;
        //}

        //if (behaviour == null) //should never happen unless function is called in editor mode
        //    Debug.LogError($"Cannot find system behaviour of type: {typeof(T).FullName}");
        //return behaviour;
    }
}
