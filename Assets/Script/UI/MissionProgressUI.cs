using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionProgressUI : MonoBehaviour
{
    public TextMeshProUGUI mergedText;
    public CanvasGroup canvasGroup;

    public float fadeDuration = 0.5f;
    public float displayDuration = 2f;

    public void Show(string title, int current, int total)
    {
        mergedText.text = $"{title}   {current}/{total}"; // 3 non-breaking spaces
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        yield return Fade(0f, 1f, fadeDuration); // Fade in
        yield return new WaitForSeconds(displayDuration);
        yield return Fade(1f, 0f, fadeDuration); // Fade out
        Destroy(gameObject);
    }

    private IEnumerator Fade(float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = end;
    }
}
