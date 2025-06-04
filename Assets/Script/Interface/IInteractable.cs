using UnityEngine;

public interface IInteractable
{
    KeyCode InteractionKey { get; }
    Sprite InteractionIcon { get; }
    string InteractionText { get; }
}
