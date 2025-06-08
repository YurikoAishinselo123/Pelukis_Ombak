using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChapterCompletedUI : MonoBehaviour
{
    public CanvasGroup chapterCompletedCanvas;
    public float fadeDuration = 1f;
    public static ChapterCompletedUI Instance;

    private void Awake()
    {
        // Start hidden
        if (chapterCompletedCanvas != null)
        {
            chapterCompletedCanvas.alpha = 0f;
            // gameObject.SetActive(false);
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        chapterCompletedCanvas.enabled = false;
    }

    public void Show()
    {
        chapterCompletedCanvas.enabled = true;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        chapterCompletedCanvas.alpha = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            chapterCompletedCanvas.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        chapterCompletedCanvas.alpha = 1f;

        // Optional: Play sound effect
        AudioManager.Instance?.PlaySFX("ChapterComplete");
    }
}
