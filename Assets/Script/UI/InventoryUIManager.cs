using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;
    public GameObject InventoryUICanvas;
    public InventorySlotUI slotPrefab;
    public Transform inventoryPanel;
    public int slotCount = 4;
    public Sprite defaultSprite;

    private List<InventorySlotUI> inventorySlots = new List<InventorySlotUI>();
    private InventorySlotUI selectedItem;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        GenerateInventorySlots();
        ClearInventoryUI();
    }

    private void GenerateInventorySlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            InventorySlotUI newSlot = Instantiate(slotPrefab, inventoryPanel);
            inventorySlots.Add(newSlot);
        }
    }

    public void AddItemToInventory(Sprite itemSprite, string itemName)
    {
        ResetSlotBackgroundColors();

        foreach (InventorySlotUI slot in inventorySlots)
        {
            if (slot.itemImage.sprite == defaultSprite)
            {
                slot.SetItem(itemSprite, itemName);
                slot.itemButton.gameObject.SetActive(true);
                return;
            }
        }
        Debug.Log("Inventory is full! Can't add more items.");
    }

    public void ClearInventoryUI()
    {
        foreach (InventorySlotUI slot in inventorySlots)
        {
            slot.ClearSlot(defaultSprite);
        }
    }

    private void Update()
    {
        int selectedIndex = InputManager.Instance.GetSelectedItemByKey();

        if (selectedIndex > 0 && selectedIndex <= inventorySlots.Count)
        {
            SelectItem(inventorySlots[selectedIndex - 1]);
        }
    }

    public void SelectItem(InventorySlotUI slot)
    {
        if (selectedItem == slot)
            return;

        ResetSlotBackgroundColors();
        selectedItem = slot;

        slot.SetBackgroundColor(new Color(0.5f, 0.5f, 0.5f, 1f));

        Debug.Log("Item Selected: " + slot.itemName.text);
    }

    private void ResetSlotBackgroundColors()
    {
        foreach (InventorySlotUI slot in inventorySlots)
        {
            slot.SetBackgroundColor(Color.white);
        }
    }

    public void ShowInventoryCanvas()
    {
        InventoryUICanvas.SetActive(true);
    }

    public void HideInventoryCanvas()
    {
        InventoryUICanvas.SetActive(false);
    }
}