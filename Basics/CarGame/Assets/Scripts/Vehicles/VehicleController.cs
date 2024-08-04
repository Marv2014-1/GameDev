using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 5f;
    public float shiftSpeed = 5f; // Speed at which the car shifts left and right
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Lock the rotation in the Rigidbody component
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        // Get input values
        float moveForward = 0f;
        float shiftSideways = 0f;

        // Move forward if W is pressed
        if (Input.GetKey(KeyCode.W))
        {
            moveForward = speed;
        }

        // Move backward if S is pressed
        if (Input.GetKey(KeyCode.S))
        {
            moveForward = -speed;
        }

        // Shift left if A is pressed
        if (Input.GetKey(KeyCode.A))
        {
            shiftSideways = -shiftSpeed;
        }

        // Shift right if D is pressed
        if (Input.GetKey(KeyCode.D))
        {
            shiftSideways = shiftSpeed;
        }

        // Set velocity
        Vector3 movement = Vector3.forward * moveForward;
        rb.AddForce(movement);
    }
}