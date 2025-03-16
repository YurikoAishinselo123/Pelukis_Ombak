using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemName;
    public Button itemButton;
    public Image backgroundImage;

    private void Start()
    {
        itemButton.onClick.AddListener(OnItemClick);
    }

    private void OnItemClick()
    {
        InventoryUIManager.Instance.SelectItem(this);
    }

    public void SetItem(Sprite itemSprite, string name)
    {
        itemImage.sprite = itemSprite;
        itemName.text = name;
    }

    public void ClearSlot(Sprite defaultSprite)
    {
        itemImage.sprite = defaultSprite;
        itemName.text = "";
        itemButton.gameObject.SetActive(false);
    }

    public void SetBackgroundColor(Color color)
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = color;
        }
    }
}
