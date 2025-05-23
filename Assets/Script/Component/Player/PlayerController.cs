using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    private float walkSpeed = 2f;
    private float sprintSpeed = 3f;
    private Vector3 moveDirection;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    private float lookSensitivity = 100f;
    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;

    [Header("Jump Settings")]
    private float gravity = -9.81f;
    private float jumpForce = 0.3f;
    private Vector3 velocity;

    [Header("Diving Settings")]
    private bool isDiving = false;
    private float divingSpeed = 3f;
    private float divingSprintSpeed = 4f;

    [Header("References")]
    private CharacterController characterController;
    public static PlayerController Instance;

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
        // HandleCursor();

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

        float currentSpeed = GetCurrentSpeed();
        moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y) * currentSpeed;
    }


    private float GetCurrentSpeed()
    {
        if (isDiving)
        {
            return InputManager.Instance.IsSprinting ? divingSprintSpeed : divingSpeed;
        }
        else
        {
            return InputManager.Instance.IsSprinting ? sprintSpeed : walkSpeed;
        }
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

        // Move direction based on camera orientation
        Vector3 moveDirection = (cameraTransform.forward * vertical + cameraTransform.right * horizontal).normalized;

        float currentSpeed = GetCurrentSpeed();
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (InputManager.Instance.JumpPressed && !isDiving && !GameplayManager.Instance.OnInteraction())
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

    private void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    public void OfficeEnvirontment()
    {
        isDiving = false;
    }

    public void OceanEnvirontment()
    {
        isDiving = true;
        Debug.Log("is Diving : " + isDiving);
    }

    // private void HandleCursor()
    // {
    //     if (InputManager.Instance.ShowCursor)
    //         CursorManager.Instance.ShowCursor();
    //     else if (!InputManager.Instance.ShowCursor && !PauseUI.Instance.isPaused)
    //         CursorManager.Instance.HideCursor();
    // }
}