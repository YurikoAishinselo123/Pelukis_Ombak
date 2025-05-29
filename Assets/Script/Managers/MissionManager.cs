using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }
    public MissionData missionData;

    private Dictionary<int, int> missionProgress = new Dictionary<int, int>();
    private HashSet<int> completedMissions = new HashSet<int>();
    private HashSet<ItemType> collectedTools = new HashSet<ItemType>();

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

        LoadMissionData();
        missionProgress = SaveSystemManager.Instance.LoadMissionProgress();

        if (missionData != null)
        {
            foreach (var mission in missionData.missions)
            {
                if (!missionProgress.ContainsKey(mission.id))
                    missionProgress[mission.id] = 0;
            }
        }

        InitializeCompletedMissions(); // Evaluate completion after everything is loaded
    }

    private void LoadMissionData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Mission/Chapter1.json");

        if (File.Exists(path))
        {
            string jsonText = File.ReadAllText(path);
            missionData = JsonUtility.FromJson<MissionData>(jsonText);
            Debug.Log("Mission data loaded successfully!");
        }
        else
        {
            Debug.LogError($"Mission JSON not found at path: {path}");
        }
    }

    private void InitializeCompletedMissions()
    {
        if (missionData == null) return;

        foreach (var mission in missionData.missions)
        {
            if (missionProgress.TryGetValue(mission.id, out int progress) && progress >= mission.qty)
            {
                completedMissions.Add(mission.id);
            }
        }
    }

    public void UpdateMissionProgress(int missionId, int amount = 1)
    {
        Mission mission = GetMissionById(missionId);
        if (mission == null)
        {
            Debug.LogWarning($"Mission data for ID {missionId} not found.");
            return;
        }

        if (!missionProgress.ContainsKey(missionId))
        {
            Debug.LogWarning($"Mission ID {missionId} not found in missionProgress. Initializing.");
            missionProgress[missionId] = 0;
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
        SaveSystemManager.Instance.SaveMissionProgress(missionProgress);
    }

    private IEnumerator PlayMissionCompleteSFXDelayed()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance?.SFXMissionCompleted();
    }

    public void ResetMissionProgress()
    {
        missionProgress.Clear();
        completedMissions.Clear();
        collectedTools.Clear();

        SaveSystemManager.Instance.ResetMissionProgress();

        if (missionData != null)
        {
            foreach (var mission in missionData.missions)
            {
                missionProgress[mission.id] = 0;
            }
        }
        SaveSystemManager.Instance.SaveMissionProgress(missionProgress);
        MissionUIManager.Instance?.RefreshAllMissionsUI();
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
        return missionData?.missions?.Find(m => m.id == missionId);
    }

    public void OnItemCollected(ItemType item)
    {
        UpdateToolCollectionProgress(item);
    }

    private void UpdateToolCollectionProgress(ItemType item)
    {
        const int toolMissionId = 1;

        Mission mission = GetMissionById(toolMissionId);
        if (mission == null || collectedTools.Contains(item)) return;

        if (item == ItemType.Camera || item == ItemType.Vacuum)
        {
            collectedTools.Add(item);
            UpdateMissionProgress(toolMissionId, 1);
        }
    }

    public void OnGarbageCollected()
    {
        const int garbageMissionId = 2;
        if (GetMissionById(garbageMissionId) != null)
        {
            UpdateMissionProgress(garbageMissionId, 1);
        }
    }

    public void OnPhotoTaken()
    {
        const int photoMissionId = 3;
        if (GetMissionById(photoMissionId) != null)
        {
            UpdateMissionProgress(photoMissionId, 1);
        }
    }

}
