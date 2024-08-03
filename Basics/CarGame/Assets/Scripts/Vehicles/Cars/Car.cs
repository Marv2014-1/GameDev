using UnityEngine;

public class Car : MonoBehaviour
{
    protected Rigidbody rb;

    // Variables
    public float acceleration = 500f;
    public float brakingForce = 300f;
    public float veerForce = 5f;
    public float maxSpeed = 50f;
    protected float currentAcceleration = 0f;
    protected float currentBrakeForce = 0f;

    // Wheels
    [SerializeField] protected WheelCollider frontRight;
    [SerializeField] protected WheelCollider backRight;
    [SerializeField] protected WheelCollider frontLeft;
    [SerializeField] protected WheelCollider backLeft;

    // Get input for movement
    protected float moveInput;
    protected float veerInput;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }

    private void Update()
    {
        // Get user input
        moveInput = Input.GetAxis("Vertical");
        veerInput = Input.GetAxis("Horizontal");
    }

    // FixedUpdate is called once per physics update
    private void FixedUpdate()
    {
        HandleMovement();
        HandleVeering();
        AlignVelocityWithForward();
    }

    protected void HandleMovement()
    {
        if (moveInput > 0f)
        {
            Accelerate();
        }
        else if (moveInput < 0f)
        {
            Brake();
        }
        else
        {
            ResetBrakesAndAcceleration();
        }
    }

    protected void HandleVeering()
    {
        // Apply veering force directly to the rigidbody for simple lane merging
        Vector3 veerDirection = transform.right * veerInput * veerForce;
        rb.AddForce(veerDirection, ForceMode.Acceleration);
    }

    protected void Accelerate()
    {
        // Check the current speed
        float currentSpeed = rb.velocity.magnitude;

        // Only apply acceleration if the current speed is below the maximum speed
        if (currentSpeed < maxSpeed)
        {
            currentAcceleration = acceleration * moveInput;

            // Apply acceleration to front wheels
            frontRight.motorTorque = currentAcceleration;
            frontLeft.motorTorque = currentAcceleration;

            // Release brakes
            frontRight.brakeTorque = 0f;
            backRight.brakeTorque = 0f;
            frontLeft.brakeTorque = 0f;
            backLeft.brakeTorque = 0f;
        }
        else
        {
            // If the speed is at or above maxSpeed, don't accelerate
            frontRight.motorTorque = 0f;
            frontLeft.motorTorque = 0f;
        }
    }

    protected void Brake()
    {
        currentBrakeForce = brakingForce;

        // Apply brakes to wheels
        frontRight.brakeTorque = currentBrakeForce;
        backRight.brakeTorque = currentBrakeForce;
        frontLeft.brakeTorque = currentBrakeForce;
        backLeft.brakeTorque = currentBrakeForce;

        // Remove motor torque to stop acceleration
        frontRight.motorTorque = 0f;
        frontLeft.motorTorque = 0f;
    }

    protected void ResetBrakesAndAcceleration()
    {
        currentBrakeForce = 0f;
        currentAcceleration = 0f;

        // Release brakes
        frontRight.brakeTorque = 0f;
        backRight.brakeTorque = 0f;
        frontLeft.brakeTorque = 0f;
        backLeft.brakeTorque = 0f;
    }

    protected void AlignVelocityWithForward()
    {
        // Ensure the car's velocity is always aligned with its forward direction
        rb.velocity = transform.forward * rb.velocity.magnitude;
    }
}
