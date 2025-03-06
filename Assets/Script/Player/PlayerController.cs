using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float gravity = -9.81f;

    [Header("Pickup Settings")]
    private float pickupRange = 1f;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
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
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickupRange);

            foreach (Collider hit in hitColliders)
            {
                ItemPickup item = hit.GetComponent<ItemPickup>();
                if (item != null)
                {
                    Debug.Log("Item within range: " + item.gameObject.name);
                    item.Collect();
                }
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
