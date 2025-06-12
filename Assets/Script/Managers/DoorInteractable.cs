using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite icon;
    private KeyCode interactKey = KeyCode.E;

    public string DoorTag => gameObject.tag;

    public KeyCode InteractionKey => interactKey;
    public Sprite InteractionIcon => icon;
    public string InteractionText => "Move";
}
