using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 6f;
    private Vector3 moveDirection;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    private float lookSensitivity = 100f;
    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;

    [Header("Jump Settings")]
    private float gravity = -9.81f;
    private float jumpForce = 0.5f;
    private Vector3 velocity;

    [Header("Diving Settings")]
    [SerializeField] public bool isDiving = false;
    [SerializeField] private float divingSpeed = 3f;

    [Header("References")]
    private CharacterController characterController;
    public static PlayerController Instance;
    private bool isMissionUIVisible = false;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        characterController = GetComponent<CharacterController>();

    }

    private void Update()
    {
        HandleLook();
        HandleMission();
        if (isDiving)
        {
            HandleDivingMovement();
        }
        else
        {
            ApplyGravity();
            HandleMovement();
            HandleJump();
            MoveCharacter();
        }
    }

    private void HandleMovement()
    {
        Vector2 moveInput = InputManager.Instance.MoveInput;
        bool isSprinting = InputManager.Instance.IsSprinting;

        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y) * currentSpeed;
    }

    private void MoveCharacter()
    {
        Vector3 finalMove = moveDirection;
        finalMove.y = velocity.y;
        characterController.Move(finalMove * Time.deltaTime);
    }

    private void HandleDivingMovement()
    {
        Vector2 moveInput = InputManager.Instance.MoveInput;
        float horizontal = moveInput.x;
        float vertical = moveInput.y;

        Vector3 move = (transform.forward * vertical + transform.right * horizontal).normalized;

        // Temp Control
        if (Input.GetKey(KeyCode.Q))
        {
            move += Vector3.up;
        }

        if (Input.GetKey(KeyCode.E))
        {
            move += Vector3.down;
        }

        characterController.Move(move * divingSpeed * Time.deltaTime);
    }

    public void OceanEnvirontment()
    {
        isDiving = true;
    }

    public void OfficeEnvirontment()
    {
        isDiving = false;
    }

    private void HandleJump()
    {
        if (InputManager.Instance.JumpPressed && !isDiving)
        {
            Debug.Log("Jump : " + characterController.isGrounded);
            if (characterController.isGrounded)
            {
                // AudioManager.Instance.PlayClikUI();
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }
    }

    private void HandleMission()
    {
        if (InputManager.Instance.Mission)
        {
            Debug.Log("Mission : " + isMissionUIVisible);
            if (isMissionUIVisible)
            {
                MissionUIManager.Instance.HideMissionUI();
                InventoryUIManager.Instance.ShowInventoryCanvas();
            }
            else
            {
                MissionUIManager.Instance.ShowMissionUI();
                InventoryUIManager.Instance.HideInventoryCanvas();
            }
            isMissionUIVisible = !isMissionUIVisible;
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