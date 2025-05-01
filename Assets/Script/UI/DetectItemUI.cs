using UnityEngine;
using TMPro;

public class DetectItemUI : MonoBehaviour
{
    public static DetectItemUI Instance;

    [SerializeField] private GameObject DetecItemCanvas;
    [SerializeField] private TextMeshProUGUI itemInfoText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        HideItemUI();
    }

    public void ShowItemUI(string itemName)
    {
        if (DetecItemCanvas != null && itemInfoText != null)
        {
            DetecItemCanvas.SetActive(true);
            itemInfoText.text = $"Press E to collect: {itemName}";
        }
    }

    public void HideItemUI()
    {
        if (DetecItemCanvas != null)
        {
            DetecItemCanvas.SetActive(false);
        }
    }
}
