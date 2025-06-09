using UnityEngine;

public class FishManager : MonoBehaviour
{
    [Header("Fish Settings")]
    public float swimSpeed = 2f;  // Adjust for smoother movement
    public float turnSpeed = 2f;  // Adjust to control rotation speed
    public float idleDuration = 2f;  // Time before idle fish moves again

    [Header("Ocean Area")]
    public OceanArea oceanArea;

    private Vector3 targetPosition;
    private float idleTimer;
    private bool isIdle;

    void Start()
    {
        PickNewTarget();
    }

    void FixedUpdate()
    {
        if (isIdle)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0f)
            {
                isIdle = false;
                PickNewTarget();
            }
            return;
        }

        MoveTowardsTarget();

        if (!oceanArea.IsInside(transform.position))
        {
            PickNewTarget();
        }
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        // If the fish is close to the target, start idle
        if (direction.magnitude < 0.5f)
        {
            StartIdle();
            return;
        }

        // Smooth rotation (Slerp for smooth turning)
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);

        // Smooth movement (Lerp for smooth position change)
        // Use swimSpeed adjusted with Time.deltaTime for better frame rate independence
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, swimSpeed * Time.deltaTime);
    }

    void PickNewTarget()
    {
        if (oceanArea != null)
        {
            targetPosition = oceanArea.GetRandomPosition();
        }
    }

    void StartIdle()
    {
        isIdle = true;
        idleTimer = idleDuration;
    }
}
