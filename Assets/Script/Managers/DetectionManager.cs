using System.Collections.Generic;
using UnityEngine;

public class DetectionManager : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float pickupRange = 5f;
    [SerializeField] private float pickupAngle = 30f;
    [SerializeField] private float angleStep = 10f;

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

        // Only collect or interact if detection succeeded AND player presses the button
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
                    Debug.Log("Open door: " + detectedDoorTag);
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

    void DetectObject()
    {
        detectedItem = null;
        detectedDoorTag = null;

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
                            return;
                        }
                        else if (validItemTypes.Contains(item.itemType))
                        {
                            detectedItem = item;
                            return;
                        }
                    }
                }
            }
        }
    }
}
