using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform mainCamera;
    [SerializeField] PlayerAnimator playerAnimator;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 6.5f;
    [SerializeField] private float acceleration = 40f;
    [SerializeField] private float rotationSpeed = 15f;

    private Vector3 inputDirection;
    private Vector3 moveDirection;

    public float CurrentSpeed;

    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        playerAnimator.Initialize(rb, this);
    }

    private void Update()
    {
        playerAnimator.Tick();
        ReadInput();
    }

    private void FixedUpdate()
    {
        CalculateMoveDirection();
        ApplyMovement();
        ApplyRotation();
        UpdateCurrentSpeed();
    }

    private void ReadInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        inputDirection = new Vector3(h, 0f, v).normalized;
    }

    private void CalculateMoveDirection()
    {
        if (!mainCamera) return;

        Vector3 camForward = mainCamera.forward;
        Vector3 camRight = mainCamera.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        moveDirection = camForward * inputDirection.z + camRight * inputDirection.x;
        moveDirection.Normalize();
    }

    private void ApplyMovement()
    {
        Vector3 targetVelocity = moveDirection * speed;

        Vector3 currentVelocity = rb.velocity;
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);

        Vector3 velocityChange = targetVelocity - horizontalVelocity;
        velocityChange = Vector3.ClampMagnitude(velocityChange, acceleration * Time.fixedDeltaTime);

        rb.velocity = new Vector3(
            horizontalVelocity.x + velocityChange.x,
            currentVelocity.y,
            horizontalVelocity.z + velocityChange.z
        );
    }

    private void ApplyRotation()
    {
        if (moveDirection.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        rb.MoveRotation(
            Quaternion.RotateTowards(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            )
        );
    }

    private void UpdateCurrentSpeed()
    {
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0f;

        float speed = horizontalVelocity.magnitude;

        if (speed <= 0.1f)
            speed = 0f;

        CurrentSpeed = speed;
    }
}
