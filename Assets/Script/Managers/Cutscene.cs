using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Cutscene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Canvas cutsceneCanvas;
    public Button startButton;

    void Start()
    {
        if (videoPlayer == null || cutsceneCanvas == null)
        {
            Debug.LogError("VideoPlayer or Canvas not assigned in Cutscene script.");
            return;
        }
        cutsceneCanvas.enabled = false;

        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.Play();
        StartCoroutine(PlayCutsceneSequence());
        startButton.onClick.AddListener(StartGame);

    }

    private void StartGame()
    {
        SceneLoader.Instance.LoadOffice1();
        GameplayManager.Instance.onGameplay = true;
        CursorManager.Instance.HideCursor();
        SpawnCharacterManager.Instance.SpawnPositionOnStart(new Vector3(2.53f, 1.075f, 1.74f));
    }

    private IEnumerator PlayCutsceneSequence()
    {
        // Play the background theme
        AudioManager.Instance.PlayMainThemeBacksound();

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Play the video
        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        cutsceneCanvas.enabled = true;
    }
}
