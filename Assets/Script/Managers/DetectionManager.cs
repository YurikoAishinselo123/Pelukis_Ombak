using System.Collections.Generic;
using UnityEngine;

public class DetectionManager : MonoBehaviour
{
    [Header("Detection Settings")]
    private float pickupRange = 2f;
    private float pickupAngle = 30f;
    private float angleStep = 15f;

    [Header("References")]
    [SerializeField] private Camera detectionCamera;

    private ItemPickup detectedItem = null;
    private DoorInteractable detectedDoor = null;
    private GameObject detectedNPC = null;

    private bool interactionTriggered = false;
    private const string npcTag = "Dayat";

    private HashSet<ItemType> validItemTypes = new HashSet<ItemType>
    {
        ItemType.Camera,
        ItemType.Vacuum,
        ItemType.Coin,
        ItemType.Oxygen
    };

    private void Awake()
    {
        if (detectionCamera == null)
            Debug.LogWarning("Detection camera not assigned!");
    }

    private void Update()
    {
        if (GameplayManager.Instance.OnInteractionWithNPC())
        {
            InteractionUIManager.Instance.HideAllInteractions();
            return;
        }

        if (UIManager.Instance.detectManagerActive)
            DetectObject();

        if (InputManager.Instance.Interact && !interactionTriggered)
        {
            interactionTriggered = true;

            if (detectedItem != null)
            {
                detectedItem.Collect();
                detectedItem = null;
            }
            else if (detectedDoor != null)
            {
                DoorManager.Instance.OpenDoor(detectedDoor.DoorTag);
                detectedDoor = null;
            }
            else if (detectedNPC != null)
            {
                NPCInteraction npcInteraction = detectedNPC.GetComponent<NPCInteraction>();
                if (npcInteraction != null)
                {
                    InteractionUIManager.Instance.HideAllInteractions();
                    DialogueManager.Instance.StartDialogue(npcInteraction.dialogues);
                }
                detectedNPC = null;
            }
        }
        else if (!InputManager.Instance.Interact)
        {
            interactionTriggered = false;
        }
    }

    private void DetectObject()
    {
        detectedItem = null;
        detectedDoor = null;
        detectedNPC = null;

        if (detectionCamera == null)
            return;

        Vector3 origin = detectionCamera.transform.position;
        Vector3 forward = detectionCamera.transform.forward;

        for (float h = -pickupAngle; h <= pickupAngle; h += angleStep)
        {
            for (float v = -pickupAngle; v <= pickupAngle; v += angleStep)
            {
                Quaternion rotation = Quaternion.Euler(v, h, 0);
                Vector3 direction = rotation * forward;

                if (Physics.Raycast(origin, direction, out RaycastHit hit, pickupRange))
                {
                    // Prioritize NPC detection via tag
                    if (hit.collider.CompareTag(npcTag))
                    {
                        detectedNPC = hit.collider.gameObject;
                        break;
                    }

                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                    if (interactable != null)
                    {
                        InteractionUIManager.Instance.ShowInteraction(interactable);

                        if (interactable is ItemPickup item && validItemTypes.Contains(item.itemType))
                        {
                            detectedItem = item;
                        }
                        else if (interactable is DoorInteractable door)
                        {
                            detectedDoor = door;
                            Debug.Log("detected door : " + detectedDoor);
                        }

                        break;
                    }
                }
            }

            if (detectedItem != null || detectedDoor != null || detectedNPC != null)
                break;
        }

        // Nothing detected
        if (detectedItem == null && detectedDoor == null && detectedNPC == null)
        {
            InteractionUIManager.Instance.HideAllInteractions();
        }
    }
}
