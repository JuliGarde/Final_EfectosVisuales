
using UnityEngine;

public class KeyTrigger : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] ConditionDoor conditionDoor;
    [SerializeField] float distanceDetection = 2f;
    [SerializeField] LayerMask obstructsLayers;
    private Transform followTarget;
    private Vector3 unlockPoint;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float followDelay = 0.1f;
    [SerializeField] private float offsetY;

    [Header("Separation")]
    [SerializeField] private float separationRadius = 1.5f;
    [SerializeField] private float separationStrength = 1f;

    private bool isActive;
    private bool isNearPoint;

    private void Start()
    {
        unlockPoint = conditionDoor.UnlockPoint.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isActive)
            {
                followTarget = other.transform;
                isActive = true;
            }

        }
    }

    private void Update()
    {
        FollowTarget();
        ToThePoint();
    }

    private void ToThePoint()
    {
        var distance = unlockPoint - this.transform.position;
        var direction = distance.normalized;

        if (distance.sqrMagnitude <= distanceDetection * distanceDetection)
        {
            if (!Physics.Raycast(this.transform.position, direction, 10f, obstructsLayers, QueryTriggerInteraction.Ignore))
            {
                isNearPoint = true;
                transform.position = Vector3.Lerp(transform.position, unlockPoint, followDelay * Time.deltaTime * moveSpeed * 5f);
            }

        }

        
        if (isNearPoint)
        {
            if (distance.sqrMagnitude <= 0.25f * 0.25f)
            {
                this.transform.position = unlockPoint;
                conditionDoor.OpenDoor();
                this.enabled = false;
            }
        }

    }


    private void FollowTarget()
    {
        if (isNearPoint) return;

        if (isActive)
        {
            Vector3 targetWithOffset = followTarget.position + Vector3.up * offsetY;

            Vector3 separationForce = Vector3.zero;

            Collider[] colliders = Physics.OverlapSphere(transform.position, separationRadius);

            foreach (var col in colliders)
            {
                if (col.gameObject != this.gameObject && col.GetComponent<KeyTrigger>() != null)
                {
                    Vector3 away = transform.position - col.transform.position;
                    float dist = away.magnitude;

                    if (dist > 0)
                    {
                        separationForce += away.normalized / dist;
                    }
                }
            }

            Vector3 desiredPosition = targetWithOffset + separationForce * separationStrength;

            float distanceSq = (desiredPosition - transform.position).sqrMagnitude;

            if (distanceSq > 0.8f)
            {
                transform.position = Vector3.Lerp(transform.position, desiredPosition, followDelay * Time.deltaTime * moveSpeed);
            }
        }
    }

    public void DestroyTheKey()
    {
        Destroy(this.gameObject);
    }
}
