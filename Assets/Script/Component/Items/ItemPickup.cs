using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public Sprite icon;
    private KeyCode interactKey = KeyCode.E;
    public KeyCode InteractionKey => interactKey;
    public Sprite InteractionIcon => icon;
    public string InteractionText => itemType.ToString();

    public ItemType itemType;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private int amount = 1;

    public void Collect()
    {
        Debug.Log("Item Collected: " + itemType);

        if (!ItemManager.Instance.HasItem(itemType))
        {
            ItemManager.Instance.CollectItem(itemType); // Save and track
            InventoryUIManager.Instance.AddItemToInventory(itemIcon, itemType.ToString()); // Show in UI
        }

        Destroy(gameObject); // Remove from scene
    }
}
