using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystemManager : MonoBehaviour
{
    public static SaveSystemManager Instance { get; private set; }

    private string missionDirectory;
    private string missionFileName = "MissionProgress.json";

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            missionDirectory = Path.Combine(Application.persistentDataPath, "Save System", "Mission");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveMissionProgress(Dictionary<int, int> missionProgress)
    {
        string path = GetMissionProgressFilePath();

        // Ensure the directory exists
        if (!Directory.Exists(missionDirectory))
        {
            Directory.CreateDirectory(missionDirectory);
        }

        MissionProgressWrapper wrapper = new MissionProgressWrapper(missionProgress);
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(path, json);
        Debug.Log($" Mission progress saved to: {path}");
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

        Debug.Log(" No saved mission progress found. Starting fresh.");
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

        Debug.Log("Mission progress has been reset to zero.");
    }


    private string GetMissionProgressFilePath()
    {
        return Path.Combine(missionDirectory, missionFileName);
    }
}
