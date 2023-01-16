using System;
using UnityEngine;
using Utility;

/// <summary>
/// A system behaviour is a class that can be statically accessed through SystemManager.Get and only one instance of this script is allowed to exist.
/// System behaviours are automatically generated by the SystemBehaviourManager class and inspectors can be edited using the system editor window
/// Upon generation this gameobject are set to DoNotDestroy (and as such will be in a separate scene)
/// </summary>
[Serializable]
public class SystemBehaviour : MonoBehaviour
{
    internal SystemBehaviour() { }

    public enum Persistence { Global, Scene }

    [SerializeField, ReadOnly]
    private protected Persistence persistence;
}

public abstract class GlobalSystem : SystemBehaviour
{
    protected GlobalSystem()
    {
        persistence = Persistence.Global;
    }
}

//Make a gameobject called "SceneSystems" in every scene whilst editing. Similar to Bolt's variables gameobject.
public abstract class SceneSystem : SystemBehaviour
{
    protected SceneSystem()
    {
        persistence = Persistence.Scene;
    }

    [SerializeField, SerializeScene]
    internal string sceneToLoad;
}