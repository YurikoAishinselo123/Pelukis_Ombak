using UnityEngine;
using System.IO;

public class NPCInteraction : MonoBehaviour
{
    private string jsonFileName = "Dialog/dialogue_1.json";

    public DialogueEntry[] dialogues { get; private set; }

    private void Awake()
    {
        LoadDialogue();
    }

    private void LoadDialogue()
    {
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            DialogueConversation convo = JsonUtility.FromJson<DialogueConversation>(json);
            if (convo != null)
                dialogues = convo.dialogues;
            else
                Debug.LogWarning("Dialogue JSON is invalid: " + path);
        }
        else
        {
            Debug.LogWarning("Dialogue file not found: " + path);
        }
    }

    public void TriggerDialogue()
    {
        if (dialogues != null && dialogues.Length > 0)
        {
            DialogueManager.Instance.StartDialogue(dialogues);
        }
        else
        {
            Debug.LogWarning("No dialogues loaded for NPC " + gameObject.name);
        }
    }
}
