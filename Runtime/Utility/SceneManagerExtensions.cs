using UnityEngine.SceneManagement;

internal class SceneManagerExtensions : SceneManager
{
    public static Scene[] GetActiveScenes()
    {
        int countLoaded = sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];

        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = GetSceneAt(i);
        }

        return loadedScenes;
    }

    public static void LoadScenes(Scene[] scenes)
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            LoadScene(scenes[i].name, i == 0 ? LoadSceneMode.Single : LoadSceneMode.Additive);
        }
    }

    /// <summary> Adds scene to the runtime if it hasn't been loaded in yet</summary>
    public static void AddSceneIfNotLoaded(string sceneName)
    {
        Scene playerScene = GetSceneByName(sceneName);
        if (!playerScene.IsValid())
        {
            LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }
}
