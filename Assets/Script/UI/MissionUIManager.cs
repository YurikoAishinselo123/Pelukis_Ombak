using UnityEngine;
using System.Collections.Generic;

public class MissionUIManager : MonoBehaviour
{
    public MissionManager missionManager;
    public GameObject missionPanelPrefab;
    public Transform contentParent;
    public GameObject MissionUICanvas;
    public static MissionUIManager Instance;

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
                missionUI.Init(mission, 0);
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
    }

    public void HideMissionUI()
    {
        MissionUICanvas.SetActive(false);
    }
}
