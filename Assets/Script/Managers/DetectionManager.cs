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
    private string detectedDoorTag = null;
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
        {
            Debug.LogWarning("Detection camera not assigned! Please assign it in the inspector.");
        }
    }

    private void Update()
    {
        if (UIManager.Instance.detectManagerActive)
            DetectObject();

        if (InputManager.Instance.Interact)
        {
            if (!interactionTriggered)
            {
                interactionTriggered = true;

                if (detectedItem != null)
                {
                    detectedItem.Collect();
                    detectedItem = null;
                }
                else if (detectedDoorTag != null)
                {
                    DoorManager.Instance.OpenDoor(detectedDoorTag);
                    detectedDoorTag = null;
                }
                else if (detectedNPC != null)
                {
                    NPCInteraction npcInteraction = detectedNPC.GetComponent<NPCInteraction>();

                    if (npcInteraction != null)
                    {
                        // Hide interaction UI when starting dialogue
                        InteractionUIManager.Instance.HideAllInteractions();

                        DialogueManager.Instance.StartDialogue(npcInteraction.dialogues);
                    }
                    else
                    {
                        Debug.LogWarning("Detected NPC does not have an NPCInteraction component attached.");
                    }

                    detectedNPC = null;
                }
            }
        }
        else
        {
            interactionTriggered = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (detectionCamera == null)
            return;

        Gizmos.color = Color.yellow;

        Vector3 origin = detectionCamera.transform.position;
        Vector3 forward = detectionCamera.transform.forward;

        for (float h = -pickupAngle; h <= pickupAngle; h += angleStep)
        {
            for (float v = -pickupAngle; v <= pickupAngle; v += angleStep)
            {
                Quaternion rotation = Quaternion.Euler(v, h, 0);
                Vector3 direction = rotation * forward;

                Gizmos.DrawRay(origin, direction * pickupRange);
            }
        }
    }

    private void DetectObject()
    {
        detectedItem = null;
        detectedDoorTag = null;
        detectedNPC = null;

        if (detectionCamera == null)
            return;

        if (UIManager.Instance.isTalkingWithNPC)
        {
            InteractionUIManager.Instance.HideAllInteractions();
            return;
        }

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
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                    if (interactable != null)
                    {
                        InteractionUIManager.Instance.ShowInteraction(interactable);

                        if (interactable is ItemPickup item)
                        {
                            if (item.itemType == ItemType.Door)
                            {
                                detectedDoorTag = hit.collider.tag;
                            }
                            else if (validItemTypes.Contains(item.itemType))
                            {
                                detectedItem = item;
                            }
                        }
                        else if (hit.collider.CompareTag(npcTag))
                        {
                            detectedNPC = hit.collider.gameObject;
                        }

                        break;
                    }
                }
            }

            if (detectedItem != null || detectedDoorTag != null || detectedNPC != null)
                break;
        }

        // No detection
        if (detectedItem == null && detectedDoorTag == null && detectedNPC == null)
        {
            InteractionUIManager.Instance.HideAllInteractions();
        }
    }
}
