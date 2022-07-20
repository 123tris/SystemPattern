using UnityEngine;

public class ExampleSystem : SystemBehaviour
{
    public float exampleNumber = 10;

    public GameObject test;

    protected internal override void Start()
    {
        return;
        Debug.Log($"{test.name}");
        Debug.Log("Start called example number: "+exampleNumber);
    }

    protected internal override void OnDestroy()
    {
        //save system data
    }
}