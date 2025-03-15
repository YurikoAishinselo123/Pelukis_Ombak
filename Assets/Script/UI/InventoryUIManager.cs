using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // Import TextMeshPro

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    [System.Serializable]
    public class InventorySlot
    {
        public GameObject slotObject;
        public Image itemImage;
        public TMP_Text itemName;
    }

    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public Sprite defaultSprite;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        ClearInventoryUI();

    }

    public void AddItemToInventory(Sprite itemSprite, string itemName)
    {
        // Cari slot kosong pertama
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemImage.sprite == defaultSprite)
            {
                slot.itemImage.sprite = itemSprite;
                slot.itemName.text = itemName;
                slot.slotObject.SetActive(true);
                return;
            }
        }

        Debug.Log("Inventory penuh! Tidak bisa menambahkan item baru.");
    }

    public void ClearInventoryUI()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            slot.itemImage.sprite = defaultSprite;
            slot.itemName.text = "";
        }
    }
}
