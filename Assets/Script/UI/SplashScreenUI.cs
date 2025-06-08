using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1.5f;
    public float waitAfterFadeIn = 1f;

    private void Start()
    {
        StartCoroutine(PlaySplashSequence());
    }

    private System.Collections.IEnumerator PlaySplashSequence()
    {
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        // Wait
        yield return new WaitForSeconds(waitAfterFadeIn);

        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        SceneLoader.Instance.LoadMainMenu();
    }

    private System.Collections.IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
