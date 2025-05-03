using UnityEngine;
using PathCreation;

public class OncomingCarController : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float normalSpeed = 50f;  // 通常速度 (km/h)
    public float reducedSpeed = 30f;  // 減速時の速度 (km/h)
    public float triggerDistance = 200f; // 自動運転を開始する距離 (m)
    public Transform targetCar; // 他のプレイヤーの車を指定するTransform

    private float currentSpeed;
    private float distanceTravelled;

    private bool isAutoDriveActive = false;
    private bool isInSlowZone = false;
    private bool isInStopZone = false;

    private GameObject stopZoneObject;

    public float CurrentSpeed
    {
        get => currentSpeed;
        set => currentSpeed = value;
    }

    void Start()
    {
        if (pathCreator != null)
        {
            pathCreator.pathUpdated += OnPathChanged;

            distanceTravelled = 0f;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        }

        currentSpeed = 0f;
    }

    void Update()
    {
        if (pathCreator != null)
        {
            if (targetCar != null && !isAutoDriveActive)
            {
                float distanceToTarget = Vector3.Distance(transform.position, targetCar.position);

                if (distanceToTarget <= triggerDistance)
                {
                    isAutoDriveActive = true;
                    currentSpeed = normalSpeed;
                }
            }

            if (isAutoDriveActive && !isInStopZone)
            {
                float speedInMetersPerSecond = currentSpeed / 3.6f;
                distanceTravelled += speedInMetersPerSecond * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isInStopZone && stopZoneObject != null && !stopZoneObject.activeInHierarchy)
        {
            isInStopZone = false;
            currentSpeed = isInSlowZone ? reducedSpeed : normalSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SlowZone") && isAutoDriveActive)
        {
            isInSlowZone = true;
            currentSpeed = reducedSpeed;
        }
        else if (other.CompareTag("StopZone"))
        {
            isInStopZone = true;
            stopZoneObject = other.gameObject;
            currentSpeed = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SlowZone") && isAutoDriveActive)
        {
            isInSlowZone = false;
            if (!isInStopZone)
            {
                currentSpeed = normalSpeed;
            }
        }
        else if (other.CompareTag("StopZone"))
        {
            isInStopZone = false;
            stopZoneObject = null;
            if (isAutoDriveActive)
            {
                currentSpeed = isInSlowZone ? reducedSpeed : normalSpeed;
            }
        }
    }

    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
