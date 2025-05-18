using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Show(string speaker, string dialogue)
    {
        dialoguePanel.SetActive(true);
        nameText.text = speaker;
        dialogueText.text = dialogue;
    }

    public void Hide()
    {
        dialoguePanel.SetActive(false);
    }
}
