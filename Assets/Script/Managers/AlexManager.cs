using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI; // Required for Button

public class AlexManager : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoPlayer;
    public Canvas cutsceneCanvas;
    public TextMeshProUGUI pressAnyKeyText;
    public Button skipButton;

    private bool canStart = false;
    private bool cutsceneSkipped = false;

    private void Start()
    {
        if (videoPlayer == null || cutsceneCanvas == null || pressAnyKeyText == null || skipButton == null)
        {
            Debug.LogError("Missing references in Cutscene script.");
            return;
        }

        cutsceneCanvas.enabled = false;
        pressAnyKeyText.gameObject.SetActive(false);

        skipButton.gameObject.SetActive(true); // Show the skip button
        skipButton.onClick.AddListener(SkipCutscene);

        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.Play();
        StartCoroutine(PlayCutsceneSequence());
    }

    private void Update()
    {
        if (!cutsceneSkipped && Input.anyKeyDown)
        {
            if (canStart)
            {
                StartGame();
            }
            else
            {
                SkipCutscene();
            }
        }
    }

    private void StartGame()
    {
        cutsceneSkipped = true;
        StopAllCoroutines();
        pressAnyKeyText.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        videoPlayer.Stop();

        AudioManager.Instance.PlayOfficeBacksound();
        SceneLoader.Instance.LoadOffice1();
        GameplayManager.Instance.onGameplay = true;
        CursorManager.Instance.HideCursor();
        SpawnCharacterManager.Instance.SpawnPositionOnStart(new Vector3(2.53f, 1.075f, 1.74f));
    }

    public void SkipCutscene()
    {
        if (cutsceneSkipped) return;

        videoPlayer.Stop();
        OnVideoFinished(videoPlayer); // Reuse finish logic
    }

    private IEnumerator PlayCutsceneSequence()
    {
        AudioManager.Instance.PlayMainThemeBacksound();
        yield return new WaitForSeconds(1f);
        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        if (cutsceneSkipped) return;

        cutsceneCanvas.enabled = true;
        pressAnyKeyText.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(false); // Hide skip button after video
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
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                alpha = Mathf.Lerp(1f, 0f, t / duration);
                SetTextAlpha(alpha);
                yield return null;
            }

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
