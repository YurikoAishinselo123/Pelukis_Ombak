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

    [Header("Camera Settings")]
    private float lookSensitivity = 100f;
    [SerializeField] private Transform cameraTransform;
    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;

    [Header("Diving Settings")]
    [SerializeField] public bool isDiving;
    [SerializeField] private float divingSpeed = 3f;
    [SerializeField] private bool isSwimming = false;

    [Header("Sounds Settings")]
    private float stepDistance = 0.2f;
    private float distanceMoved = 0f;
    private Vector3 lastPosition;

    [Header("References")]
    private CharacterController characterController;
    private Vector3 velocity;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // HandleDivingMovement();
        HandleLook();
        HandleMovement();
        ApplyGravity();
        HandleJump();
        HandleItemCollection();
    }

    private void Start()
    {
        lastPosition = transform.position;
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

    public class CameraGizmoDrawer : MonoBehaviour
    {
        public Camera dragCamera;

        private void OnDrawGizmos()
        {
            if (dragCamera == null) return;

            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(dragCamera.transform.position, dragCamera.transform.forward * 5f);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(dragCamera.transform.position + dragCamera.transform.forward * 3f, 0.5f);
        }
    }

    private void HandleMovement()
    {
        Vector2 moveInput = InputManager.Instance.MoveInput;
        bool isSprinting = InputManager.Instance.IsSprinting;

        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

        // Hitung distance moved
        if (characterController.isGrounded && moveInput.magnitude > 0.1f)
        {
            float distanceThisFrame = Vector3.Distance(transform.position, lastPosition);
            distanceMoved += distanceThisFrame;

            if (distanceMoved >= stepDistance)
            {
                // AudioManager.Instance.PlayClikUI(); 
                distanceMoved = 0f;
            }
        }

        lastPosition = transform.position;
    }

    private void HandleDivingMovement()
    {
        Vector2 moveInput = InputManager.Instance.MoveInput;
        float horizontal = moveInput.x;
        float vertical = Mathf.Max(0f, moveInput.y);

        Vector3 move = transform.forward * vertical + transform.right * horizontal;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            move += Vector3.up;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            move += Vector3.down;
        }

        characterController.Move(move * divingSpeed * Time.deltaTime);
    }


    private void HandleJump()
    {
        if (InputManager.Instance.JumpPressed)
        {
            Debug.Log("Jump : " + characterController.isGrounded);
            if (characterController.isGrounded)
            {
                // AudioManager.Instance.PlayClikUI();
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }
    }

    private void HandleLook()
    {
        Vector2 lookInput = InputManager.Instance.LookInput;
        float mouseX = lookInput.x * lookSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * lookSensitivity * Time.deltaTime;

        horizontalRotation += mouseX;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);

        if (isDiving)
        {
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
            transform.rotation = Quaternion.Euler(0, horizontalRotation, 0);
        }
        else
        {
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
            transform.rotation = Quaternion.Euler(0, horizontalRotation, 0);
        }
    }


    private void HandleItemCollection()
    {
        if (InputManager.Instance.IsItemCollectPressed)
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
