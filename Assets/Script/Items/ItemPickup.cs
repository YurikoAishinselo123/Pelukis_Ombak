using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType { Coin, Oxygen, Camera, Vacuum }
    public ItemType itemType;
    public int amount = 1;
    public Sprite itemIcon;

    public void Collect()
    {
        Debug.Log("Item Collected: " + itemType);

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
                // case ItemType.Coin:
                //     ItemManager.Instance.AddCoin(amount);
                //     break;
        }

        Destroy(gameObject);
    }
}
