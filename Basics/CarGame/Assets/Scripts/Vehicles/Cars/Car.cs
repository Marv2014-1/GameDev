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
    public float decelerationFactor = 0.01f; // Further adjust this factor for more gradual deceleration

    // Wheels
    [SerializeField] protected WheelCollider frontRight;
    [SerializeField] protected WheelCollider backRight;
    [SerializeField] protected WheelCollider frontLeft;
    [SerializeField] protected WheelCollider backLeft;

    // Sound Effects
    public AudioSource idleSource;
    public AudioSource accelerationSource;
    public AudioSource decelerationSource;

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

        // Start idle sound
        if (idleSource != null)
        {
            idleSource.loop = true;
            idleSource.Play();
        }
    }

    private void Update()
    {
        // Get user input
        moveInput = Input.GetAxis("Vertical");
        veerInput = Input.GetAxis("Horizontal");

        // Update sound effects based on velocity
        UpdateSoundEffects();
    }

    // FixedUpdate is called once per physics update
    private void FixedUpdate()
    {
        HandleMovement();
        HandleVeering();
        ApplyDeceleration(); // Apply deceleration when no acceleration input
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
            ApplyWheelFriction(); // Adjust wheel friction when not accelerating or braking
        }
    }

    protected void HandleVeering()
    {
        // Apply veering force directly to the rigidbody for simple lane merging
        if (moveInput != 0f)
        {
            Vector3 veerDirection = transform.right * veerInput * veerForce;
            rb.AddForce(veerDirection, ForceMode.Acceleration);
        }
        else
        {
            // Apply veering without affecting forward velocity
            Vector3 veerDirection = transform.right * veerInput * veerForce;
            rb.AddForce(veerDirection - transform.forward * Vector3.Dot(rb.velocity, transform.forward), ForceMode.Acceleration);
        }
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

    protected void ApplyDeceleration()
    {
        if (moveInput == 0f)
        {
            // Apply deceleration proportional to the car's mass
            float deceleration = (decelerationFactor / rb.mass) * Time.fixedDeltaTime;
            rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, deceleration);
        }
    }

    protected void ApplyWheelFriction()
    {
        WheelFrictionCurve forwardFriction = new WheelFrictionCurve();
        WheelFrictionCurve sidewaysFriction = new WheelFrictionCurve();

        forwardFriction.extremumSlip = 1f;
        forwardFriction.extremumValue = 1f;
        forwardFriction.asymptoteSlip = 1f;
        forwardFriction.asymptoteValue = 1f;
        forwardFriction.stiffness = 0.5f; // Adjust the stiffness value to control friction

        sidewaysFriction.extremumSlip = 1f;
        sidewaysFriction.extremumValue = 1f;
        sidewaysFriction.asymptoteSlip = 1f;
        sidewaysFriction.asymptoteValue = 1f;
        sidewaysFriction.stiffness = 0.5f; // Adjust the stiffness value to control friction

        frontRight.forwardFriction = forwardFriction;
        frontRight.sidewaysFriction = sidewaysFriction;
        backRight.forwardFriction = forwardFriction;
        backRight.sidewaysFriction = sidewaysFriction;
        frontLeft.forwardFriction = forwardFriction;
        frontLeft.sidewaysFriction = sidewaysFriction;
        backLeft.forwardFriction = forwardFriction;
        backLeft.sidewaysFriction = sidewaysFriction;
    }

    protected void AlignVelocityWithForward()
    {
        // Ensure the car's velocity is always aligned with its forward direction
        rb.velocity = transform.forward * rb.velocity.magnitude;
    }

    protected void UpdateSoundEffects()
    {
        float speed = rb.velocity.magnitude;
        float normalizedSpeed = speed / maxSpeed;

        if (moveInput > 0f)
        {
            if (!accelerationSource.isPlaying)
            {
                accelerationSource.loop = true;
                accelerationSource.Play();
                decelerationSource.Stop();
                idleSource.Stop();
            }
            accelerationSource.pitch = Mathf.Lerp(2f, 3f, normalizedSpeed);
            accelerationSource.volume = Mathf.Lerp(0.5f, 1f, normalizedSpeed);
        }
        else if (moveInput < 0f)
        {
            if (!decelerationSource.isPlaying)
            {
                decelerationSource.loop = true;
                decelerationSource.Play();
                accelerationSource.Stop();
                idleSource.Stop();
            }
            decelerationSource.pitch = Mathf.Lerp(2f, 3f, normalizedSpeed);
            decelerationSource.volume = Mathf.Lerp(0.5f, 1f, normalizedSpeed);
        }
        else
        {
            if (!idleSource.isPlaying)
            {
                idleSource.loop = true;
                idleSource.Play();
                accelerationSource.Stop();
                decelerationSource.Stop();
            }
            idleSource.pitch = Mathf.Lerp(2f, 3f, normalizedSpeed);
            idleSource.volume = Mathf.Lerp(0.5f, 1f, normalizedSpeed);
        }
    }
}
