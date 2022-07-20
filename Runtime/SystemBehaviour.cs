using System;
using System_Pattern;

/// <summary>
/// A system behaviour is a class that can be statically accessed through SystemManager.Get<> and only one instance of this script is allowed to exist.
/// System behaviours are automatically generated by the SystemBehaviourManager class and inspectors can be edited using the system editor window
/// Upon generation this gameobject are set to DoNotDestroy (and as such will be in a separate scene)
/// </summary>
[Serializable]
public class SystemBehaviour
{
    public enum Persistence
    {
        Global,
        Scene
    }

    public Persistence persistence;

    public SystemBehaviourMetaData data;

    protected internal virtual void Start() { }

    protected internal virtual void OnDrawGizmos() { }

    protected internal virtual void OnDestroy() { }

    protected internal virtual void Update() { }
}