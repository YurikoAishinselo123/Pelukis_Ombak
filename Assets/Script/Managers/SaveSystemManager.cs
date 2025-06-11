using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CollectedItemSaveData
{
    public List<ItemType> collectedItems = new List<ItemType>();
    public int coinCount = 0;
}


public class SaveSystemManager : MonoBehaviour
{
    public static SaveSystemManager Instance { get; private set; }

    private string missionDirectory;
    private string missionFileName = "MissionProgress.json";

    private string collectedItemFileName = "CollectedItemData.json";
    private string collectedItemDirectory;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            missionDirectory = Path.Combine(Application.persistentDataPath, "Save System", "Mission");
            collectedItemDirectory = Path.Combine(Application.persistentDataPath, "Save System", "CollectedItems");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // -----------------------------
    // Mission Progress System
    // -----------------------------
    public void SaveMissionProgress(Dictionary<int, int> missionProgress)
    {
        string path = GetMissionProgressFilePath();

        if (!Directory.Exists(missionDirectory))
            Directory.CreateDirectory(missionDirectory);

        MissionProgressWrapper wrapper = new MissionProgressWrapper(missionProgress);
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(path, json);
        Debug.Log($"[SaveSystem] Mission progress saved to: {path}");
    }

    public Dictionary<int, int> LoadMissionProgress()
    {
        string path = GetMissionProgressFilePath();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            MissionProgressWrapper wrapper = JsonUtility.FromJson<MissionProgressWrapper>(json);
            return wrapper.ToDictionary();
        }

        Debug.Log("[SaveSystem] No saved mission progress found. Starting fresh.");
        return new Dictionary<int, int>();
    }

    public void ResetMissionProgress()
    {
        Dictionary<int, int> currentProgress = LoadMissionProgress();
        List<int> keys = new List<int>(currentProgress.Keys);
        foreach (int key in keys)
        {
            currentProgress[key] = 0;
        }

        SaveMissionProgress(currentProgress);
        Debug.Log("[SaveSystem] Mission progress has been reset to zero.");
    }

    private string GetMissionProgressFilePath()
    {
        return Path.Combine(missionDirectory, missionFileName);
    }

    // -----------------------------
    // Collected Item Save System
    // -----------------------------
    public void SaveCollectedItemData(List<ItemType> collectedItems, int coinCount)
    {
        string path = GetCollectedItemFilePath();

        if (!Directory.Exists(collectedItemDirectory))
            Directory.CreateDirectory(collectedItemDirectory);

        CollectedItemSaveData data = new CollectedItemSaveData
        {
            collectedItems = collectedItems,
            coinCount = coinCount
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log($"[SaveSystem] Collected item data saved to: {path}");
    }

    public CollectedItemSaveData LoadCollectedItemData()
    {
        string path = GetCollectedItemFilePath();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<CollectedItemSaveData>(json);
        }

        Debug.Log("[SaveSystem] No saved collected items found. Starting fresh.");
        return new CollectedItemSaveData();
    }

    private string GetCollectedItemFilePath()
    {
        return Path.Combine(collectedItemDirectory, collectedItemFileName);
    }

    public void ResetCollectedItems()
    {
        // Clear the saved file by overwriting it with an empty state
        CollectedItemSaveData emptyData = new CollectedItemSaveData
        {
            collectedItems = new List<ItemType>(),
            coinCount = 0 // optional, depending on whether you still use coins
        };

        string path = GetCollectedItemFilePath();

        if (!Directory.Exists(collectedItemDirectory))
            Directory.CreateDirectory(collectedItemDirectory);

        string json = JsonUtility.ToJson(emptyData, true);
        File.WriteAllText(path, json);

        Debug.Log("[SaveSystem] Collected items have been reset.");
    }
}

