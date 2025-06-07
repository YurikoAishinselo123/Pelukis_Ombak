using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class Cutscene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Canvas cutsceneCanvas;

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
