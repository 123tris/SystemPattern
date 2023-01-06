using UnityEngine;
using UnityEngine.SceneManagement;

public class ExampleSystem : GlobalSystem
{
    public float exampleNumber = 10;

    public GameObject test;

    void Start()
    {
        return;
        Debug.Log($"{test.name}");
        Debug.Log("Start called example number: "+exampleNumber);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            SceneManager.LoadSceneAsync("New Scene",LoadSceneMode.Additive);
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("Scenes/SampleScene");
        if (Input.GetKeyDown(KeyCode.U))
            SceneManager.LoadScene("New Scene");
    }

    void OnDestroy()
    {
        //save system data
    }
}