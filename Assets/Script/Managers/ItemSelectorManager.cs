using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    public GameObject VacuumObject;
    public GameObject CameraObject;

    private int currentSelectedIndex = -1;
    private Dictionary<ItemType, GameObject> itemObjects = new();

    private void Start()
    {
        itemObjects[ItemType.Vacuum] = VacuumObject;
        itemObjects[ItemType.Camera] = CameraObject;
    }

    private void Update()
    {
        int selectedIndex = InputManager.Instance.GetSelectedItemByKey();

        if (selectedIndex == -1) return;

        List<ItemType> collected = ItemManager.Instance.GetCollectedItems();

        if (selectedIndex <= 0 || selectedIndex > collected.Count) return;

        ItemType selectedItem = collected[selectedIndex - 1];

        if (selectedIndex - 1 == currentSelectedIndex)
        {
            // Toggle off the currently selected item
            if (itemObjects.TryGetValue(selectedItem, out GameObject selectedObject))
            {
                selectedObject?.SetActive(false);
            }
            currentSelectedIndex = -1;
        }
        else
        {
            // Turn off all item objects
            foreach (var go in itemObjects.Values)
            {
                go?.SetActive(false);
            }

            // Activate the newly selected item
            if (itemObjects.TryGetValue(selectedItem, out GameObject selectedObject))
            {
                selectedObject?.SetActive(true);
                currentSelectedIndex = selectedIndex - 1;
            }
        }
    }
}
