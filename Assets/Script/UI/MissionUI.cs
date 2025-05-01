using TMPro;
using UnityEngine;

public class MissionUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI progressText;

    public void Init(Mission mission, int progress = 0)
    {
        if (mission == null)
        {
            Debug.LogWarning("Mission is null!");
            return;
        }

        titleText.text = mission.title;
        descriptionText.text = mission.description;

        int max = mission.qty ?? 1;
        progressText.text = $"{progress}/{max}";
    }


}
