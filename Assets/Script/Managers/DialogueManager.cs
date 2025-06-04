using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    private DialogueEntry[] currentDialogue;
    private int currentLineIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (currentDialogue != null && Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }

    public void StartDialogue(DialogueEntry[] dialogues)
    {
        if (dialogues == null || dialogues.Length == 0)
        {
            Debug.LogWarning("No dialogue entries provided.");
            return;
        }
        GameplayManager.Instance.TalkingWithNPC();
        currentDialogue = dialogues;
        currentLineIndex = 0;
        ShowCurrentLine();
    }

    public void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < currentDialogue.Length)
        {
            ShowCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void ShowCurrentLine()
    {
        DialogueEntry line = currentDialogue[currentLineIndex];
        DialogueUI.Instance.Show(line.speaker, line.text);
    }

    public void EndDialogue()
    {
        DialogueUI.Instance.Hide();
        currentDialogue = null;
        currentLineIndex = 0;
        GameplayManager.Instance.FinishTalkingWithNPC();
    }
}
