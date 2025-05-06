using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType { Coin, Oxygen, Camera, Vacuum, Door }
    public ItemType itemType;
    private bool item = false;
    private int amount = 1;
    [SerializeField] private Sprite itemIcon;

    public void Collect()
    {
        Debug.Log("Item Collected: " + itemType);
        item = true;
        switch (itemType)
        {
            case ItemType.Camera:
                ItemManager.Instance.CollectCamera();
                InventoryUIManager.Instance.AddItemToInventory(itemIcon, "Camera");
                break;
            case ItemType.Vacuum:
                ItemManager.Instance.CollectVacuum();
                InventoryUIManager.Instance.AddItemToInventory(itemIcon, "Vacuum");
                break;
            case ItemType.Door:
                item = false;
                DetectDoorUI.Instance.ShowDetectDoor();
                break;
                // case ItemType.Coin:
                //     ItemManager.Instance.AddCoin(amount);
                //     break;
        }

        if (item)
            Destroy(gameObject);
    }
}
