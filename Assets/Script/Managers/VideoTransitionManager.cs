using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class VideoTransitionManager : MonoBehaviour
{
    public static string NextSceneName;
    public static Vector3? TargetSpawnPosition = null;
    public VideoPlayer videoPlayer;
    private float transitionTime = 1f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(PlayVideoAndLoadScene());
    }

    private IEnumerator PlayVideoAndLoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(NextSceneName);
        asyncLoad.allowSceneActivation = false;

        videoPlayer.Play();
        yield return new WaitForSeconds(transitionTime);

        asyncLoad.allowSceneActivation = true;

        yield return null;
        Debug.Log("spwn position : " + TargetSpawnPosition);

        if (TargetSpawnPosition.HasValue)
        {
            Debug.Log("spwn position : " + TargetSpawnPosition.Value);
            SpawnCharacterManager.Instance.SpawnPositionOnStart(TargetSpawnPosition.Value);
        }
    }
}
