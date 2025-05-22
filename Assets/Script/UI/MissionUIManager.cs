using UnityEngine;
using System.Collections.Generic;

public class MissionUIManager : MonoBehaviour
{
    public MissionManager missionManager;
    public GameObject missionPanelPrefab;
    public Transform contentParent;
    public GameObject MissionUICanvas;
    public GameObject WrapedMissionUICanvas;
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
                // qty is now a non-nullable int, so just use it directly
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
    }
}
