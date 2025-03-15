using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private List<string> collectedItems = new List<string>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // public void AddItem(string itemName)
    // {
    //     collectedItems.Add(itemName);
    //     Debug.Log("Item Added: " + itemName);
    // }

    public List<string> GetCollectedItems()
    {
        return collectedItems;
    }
}
