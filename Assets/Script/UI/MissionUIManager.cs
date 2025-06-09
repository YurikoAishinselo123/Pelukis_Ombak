using UnityEngine;
using System.Collections.Generic;

public class MissionUIManager : MonoBehaviour
{
    public MissionManager missionManager;

    [Header("Mission Panel UI")]
    public GameObject missionPanelPrefab;
    public Transform contentParent;
    public GameObject MissionUICanvas;
    public GameObject WrapedMissionUICanvas;

    [Header("Popup Mission Progress UI")]
    public GameObject missionProgressUIPrefab;     // Assign your MissionProgressUI prefab
    public Transform missionPopupParent;           // Assign a parent UI panel for popup (e.g., top-right corner)

    public static MissionUIManager Instance;

    private Dictionary<int, MissionUI> missionUIMap = new Dictionary<int, MissionUI>();

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
        }
    }

    private void Start()
    {
        HideMissionUI();

        if (missionManager == null || missionManager.missionData == null || missionManager.missionData.missions == null)
        {
            Debug.LogWarning("MissionManager or mission data is missing!");
            return;
        }

        foreach (Mission mission in missionManager.missionData.missions)
        {
            GameObject panel = Instantiate(missionPanelPrefab, contentParent);
            MissionUI missionUI = panel.GetComponent<MissionUI>();

            if (missionUI != null)
            {
                int progress = missionManager.GetMissionProgress(mission.id);
                missionUI.Init(mission, progress, mission.qty);
                missionUIMap[mission.id] = missionUI;
            }
            else
            {
                Debug.LogWarning("MissionUI component not found on the panel prefab!");
            }
        }
    }

    public void ShowMissionUI()
    {
        MissionUICanvas.SetActive(true);
        WrapedMissionUICanvas.SetActive(false);
    }

    public void HideMissionUI()
    {
        MissionUICanvas.SetActive(false);
        WrapedMissionUICanvas.SetActive(true);
    }

    public void UpdateMissionProgressUI(int missionId, int progress, int maxProgress)
    {
        Debug.Log($"Updating Mission UI: ID={missionId}, Progress={progress}/{maxProgress}");

        if (missionUIMap.TryGetValue(missionId, out MissionUI missionUI))
        {
            Mission mission = missionManager.GetMissionById(missionId);
            if (mission != null)
            {
                missionUI.Init(mission, progress, maxProgress);
            }
            else
            {
                Debug.LogWarning($"Mission with ID {missionId} not found!");
            }
        }
        else
        {
            Debug.LogWarning($"No MissionUI found for Mission ID {missionId}.");
        }

        // Show popup progress UI
        ShowMissionProgressPopup(missionId, progress, maxProgress);
    }

    public void ShowMissionProgressPopup(int missionId, int current, int total)
    {
        Mission mission = missionManager.GetMissionById(missionId);
        if (mission == null || missionProgressUIPrefab == null) return;

        GameObject popup = Instantiate(missionProgressUIPrefab, missionPopupParent ?? transform);
        MissionProgressUI popupUI = popup.GetComponent<MissionProgressUI>();
        if (popupUI != null)
        {
            popupUI.Show($"{mission.title}", current, total); // Shows e.g., "Collect Item   1/2"
        }
        else
        {
            Debug.LogWarning("MissionProgressUI script not found on popup prefab.");
        }
    }

    public void RefreshAllMissionsUI()
    {
        foreach (var mission in MissionManager.Instance.missionData.missions)
        {
            UpdateMissionProgressUI(mission.id, 0, mission.qty);
        }
    }
}
