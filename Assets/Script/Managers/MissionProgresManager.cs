using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class MissionProgress
{
    public Dictionary<string, int> progressDict = new Dictionary<string, int>();

    public int GetProgress(string missionId)
    {
        return progressDict.ContainsKey(missionId) ? progressDict[missionId] : 0;
    }

    public void SetProgress(string missionId, int value)
    {
        if (progressDict.ContainsKey(missionId))
            progressDict[missionId] = value;
        else
            progressDict.Add(missionId, value);
    }
}

public static class MissionProgressManager
{
    private static string filePath => Path.Combine(Application.persistentDataPath, "mission_progress.json");

    public static MissionProgress LoadProgress()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<MissionProgress>(json);
        }
        return new MissionProgress();
    }

    public static void SaveProgress(MissionProgress data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }
}

