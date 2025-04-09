using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class MissionData
{
    public List<Mission> missions;
}

[System.Serializable]
public class Mission
{
    public string id;
    public string title;
    public string description;
    public int? qty;
}

public class MissionManager : MonoBehaviour
{
    public MissionData missionData;

    void Awake()
    {
        LoadMissionData();
    }

    void LoadMissionData()
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
}

