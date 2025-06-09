using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChapterCompletedUI : MonoBehaviour
{
    public Canvas chapterCompletedCanvas;
    public CanvasGroup chapterCompletedContainer;
    public float fadeDuration = 1f;
    public static ChapterCompletedUI Instance;

    private void Awake()
    {
        // Start hidden
        if (chapterCompletedContainer != null)
        {
            chapterCompletedContainer.alpha = 0f;
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
    }

    void Start()
    {
        chapterCompletedCanvas.enabled = false;
    }

    // void Update()
    // {
    //     if (InputManager.Instance.TestingButton)
    //     {
    //         Show();
    //         Debug.Log("Testing");
    //     }
    // }

    public void Show()
    {
        chapterCompletedCanvas.enabled = true;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        AudioManager.Instance?.FadeOutBacksound();

        AudioManager.Instance?.FadeOutBacksound(() =>
        {
            AudioManager.Instance?.SFXChapterCompleted();
        });

        float elapsed = 0f;
        chapterCompletedContainer.alpha = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            chapterCompletedContainer.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        chapterCompletedContainer.alpha = 1f;
    }
}
