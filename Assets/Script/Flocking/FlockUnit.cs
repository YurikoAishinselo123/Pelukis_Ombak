using System;
using System.Collections.Generic;
using UnityEngine;

public class FlockUnit : MonoBehaviour
{
    [SerializeField] private float FOVAngle;
    [SerializeField] private float smoothDamp;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Vector3[] directionsToCheckWhenAvoidingObstacles;

    private List<FlockUnit> cohesionNeigbours = new List<FlockUnit>();
    private List<FlockUnit> avoidanceNeigbours = new List<FlockUnit>();
    private List<FlockUnit> alignmentNeigbours = new List<FlockUnit>();

    private Flock assignedFlock;
    private Vector3 currentVelocity;
    private Vector3 currentObstacleAvoidanceVector;
    private float speed;
    public Transform myTransform { get; private set; }

    private void Awake()
    {
        myTransform = transform;
    }

    public void AssignedFlock(Flock flock)
    {
        assignedFlock = flock;
    }

    public void InitializeSpeed(float speed)
    {
        this.speed = speed;
    }

    public void MoveUnit()
    {
        FindNeighbours();
        calculateSpeed();
        var cohesionVector = CalculateCohesionVector();
        var avoidanceVector = CalculateAvoidanceVector();
        var alignmentVector = CalculateAlignmentVector();
        var boundsVector = CalculateBoundsVector() * assignedFlock.boundsWeight;
        var obstacleVector = CalculateObstacleVector() * assignedFlock.obstacleWeight;
        var moveVector = cohesionVector + avoidanceVector + alignmentVector;
        moveVector = cohesionVector + avoidanceVector + alignmentVector + boundsVector + obstacleVector;
        moveVector = Vector3.SmoothDamp(myTransform.forward, moveVector, ref currentVelocity, smoothDamp);
        myTransform.forward = moveVector;
        myTransform.position += moveVector * Time.deltaTime;
    }


    private void calculateSpeed()
    {
        if (cohesionNeigbours.Count == 0)
            return;

        speed = 0;
        for (int i = 0; i < cohesionNeigbours.Count; i++)
        {
            speed += cohesionNeigbours[i].speed;
        }
        speed /= cohesionNeigbours.Count;
        speed = Mathf.Clamp(speed, assignedFlock.minSpeed, assignedFlock.maxSpeed);
    }

    private void FindNeighbours()
    {
        cohesionNeigbours.Clear();
        avoidanceNeigbours.Clear();
        alignmentNeigbours.Clear();
        var allUnits = assignedFlock.allUnits;
        for (int i = 0; i < allUnits.Length; i++)
        {
            var currentUnit = allUnits[i];
            if (currentUnit != this)
            {
                float currentNeighbourDistanceSqr = Vector3.SqrMagnitude(currentUnit.transform.position - transform.position);
                if (currentNeighbourDistanceSqr <= assignedFlock.cohesionDistance * assignedFlock.cohesionDistance)
                {
                    cohesionNeigbours.Add(currentUnit);
                }
                if (currentNeighbourDistanceSqr <= assignedFlock.avoidanceDistance * assignedFlock.avoidanceDistance)
                {
                    avoidanceNeigbours.Add(currentUnit);
                }
                if (currentNeighbourDistanceSqr <= assignedFlock.alignmentDistance * assignedFlock.alignmentDistance)
                {
                    alignmentNeigbours.Add(currentUnit);
                }
            }
        }
    }

    private Vector3 CalculateCohesionVector()
    {
        var cohesionVector = Vector3.zero;
        if (cohesionNeigbours.Count == 0)
            return cohesionVector;

        int neighboursInFOV = 0;
        for (int i = 0; i < cohesionNeigbours.Count; i++)
        {
            if (IsInFov(cohesionNeigbours[i].myTransform.position))
            {
                neighboursInFOV++;
                cohesionVector += cohesionNeigbours[i].myTransform.position;
            }
        }

        cohesionVector /= neighboursInFOV;
        cohesionVector -= myTransform.position;
        cohesionVector = cohesionVector.normalized;
        return cohesionVector;
    }

    private Vector3 CalculateAvoidanceVector()
    {
        var avoidanceVector = Vector3.zero;
        if (alignmentNeigbours.Count == 0)
            return Vector3.zero;

        int neighboursInFOV = 0;
        for (int i = 0; i < avoidanceNeigbours.Count; i++)
        {
            if (IsInFov(avoidanceNeigbours[i].myTransform.position))
            {
                neighboursInFOV++;
                avoidanceVector += (myTransform.position - avoidanceNeigbours[i].myTransform.position);
            }
        }

        avoidanceVector /= neighboursInFOV;
        avoidanceVector = avoidanceVector.normalized;
        return avoidanceVector;
    }

    private Vector3 CalculateAlignmentVector()
    {
        var alignmentVector = myTransform.forward;
        if (alignmentNeigbours.Count == 0)
            return myTransform.forward;
        int neighboursInFOV = 0;
        for (int i = 0; i < alignmentNeigbours.Count; i++)
        {
            if (IsInFov(alignmentNeigbours[i].myTransform.position))
            {
                neighboursInFOV++;
                alignmentVector += alignmentNeigbours[i].myTransform.forward;
            }
        }

        alignmentVector /= neighboursInFOV;
        alignmentVector = alignmentVector.normalized;
        return alignmentVector;
    }

    private Vector3 CalculateBoundsVector()
    {
        var offsetToCenter = assignedFlock.transform.position - myTransform.position;
        bool isNearCenter = (offsetToCenter.magnitude >= assignedFlock.boundsDistance * 0.9f);
        return isNearCenter ? offsetToCenter.normalized : Vector3.zero;
    }

    Vector3 CalculateObstacleVector()
    {
        Collider[] obstacles = Physics.OverlapSphere(transform.position, assignedFlock.obstacleDistance, obstacleMask);
        Vector3 avoidanceVector = Vector3.zero;

        foreach (Collider obstacle in obstacles)
        {
            Vector3 directionToObstacle = transform.position - obstacle.transform.position;
            avoidanceVector += directionToObstacle.normalized;
        }

        if (obstacles.Length > 0)
        {
            avoidanceVector /= obstacles.Length;
        }

        return avoidanceVector.normalized;
    }


    private Vector3 FindBestDirectionToAvoidObstacle()
    {
        Collider[] obstacles = Physics.OverlapSphere(myTransform.position, assignedFlock.obstacleDistance, obstacleMask);

        if (obstacles.Length == 0)
        {

            currentObstacleAvoidanceVector = Vector3.zero;
            return Vector3.zero;
        }

        Vector3 selectedDirection = Vector3.zero;
        float maxDistance = float.MinValue;

        // Periksa setiap arah yang bisa digunakan untuk menghindari rintangan
        for (int i = 0; i < directionsToCheckWhenAvoidingObstacles.Length; i++)
        {
            Vector3 currentDirection = myTransform.TransformDirection(directionsToCheckWhenAvoidingObstacles[i].normalized);
            bool isSafe = true;
            float minObstacleDistance = float.MaxValue;

            // Periksa apakah arah ini memiliki obstacle terdekat
            foreach (var obstacle in obstacles)
            {
                float distance = Vector3.Distance(myTransform.position, obstacle.transform.position);
                if (distance < assignedFlock.obstacleDistance)
                {
                    isSafe = false;
                    if (distance < minObstacleDistance)
                    {
                        minObstacleDistance = distance;
                    }
                }
            }

            // Pilih arah yang paling jauh dari obstacle terdekat
            if (isSafe || minObstacleDistance > maxDistance)
            {
                maxDistance = minObstacleDistance;
                selectedDirection = currentDirection;
            }
        }

        currentObstacleAvoidanceVector = selectedDirection.normalized;
        return selectedDirection.normalized;
    }




    private bool IsInFov(Vector3 position)
    {
        return Vector3.Angle(myTransform.forward, position - myTransform.position) <= FOVAngle;
    }
}