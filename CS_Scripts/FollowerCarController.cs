using UnityEngine;
using PathCreation;

public class FollowerCarController : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float normalSpeed = 30f;   // 通常速度 (km/h)
    public float reducedSpeed = 15f;  // 減速時の速度 (km/h)
    public float triggerTime = 5f;    // 発進までの待機時間（秒）

    private float currentSpeed;
    private float distanceTravelled;

    private bool isAutoDriveActive = false;
    private bool isInSlowZone = false;
    private bool isInStopZone = false;
    private GameObject stopZoneObject;
    private float elapsedTime = 0f;

    void Start()
    {
        if (pathCreator != null)
        {
            pathCreator.pathUpdated += OnPathChanged;
            distanceTravelled = 0f;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        }

        currentSpeed = 0f; // 初期は停止
    }

    void Update()
    {
        if (pathCreator != null)
        {
            // 発進タイミングを計測
            if (!isAutoDriveActive)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= triggerTime)
                {
                    isAutoDriveActive = true;
                    currentSpeed = normalSpeed;
                }
            }

            // 自動走行が有効 & 停止ゾーンでないときのみ移動
            if (isAutoDriveActive && !isInStopZone)
            {
                float speedInMetersPerSecond = currentSpeed / 3.6f;
                distanceTravelled += speedInMetersPerSecond * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }
    }

    void FixedUpdate()
    {
        // StopZone が非アクティブになったら再出発
        if (isInStopZone && stopZoneObject != null && !stopZoneObject.activeInHierarchy)
        {
            isInStopZone = false;
            stopZoneObject = null;
            if (isAutoDriveActive)
            {
                currentSpeed = isInSlowZone ? reducedSpeed : normalSpeed;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SlowZone") && isAutoDriveActive)
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
        if (other.CompareTag("SlowZone") && isAutoDriveActive)
        {
            isInSlowZone = false;
            if (!isInStopZone)
                currentSpeed = normalSpeed;
        }
        else if (other.CompareTag("StopZone"))
        {
            isInStopZone = false;
            stopZoneObject = null;
            if (isAutoDriveActive)
                currentSpeed = isInSlowZone ? reducedSpeed : normalSpeed;
        }
    }

    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
