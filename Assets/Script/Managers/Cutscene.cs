using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using TMPro; // For TextMeshProUGUI

public class Cutscene : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoPlayer;
    public Canvas cutsceneCanvas;
    public TextMeshProUGUI pressAnyKeyText;

    private bool canStart = false;

    private void Start()
    {
        if (videoPlayer == null || cutsceneCanvas == null || pressAnyKeyText == null)
        {
            Debug.LogError("Missing references in Cutscene script.");
            return;
        }

        cutsceneCanvas.enabled = false;
        pressAnyKeyText.gameObject.SetActive(false);

        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.Play();
        StartCoroutine(PlayCutsceneSequence());
    }

    private void Update()
    {
        if (canStart && Input.anyKeyDown)
        {
            StartGame();
            canStart = false;
        }
    }

    private void StartGame()
    {
        StopAllCoroutines();
        pressAnyKeyText.gameObject.SetActive(false);
        AudioManager.Instance.PlayOfficeBacksound();
        SceneLoader.Instance.LoadOffice1();
        GameplayManager.Instance.onGameplay = true;
        CursorManager.Instance.HideCursor();
        SpawnCharacterManager.Instance.SpawnPositionOnStart(new Vector3(2.53f, 1.075f, 1.74f));
    }

    private IEnumerator PlayCutsceneSequence()
    {
        // Play background audio
        AudioManager.Instance.PlayMainThemeBacksound();

        // Optional delay
        yield return new WaitForSeconds(1f);

        // Start the video
        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        cutsceneCanvas.enabled = true;
        pressAnyKeyText.gameObject.SetActive(true);
        StartCoroutine(TwinkleText());

        canStart = true;

        PlayerPrefs.SetInt("HasNewGame", 1);
        PlayerPrefs.Save();
    }

    private IEnumerator TwinkleText()
    {
        float duration = 1f;
        float alpha;

        while (true)
        {
            // Fade out
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                alpha = Mathf.Lerp(1f, 0f, t / duration);
                SetTextAlpha(alpha);
                yield return null;
            }

            // Fade in
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                alpha = Mathf.Lerp(0f, 1f, t / duration);
                SetTextAlpha(alpha);
                yield return null;
            }
        }
    }

    private void SetTextAlpha(float alpha)
    {
        Color color = pressAnyKeyText.color;
        color.a = alpha;
        pressAnyKeyText.color = color;
    }
}
