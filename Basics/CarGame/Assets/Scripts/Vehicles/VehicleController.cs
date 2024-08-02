using UnityEngine;

public class CarController : MonoBehaviour
{
    public float forwardSpeed = 10f;  // Speed for moving forward and backward
    public float shiftSpeed = 10f;   // Speed for shifting left and right
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Lock the rotation in the Rigidbody component
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        // Get input values
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Create movement vector
        Vector3 movement = new Vector3(horizontal * shiftSpeed, 0f, vertical * forwardSpeed);

        // Apply force to the Rigidbody
        rb.AddForce(movement);
    }

    // void FixedUpdate()
    // {
    //     // Get input values
    //     float moveForward = 0f;
    //     float shiftSideways = 0f;

    //     // Move forward if W is pressed
    //     if (Input.GetKey(KeyCode.W))
    //     {
    //         moveForward = speed;
    //     }

    //     // Move backward if S is pressed
    //     if (Input.GetKey(KeyCode.S))
    //     {
    //         moveForward = -speed;
    //     }

    //     // Shift left if A is pressed
    //     if (Input.GetKey(KeyCode.A))
    //     {
    //         shiftSideways = -shiftSpeed;
    //     }

    //     // Shift right if D is pressed
    //     if (Input.GetKey(KeyCode.D))
    //     {
    //         shiftSideways = shiftSpeed;
    //     }

    //     // Set velocity
    //     Vector3 movement = new Vector3(shiftSideways, f, moveForward);
    //     rb.velocity = movement;
    // }
}