using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemVisual
{
    public ItemType itemType;
    public Sprite sprite;
}

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [SerializeField] private List<ItemVisual> itemVisualList;

    private HashSet<ItemType> collectedItems = new HashSet<ItemType>();
    private Dictionary<ItemType, ItemVisual> itemVisualData = new Dictionary<ItemType, ItemVisual>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        BuildItemVisualDictionary();
    }

    private void Start()
    {
        LoadCollectedItemData();
    }

    private void BuildItemVisualDictionary()
    {
        foreach (ItemVisual visual in itemVisualList)
        {
            if (!itemVisualData.ContainsKey(visual.itemType))
            {
                itemVisualData.Add(visual.itemType, visual);
            }
        }
    }

    public void CollectItem(ItemType itemType)
    {
        if (!collectedItems.Contains(itemType))
        {
            collectedItems.Add(itemType);
            SaveCollectedItemData();
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
        return new List<ItemType>(collectedItems);
    }

    public Sprite GetItemSprite(ItemType itemType)
    {
        if (itemVisualData.TryGetValue(itemType, out ItemVisual visual))
        {
            return visual.sprite;
        }
        return null;
    }

    private void SaveCollectedItemData()
    {
        SaveSystemManager.Instance.SaveCollectedItemData(new List<ItemType>(collectedItems), 0); // assuming no coins
    }

    private void LoadCollectedItemData()
    {
        var data = SaveSystemManager.Instance.LoadCollectedItemData();
        collectedItems = new HashSet<ItemType>(data.collectedItems);
    }

    public void ResetCollectedItems()
    {
        collectedItems.Clear();
        SaveCollectedItemData();
    }
}
