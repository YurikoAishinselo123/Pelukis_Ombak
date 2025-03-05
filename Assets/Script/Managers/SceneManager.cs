using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        if (SceneIsValid(sceneName))
        {
            Debug.Log("Loading scene: " + sceneName);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene " + sceneName + " not found.");
        }
    }

    // Check if the scene is available in the build settings
    private bool SceneIsValid(string sceneName)
    {
        int sceneIndex = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + sceneName + ".unity");
        return sceneIndex >= 0;
    }
}
