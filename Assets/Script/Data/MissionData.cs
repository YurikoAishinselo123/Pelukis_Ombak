using System.Collections.Generic;

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