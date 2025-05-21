using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;

[System.Serializable]
public class MissionData
{
    public int chapterId;
    public string chapterTitle;
    public string chapterDescription;
    public List<Mission> missions;
}

[System.Serializable]
public class Mission
{
    public int id;
    public string title;
    public string description;
    public int qty;
    public string condition; // e.g., "1" means this mission only starts after mission 1 is completed
}

[System.Serializable]
public class SerializableDictionary
{
    public List<string> keys = new List<string>();
    public List<int> values = new List<int>();
}


public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }
    public MissionData missionData;

    private Dictionary<int, int> missionProgress = new Dictionary<int, int>();
    private HashSet<int> completedMissions = new HashSet<int>();

    private string progressSavePath;
    private HashSet<ItemType> collectedTools = new HashSet<ItemType>(); // Track collected tools for uniqueness

    private void Awake()
    {
        //if (Instance != null && Instance != this)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //Instance = this;
        //DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        progressSavePath = Path.Combine(Application.persistentDataPath, "MissionProgress.json");

        LoadMissionData();
        LoadMissionProgress();
    }

    private void LoadMissionData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Mission/Chapter1.json");

        if (File.Exists(path))
        {
            string jsonText = File.ReadAllText(path);
            missionData = JsonUtility.FromJson<MissionData>(jsonText);
            Debug.Log("Mission data loaded successfully!");

            foreach (var mission in missionData.missions)
            {
                if (!missionProgress.ContainsKey(mission.id))
                    missionProgress[mission.id] = 0;
            }
        }
        else
        {
            Debug.LogError($"Mission JSON not found at path: {path}");
        }
    }

    private void LoadMissionProgress()
    {
        if (File.Exists(progressSavePath))
        {
            string json = File.ReadAllText(progressSavePath);
            SerializableDictionary loadedProgress = JsonUtility.FromJson<SerializableDictionary>(json);

            for (int i = 0; i < loadedProgress.keys.Count && i < loadedProgress.values.Count; i++)
            {
                if (int.TryParse(loadedProgress.keys[i], out int missionId))
                {
                    missionProgress[missionId] = loadedProgress.values[i];

                    Mission mission = GetMissionById(missionId);
                    if (mission != null && missionProgress[missionId] >= mission.qty)
                    {
                        completedMissions.Add(missionId);
                    }
                }
            }

            Debug.Log("Mission progress loaded.");
        }
        else
        {
            Debug.Log("No saved mission progress found, starting fresh.");
        }
    }

    private void SaveMissionProgress()
    {
        SerializableDictionary serializableDict = new SerializableDictionary();
        foreach (var kvp in missionProgress)
        {
            serializableDict.keys.Add(kvp.Key.ToString());
            serializableDict.values.Add(kvp.Value);
        }

        string json = JsonUtility.ToJson(serializableDict, true);
        File.WriteAllText(progressSavePath, json);
        Debug.Log("Mission progress saved.");
    }

    public void UpdateMissionProgress(int missionId, int amount = 1)
    {
        if (!missionProgress.ContainsKey(missionId))
        {
            Debug.LogWarning($"Mission ID {missionId} not found.");
            return;
        }

        Mission mission = GetMissionById(missionId);
        if (mission == null)
        {
            Debug.LogWarning($"Mission data for ID {missionId} not found.");
            return;
        }

        if (!IsMissionAvailable(mission))
        {
            Debug.Log($"Mission '{mission.title}' not yet available. Prerequisite not completed.");
            return;
        }

        missionProgress[missionId] = Mathf.Min(missionProgress[missionId] + amount, mission.qty);
        Debug.Log($"Mission '{mission.title}' progress: {missionProgress[missionId]}/{mission.qty}");

        if (missionProgress[missionId] >= mission.qty && !completedMissions.Contains(missionId))
        {
            completedMissions.Add(missionId);
            Debug.Log($"Mission '{mission.title}' completed!");
            StartCoroutine(PlayMissionCompleteSFXDelayed());
        }

        MissionUIManager.Instance?.UpdateMissionProgressUI(missionId, missionProgress[missionId], mission.qty);
        SaveMissionProgress();
    }

    // Temp Script
    private IEnumerator PlayMissionCompleteSFXDelayed()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.SFXMissionCompleted();
    }

    public void ResetMissionProgress()
    {
        missionProgress.Clear();
        completedMissions.Clear();

        if (File.Exists(progressSavePath))
        {
            File.Delete(progressSavePath);
            Debug.Log("Mission progress reset and save file deleted.");
        }

        if (missionData != null)
        {
            foreach (var mission in missionData.missions)
            {
                missionProgress[mission.id] = 0;
            }
        }

        foreach (var mission in missionData.missions)
        {
            MissionUIManager.Instance?.UpdateMissionProgressUI(mission.id, 0, mission.qty);
        }
    }


    public bool IsMissionAvailable(Mission mission)
    {
        if (string.IsNullOrEmpty(mission.condition))
            return true;

        if (int.TryParse(mission.condition, out int requiredMissionId))
        {
            return IsMissionCompleted(requiredMissionId);
        }

        return true;
    }

    public bool IsMissionCompleted(int missionId)
    {
        return completedMissions.Contains(missionId);
    }

    public int GetMissionProgress(int missionId)
    {
        return missionProgress.TryGetValue(missionId, out int progress) ? progress : 0;
    }

    public Mission GetMissionById(int missionId)
    {
        if (missionData == null || missionData.missions == null)
            return null;

        return missionData.missions.Find(m => m.id == missionId);
    }

    public void OnItemCollected(ItemType item)
    {
        UpdateToolCollectionProgress(item);
    }

    // Mission 1.1 – Collect tools
    private void UpdateToolCollectionProgress(ItemType item)
    {
        const int toolMissionId = 1;

        Mission mission = GetMissionById(toolMissionId);
        if (mission == null) return;

        if (collectedTools.Contains(item)) return;

        if (item == ItemType.Camera || item == ItemType.Vacuum)
        {
            collectedTools.Add(item);
            UpdateMissionProgress(toolMissionId, 1);
        }
    }

    // Mission 1.2 – Collect garbage
    public void OnGarbageCollected()
    {
        const int garbageMissionId = 2;

        Mission mission = GetMissionById(garbageMissionId);
        if (mission == null) return;

        int progress = GetMissionProgress(garbageMissionId);

        if (progress < mission.qty)
        {
            UpdateMissionProgress(garbageMissionId, 1);
        }
    }

    // Mission 1.3 – Take photos
    public void OnPhotoTaken()
    {
        const int photoMissionId = 3;

        Mission mission = GetMissionById(photoMissionId);
        if (mission == null) return;

        int progress = GetMissionProgress(photoMissionId);

        if (progress < mission.qty)
        {
            UpdateMissionProgress(photoMissionId, 1);
        }
    }
}
