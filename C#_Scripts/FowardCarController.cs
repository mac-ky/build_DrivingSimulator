using UnityEngine;
using PathCreation;

public class ForwardCarController : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float normalSpeed = 50f;  // 通常速度 (km/h)
    public float reducedSpeed = 30f;  // 減速時の速度 (km/h)
    
    public float triggerTime = 5f; // 発進までの待機時間（秒）

    private float currentSpeed;      
    private float distanceTravelled;

    private bool isAutoDriveActive = false;
    private bool isInSlowZone = false;
    private bool isInStopZone = false;

    private GameObject stopZoneObject;

    private float elapsedTime = 0f;  // 経過時間

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
            if (!isAutoDriveActive)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= triggerTime)
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