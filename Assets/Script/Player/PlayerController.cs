using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Pickup Settings")]
    [SerializeField] private float pickupRange = 2f;
    [SerializeField] private float pickupAngle = 30f;

    [Header("References")]
    private CharacterController characterController;
    private Vector3 velocity;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        // ApplyGravity();
        HandleItemCollection();
    }


    // Visualize Raycast Area for Collecting Item
    private void OnDrawGizmos()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        Vector3 origin = mainCamera.transform.position;
        Vector3 forward = mainCamera.transform.forward;

        Gizmos.color = Color.blue;

        float maxDistance = pickupRange;
        float angleStep = 15f;

        for (float horizontalAngle = -pickupAngle; horizontalAngle <= pickupAngle; horizontalAngle += angleStep)
        {
            for (float verticalAngle = -pickupAngle; verticalAngle <= pickupAngle; verticalAngle += angleStep)
            {
                Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
                Vector3 rayDirection = rotation * forward;
                Gizmos.DrawRay(origin, rayDirection * maxDistance);
            }
        }

        Gizmos.DrawWireSphere(origin, maxDistance);
    }


    private void HandleMovement()
    {
        Vector2 moveInput = InputManager.Instance.GetMoveInput();
        bool isSprinting = InputManager.Instance.IsSprinting();

        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    private void HandleItemCollection()
    {
        if (InputManager.Instance.IsItemCollectPressed())
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogWarning("Main camera not found!");
                return;
            }

            Vector3 origin = mainCamera.transform.position;
            Vector3 forward = mainCamera.transform.forward;
            float maxDistance = pickupRange;
            float angleStep = 10f;

            List<ItemPickup> detectedItems = new List<ItemPickup>();

            for (float horizontalAngle = -pickupAngle; horizontalAngle <= pickupAngle; horizontalAngle += angleStep)
            {
                for (float verticalAngle = -pickupAngle; verticalAngle <= pickupAngle; verticalAngle += angleStep)
                {
                    Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
                    Vector3 rayDirection = rotation * forward;

                    if (Physics.Raycast(origin, rayDirection, out RaycastHit hit, maxDistance))
                    {
                        ItemPickup item = hit.collider.GetComponent<ItemPickup>();
                        if (item != null && !detectedItems.Contains(item))
                        {
                            detectedItems.Add(item);
                        }
                    }
                }
            }

            foreach (ItemPickup item in detectedItems)
            {
                item.Collect();
            }
        }
    }


    private void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
