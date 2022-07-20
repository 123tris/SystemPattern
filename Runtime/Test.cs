using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private SomeComponent component;

    void Start()
    {
        Destroy(component);
        //component = GetComponent<SomeComponent>();
        //Destroy(component);
        //component.Do(); //prints "Hi"
        //TrueDestroy(ref component);
        //component.Do(); //throws error
    }

    void Update()
    {
        component.Do();
    }

    private void TrueDestroy<T>(ref T o) where T : UnityEngine.Object
    {
        Destroy(o);
        o = null;
    }
}
