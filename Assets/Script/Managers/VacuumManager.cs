using UnityEngine;
using System.Collections.Generic;

public class VacuumManager : MonoBehaviour
{
    [Header("Vacuum Settings")]
    public Transform vacuumPoint;
    public float rayLength = 5f;
    public int rayCount = 10;
    public float raySpreadAngle = 10f;
    public float absorbSpeed = 5f;
    public float absorbDistance = 0.5f;
    public LayerMask garbageLayerMask;

    private List<Transform> grabbedObjects = new List<Transform>();

    private void Update()
    {
        ScanForGarbage();
        PullObjects();
    }

    private void ScanForGarbage()
    {
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 direction = Quaternion.Euler(
                Random.Range(-raySpreadAngle, raySpreadAngle),
                Random.Range(-raySpreadAngle, raySpreadAngle),
                0f) * vacuumPoint.forward;

            if (Physics.Raycast(vacuumPoint.position, direction, out RaycastHit hit, rayLength, garbageLayerMask))
            {
                if (!grabbedObjects.Contains(hit.transform))
                {
                    grabbedObjects.Add(hit.transform);
                }
            }
        }
    }

    private void PullObjects()
    {
        for (int i = grabbedObjects.Count - 1; i >= 0; i--)
        {
            Transform obj = grabbedObjects[i];

            if (obj == null)
            {
                grabbedObjects.RemoveAt(i);
                continue;
            }

            // Move the object toward the vacuum point
            obj.position = Vector3.MoveTowards(obj.position, vacuumPoint.position, absorbSpeed * Time.deltaTime);

            // Scale down as it gets closer
            float distance = Vector3.Distance(obj.position, vacuumPoint.position);

            // Destroy when it's close enough
            if (distance <= 3f)
            {
                Destroy(obj.gameObject);
                grabbedObjects.RemoveAt(i);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (vacuumPoint == null) return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < rayCount; i++)
        {
            Vector3 direction = Quaternion.Euler(
                Random.Range(-raySpreadAngle, raySpreadAngle),
                Random.Range(-raySpreadAngle, raySpreadAngle),
                0f) * vacuumPoint.forward;

            Gizmos.DrawRay(vacuumPoint.position, direction * rayLength);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(vacuumPoint.position, absorbDistance);
    }
}
