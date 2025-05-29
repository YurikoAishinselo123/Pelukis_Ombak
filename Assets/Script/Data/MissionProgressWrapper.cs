using System;
using System.Collections.Generic;

[Serializable]
public class MissionProgressWrapper
{
    public List<MissionEntry> entries = new List<MissionEntry>();

    public MissionProgressWrapper(Dictionary<int, int> missionProgress)
    {
        foreach (var pair in missionProgress)
        {
            entries.Add(new MissionEntry { missionId = pair.Key, progress = pair.Value });
        }
    }

    public Dictionary<int, int> ToDictionary()
    {
        Dictionary<int, int> result = new Dictionary<int, int>();
        foreach (var entry in entries)
        {
            result[entry.missionId] = entry.progress;
        }
        return result;
    }
}

[Serializable]
public class MissionEntry
{
    public int missionId;
    public int progress;
}
