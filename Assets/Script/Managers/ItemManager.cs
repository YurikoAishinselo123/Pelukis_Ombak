using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    private HashSet<ItemType> collectedItems = new HashSet<ItemType>();
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
            Debug.Log(itemType + " Collected!");
        }
    }

    public bool HasItem(ItemType itemType)
    {
        return collectedItems.Contains(itemType);
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
