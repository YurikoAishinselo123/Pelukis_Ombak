using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ChapterCompletedUI : MonoBehaviour
{
    public GameObject chapterCompletedCanvas;
    public CanvasGroup chapterCompletedContainer;
    public float fadeDuration = 1f;
    public TextMeshProUGUI pressAnyKeyText;
    public float twinkleSpeed = 1f;

    public static ChapterCompletedUI Instance;

    private bool canPressKey = false;

    private void Awake()
    {
        // Start hidden
        if (chapterCompletedContainer != null)
        {
            chapterCompletedContainer.alpha = 0f;
        }

        if (pressAnyKeyText != null)
        {
            pressAnyKeyText.gameObject.SetActive(false);
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
        chapterCompletedCanvas.SetActive(false);
    }

    void Update()
    {
        if (canPressKey)
        {
            // Twinkling alpha effect
            float alpha = Mathf.PingPong(Time.time * twinkleSpeed, 1f);
            Color color = pressAnyKeyText.color;
            color.a = alpha;
            pressAnyKeyText.color = color;

            // Detect any key press
            if (Input.anyKeyDown)
            {
                canPressKey = false;
                SaveSystemManager.Instance.ResetCollectedItems();
                SaveSystemManager.Instance.ResetMissionProgress();
                GameplayManager.Instance.NewGame();
                SceneLoader.Instance.LoadMainMenu();
            }
        }
    }

    public void Show()
    {
        Debug.Log("show UI chapter completed");
        chapterCompletedCanvas.SetActive(true);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
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

        // Wait 0.5s, then show the press-any-key text
        yield return new WaitForSeconds(0.5f);

        if (pressAnyKeyText != null)
        {
            pressAnyKeyText.gameObject.SetActive(true);
            canPressKey = true;
        }
    }
}
