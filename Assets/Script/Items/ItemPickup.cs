using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType { Coin, Oxygen, Camera, Vacuum }
    public ItemType itemType;
    public int amount = 1;

    public void Collect()
    {
        // Handle item collection logic, like adding it to inventory
        Debug.Log("Item Collected: " + itemType);

        switch (itemType)
        {
            case ItemType.Camera:
                ItemManager.Instance.CollectCamera();
                break;
            case ItemType.Vacuum:
                ItemManager.Instance.CollectVacuum();
                break;
                // case ItemType.Coin:
                //     ItemManager.Instance.AddCoin(amount);
                //     break;
        }

        Destroy(gameObject);
    }
}
