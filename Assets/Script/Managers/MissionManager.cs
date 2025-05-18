using System.Collections.Generic;
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

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }
    public MissionData missionData;

    private Dictionary<int, int> missionProgress = new Dictionary<int, int>();
    private HashSet<int> completedMissions = new HashSet<int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadMissionData();
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
                missionProgress[mission.id] = 0;
            }
        }
        else
        {
            Debug.LogError($"Mission JSON not found at path: {path}");
        }
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

        // Skip progress update if prerequisites not completed
        if (!IsMissionAvailable(mission))
        {
            Debug.Log($"Mission '{mission.title}' not yet available. Prerequisite not completed.");
            return;
        }

        // Clamp progress to max
        missionProgress[missionId] = Mathf.Min(missionProgress[missionId] + amount, mission.qty);

        Debug.Log($"Mission '{mission.title}' progress: {missionProgress[missionId]}/{mission.qty}");

        if (missionProgress[missionId] >= mission.qty && !completedMissions.Contains(missionId))
        {
            completedMissions.Add(missionId);
            Debug.Log($"Mission '{mission.title}' completed!");
            // TODO: Trigger rewards or next mission here
        }

        MissionUIManager.Instance?.UpdateMissionProgressUI(missionId, missionProgress[missionId], mission.qty);
    }

    public bool IsMissionAvailable(Mission mission)
    {
        if (string.IsNullOrEmpty(mission.condition))
            return true;

        // If condition is a single mission ID
        if (int.TryParse(mission.condition, out int requiredMissionId))
        {
            return IsMissionCompleted(requiredMissionId);
        }

        // Extend here to support more complex conditions if needed
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

    // Mission 1.1
    private void UpdateToolCollectionProgress(ItemType item)
    {
        const int toolMissionId = 1;

        Mission mission = GetMissionById(toolMissionId);
        if (mission == null) return;

        int progress = GetMissionProgress(toolMissionId);

        if ((item == ItemType.Camera || item == ItemType.Vacuum) && progress < mission.qty)
        {
            UpdateMissionProgress(toolMissionId, 1);
        }
    }

    // Mission 1.2
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

    // Mission 1.3
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
