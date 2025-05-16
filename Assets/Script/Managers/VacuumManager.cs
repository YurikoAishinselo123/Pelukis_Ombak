using UnityEngine;
using System.Collections.Generic;

public class VacuumManager : MonoBehaviour
{
    [Header("Vacuum Settings")]
    public Transform vacuumPoint;
    [SerializeField] private float rayLength = 5f;
    [SerializeField] private int rayCount = 20;
    [SerializeField] private float raySpreadAngle = 30f;
    private float absorbSpeed = 15f;
    // [SerializeField] private float absorbDistance = 1f;
    public LayerMask garbageLayerMask;

    [SerializeField] private GameObject vacuumObject;
    // [SerializeField] private Vector3 inactiveRotation = new Vector3(-25f, -7.8f, 2f);
    // [SerializeField] private Vector3 activeRotation = new Vector3(-10f, 0f, 2f);
    private List<Transform> grabbedObjects = new List<Transform>();

    private void Update()
    {
        ActiveVacuum();
    }

    private void ActiveVacuum()
    {
        if (InputManager.Instance.Action && ItemSelectorManager.Instance.SelectedVacuum)
        {
            // vacuumObject.transform.localRotation = Quaternion.Euler(activeRotation);
            ScanForGarbage();
            PullObjects();
        }
        else
        {
            // vacuumObject.transform.localRotation = Quaternion.Euler(inactiveRotation);
        }
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
                Debug.Log("tes");
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

        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(vacuumPoint.position, absorbDistance);
    }
}
