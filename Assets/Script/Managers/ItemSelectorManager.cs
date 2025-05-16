using System.Collections.Generic;
using UnityEngine;

public class ItemSelectorManager : MonoBehaviour
{
    public GameObject VacuumObject;
    public GameObject CameraObject;
    private ItemType? currentlySelectedItem = null;
    public static ItemSelectorManager Instance { get; private set; }
    private int currentSelectedIndex = -1;
    private Dictionary<ItemType, GameObject> itemObjects = new();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
            // Toggle off
            if (itemObjects.TryGetValue(selectedItem, out GameObject selectedObject))
                selectedObject?.SetActive(false);

            currentSelectedIndex = -1;
            currentlySelectedItem = null;
        }
        else
        {
            foreach (var go in itemObjects.Values)
                go?.SetActive(false);

            if (itemObjects.TryGetValue(selectedItem, out GameObject selectedObject))
            {
                selectedObject?.SetActive(true);
                currentSelectedIndex = selectedIndex - 1;
                currentlySelectedItem = selectedItem;
            }
        }
    }

    public bool SelectedVacuum => currentlySelectedItem == ItemType.Vacuum;
    public bool SelectedCamera => currentlySelectedItem == ItemType.Camera;
}

