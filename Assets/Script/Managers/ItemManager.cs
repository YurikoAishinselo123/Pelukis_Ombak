using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    private List<ItemType> collectedItems = new List<ItemType>();
    private int coinCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CollectItem(ItemType itemType)
    {
        if (itemType == ItemType.Coin)
        {
            AddCoin(1);
            return;
        }

        if (!collectedItems.Contains(itemType))
        {
            collectedItems.Add(itemType);
            AudioManager.Instance.SFXCollectItem();
            Debug.Log(itemType + " Collected!");
            if (itemType == ItemType.Camera || itemType == ItemType.Vacuum)
            {
                MissionManager.Instance?.OnItemCollected(itemType);
            }
        }
    }

    public bool HasItem(ItemType itemType)
    {
        return collectedItems.Contains(itemType);
    }

    public List<ItemType> GetCollectedItems()
    {
        return collectedItems;
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        Debug.Log("Coin Collected: " + coinCount);
    }

    public int GetCoinCount()
    {
        return coinCount;
    }
}
