using UnityEngine;


public class ItemPickup : MonoBehaviour
{
    public ItemType itemType;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private int amount = 1;

    public void Collect()
    {
        Debug.Log("Item Collected: " + itemType);

        switch (itemType)
        {
            case ItemType.Coin:
                ItemManager.Instance.AddCoin(amount);
                break;
            case ItemType.Camera:
            case ItemType.Vacuum:
            case ItemType.Oxygen:
                ItemManager.Instance.CollectItem(itemType);
                InventoryUIManager.Instance.AddItemToInventory(itemIcon, itemType.ToString());
                break;

            case ItemType.Door:
                DetectDoorUI.Instance.ShowDetectDoor();
                return;
        }

        Destroy(gameObject);
    }
}
