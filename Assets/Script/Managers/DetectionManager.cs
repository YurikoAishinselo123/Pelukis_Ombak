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
    private bool interactionTriggered = false;


    private HashSet<ItemPickup.ItemType> validItemTypes = new HashSet<ItemPickup.ItemType>
    {
        ItemPickup.ItemType.Camera,
        ItemPickup.ItemType.Vacuum,
        ItemPickup.ItemType.Coin,
        ItemPickup.ItemType.Oxygen
    };


    private void Awake()
    {
        if (detectionCamera == null)
        {
            Debug.LogWarning("Detection camera not assigned! Please assign it in the inspector.");
        }
    }

    void Update()
    {
        DetectObject();

        if (InputManager.Instance.Interact)
        {
            if (!interactionTriggered)
            {
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

                interactionTriggered = true;
            }
        }
        else
        {
            interactionTriggered = false; // Reset when key is released
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
        bool doorDetected = false; // Track if any door was detected

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
                        if (item.itemType == ItemPickup.ItemType.Door)
                        {
                            detectedDoorTag = hit.collider.tag;
                            doorDetected = true;
                            detectedItem = null;

                            // Show door UI
                            PhotoCaptureUI.Instance.ShowDetectDoor();
                            DetectItemUI.Instance.HideItemUI();
                            break;
                        }
                        else if (validItemTypes.Contains(item.itemType))
                        {
                            detectedItem = item;
                            detectedDoorTag = null;

                            // Show item UI
                            DetectItemUI.Instance.ShowItemUI(item.itemType.ToString());
                            PhotoCaptureUI.Instance.HideDetectDoor();
                            break;
                        }
                    }
                }
            }

            if (detectedItem != null || doorDetected)
                break;
        }

        // Hide both UIs if nothing detected
        if (detectedItem == null && !doorDetected)
        {
            DetectItemUI.Instance.HideItemUI();
            PhotoCaptureUI.Instance.HideDetectDoor();
        }
    }

}
