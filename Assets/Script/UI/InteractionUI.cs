using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI shortcutKeyText;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI interactionLabel;

    public void Setup(KeyCode key, Sprite icon, string label)
    {
        shortcutKeyText.text = key.ToString();
        iconImage.sprite = icon;
        interactionLabel.text = label;
    }
}
