using UnityEngine;
using PathCreation;

public class FollowerCarController : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float normalSpeed = 30f;   // 通常速度 (km/h)
    public float reducedSpeed = 15f;  // 減速時の速度 (km/h)

    private float currentSpeed;
    private float distanceTravelled;

    private bool isInSlowZone = false;
    private bool isInStopZone = false;
    private GameObject stopZoneObject;

    void Start()
    {
        if (pathCreator != null)
        {
            pathCreator.pathUpdated += OnPathChanged;
            distanceTravelled = 0f;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        }

        currentSpeed = normalSpeed;
    }

    void Update()
    {
        if (pathCreator != null && !isInStopZone)
        {
            float speedInMetersPerSecond = currentSpeed / 3.6f;
            distanceTravelled += speedInMetersPerSecond * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        }
    }

    void FixedUpdate()
    {
        // StopZone が非アクティブになったら再出発
        if (isInStopZone && stopZoneObject != null && !stopZoneObject.activeInHierarchy)
        {
            isInStopZone = false;
            stopZoneObject = null;
            currentSpeed = isInSlowZone ? reducedSpeed : normalSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SlowZone"))
        {
            isInSlowZone = true;
            if (!isInStopZone)
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
        if (other.CompareTag("SlowZone"))
        {
            isInSlowZone = false;
            if (!isInStopZone)
                currentSpeed = normalSpeed;
        }
        else if (other.CompareTag("StopZone"))
        {
            isInStopZone = false;
            stopZoneObject = null;
            currentSpeed = isInSlowZone ? reducedSpeed : normalSpeed;
        }
    }

    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}