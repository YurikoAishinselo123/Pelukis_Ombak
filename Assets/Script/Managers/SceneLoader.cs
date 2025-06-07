using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void LoadSceneWithVideo(string targetSceneName, Vector3 spawnPosition)
    {
        TransitionData.NextSceneName = targetSceneName;
        TransitionData.TargetSpawnPosition = spawnPosition;

        Debug.Log("scenename : " + targetSceneName);
        Debug.Log("position : " + spawnPosition);
        SceneManager.LoadScene("VideoTransition");
    }

    public void LoadMainMenuWithVideo() => LoadSceneWithVideo("MainMenu", Vector3.zero);
    public void LoadOffice1WithVideo() => LoadSceneWithVideo("Office1", new Vector3(0, 0, 0)); // default, override in DoorManager
    public void LoadOffice2WithVideo() => LoadSceneWithVideo("Office2", new Vector3(0, 0, 0));
    public void LoadOffice3WithVideo() => LoadSceneWithVideo("Office3", new Vector3(0, 0, 0));
    public void LoadOceanWithVideo() => LoadSceneWithVideo("Ocean", new Vector3(0, 0, 0));


    public void LoadMainMenu() => LoadScene("MainMenu");
    public void LoadOffice1() => LoadScene("Office1");
    public void LoadOffice2() => LoadScene("Office2");
    public void LoadOffice3() => LoadScene("Office3");
    public void LoadOcean() => LoadScene("Ocean");


    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
