using System;

[Serializable]
public class DialogueEntry
{
    public string speaker;
    public string text;
}

[Serializable]
public class DialogueConversation
{
    public string conversationId;
    public string npcName;
    public DialogueEntry[] dialogues;
}
