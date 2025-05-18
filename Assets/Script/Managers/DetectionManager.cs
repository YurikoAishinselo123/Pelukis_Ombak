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
            interactionTriggered = false; // Reset when interaction input is released
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
        bool doorDetected = false;

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
                    ItemPickup item = hit.collider.GetComponent<ItemPickup>();
                    if (item != null)
                    {
                        if (item.itemType == ItemType.Door)
                        {
                            detectedDoorTag = hit.collider.tag;
                            doorDetected = true;
                            detectedItem = null;

                            DetectDoorUI.Instance.ShowDetectDoor();
                            DetectItemUI.Instance.HideDetectItemUI();
                            break;
                        }
                        else if (validItemTypes.Contains(item.itemType))
                        {
                            detectedItem = item;
                            detectedDoorTag = null;

                            DetectItemUI.Instance.ShowDetectItemUI(item.itemType.ToString());
                            DetectDoorUI.Instance.HideDetectDoor();
                            break;
                        }
                    }
                    else if (hit.collider.CompareTag(npcTag))
                    {
                        detectedNPC = hit.collider.gameObject;
                        detectedItem = null;
                        detectedDoorTag = null;

                        DetectItemUI.Instance.ShowDetectItemUI("Talk");
                        DetectDoorUI.Instance.HideDetectDoor();
                        break;
                    }
                }
            }
            if (detectedItem != null || doorDetected || detectedNPC != null)
                break;
        }

        if (!doorDetected && detectedItem == null && detectedNPC == null)
        {
            DetectItemUI.Instance.HideDetectItemUI();
            DetectDoorUI.Instance.HideDetectDoor();
        }
    }
}
