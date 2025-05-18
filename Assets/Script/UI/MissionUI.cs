using TMPro;
using UnityEngine;

public class MissionUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI progressText;

    private Mission mission;

    // Initialize the UI with mission info and progress
    public void Init(Mission missionData, int progress, int maxProgress)
    {
        mission = missionData;

        if (mission == null)
        {
            Debug.LogWarning("Mission is null!");
            return;
        }

        titleText.text = mission.title;
        descriptionText.text = mission.description;
        UpdateProgress(progress, maxProgress);
    }

    // Update progress text dynamically
    public void UpdateProgress(int progress, int maxProgress)
    {
        progressText.text = $"{progress}/{maxProgress}";
    }
}
