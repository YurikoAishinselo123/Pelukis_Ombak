using UnityEngine;

public class OceanArea : MonoBehaviour
{
    [Header("Half-Sphere Bounds")]
    private float minY = 1.43f;
    private float maxY = 3.9f;
    private float minX = -15f;
    private float maxX = 12f;
    private float minZ = -8.7f;
    private float maxZ = 8.9f;

    private Vector3 center;
    private float radius;

    private void Awake()
    {
        float centerX = (minX + maxX) / 2f;
        float centerY = (minY + maxY) / 2f;
        float centerZ = (minZ + maxZ) / 2f;
        center = new Vector3(centerX, centerY, centerZ);

        radius = Vector3.Distance(center, new Vector3(maxX, maxY, maxZ));
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 point;

        // Keep trying until we find a point inside the half-sphere
        do
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            float z = Random.Range(minZ, maxZ);
            point = new Vector3(x, y, z);
        }
        while (!IsInside(point));

        return point;
    }

    public bool IsInside(Vector3 position)
    {
        // Check box bounds first
        if (position.x < minX || position.x > maxX ||
            position.y < minY || position.y > maxY ||
            position.z < minZ || position.z > maxZ)
            return false;

        // Then check if it's inside the hemisphere (upper half of sphere)
        float dist = Vector3.Distance(position, center);
        return dist <= radius && position.y >= center.y;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 0.6f, 1f, 0.2f);
        Vector3 drawCenter = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, (minZ + maxZ) / 2f);
        float drawRadius = Vector3.Distance(drawCenter, new Vector3(maxX, maxY, maxZ));
        Gizmos.DrawSphere(drawCenter, drawRadius);
    }
}
