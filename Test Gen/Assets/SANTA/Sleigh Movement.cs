using UnityEngine;

public class ReindeerController : MonoBehaviour
{
    public Rigidbody2D reindeerRigidbody;  // Rigidbody of the reindeer
    public Rigidbody2D sleighRigidbody;   // Rigidbody of the sleigh
    public float moveForce = 500f;        // Force to move the reindeer
    public float jumpForce = 10f;         // Force for reindeer jumping

    // Lift-off variables
    public float liftSpeedThreshold = 10f; // Speed needed for lift-off
    public float liftForce = 2f;          // Gradual upward force during flight
    public float maxLiftForce = 15f;      // Maximum upward force to cap the lift
    public float manualLiftForce = 5f;    // Upward force when pressing the Up Arrow key

    // Rotation variables
    public float rotationSpeed = 100f;    // Speed at which the sleigh rotates in the air
    public float maxRotationAngle = 45f;  // Maximum tilt angle for the sleigh

    void Update()
    {
        // Move forward
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            reindeerRigidbody.AddForce(Vector2.right * moveForce * Time.deltaTime);
        }

        // Move backward
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            reindeerRigidbody.AddForce(Vector2.left * moveForce * Time.deltaTime);
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            reindeerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Manual lift when Up Arrow key is pressed
        if (Input.GetKey(KeyCode.UpArrow))
        {
            ApplyLift(manualLiftForce);
        }

        // Handle rotation input for sleigh
        HandleRotation();
    }

    void FixedUpdate()
    {
        // Gradual lift-off based on speed
        HandleLiftOff();
    }

    void HandleLiftOff()
    {
        // Calculate sleigh's speed
        float sleighSpeed = Mathf.Abs(sleighRigidbody.velocity.x);

        // Apply gradual lift-off if speed exceeds the threshold
        if (sleighSpeed > liftSpeedThreshold)
        {
            // Proportional lift based on speed
            float proportionalLift = Mathf.Clamp((sleighSpeed - liftSpeedThreshold) * liftForce, 0, maxLiftForce);
            ApplyLift(proportionalLift);
        }
    }

    void ApplyLift(float liftAmount)
    {
        // Apply the upward force to the sleigh
        sleighRigidbody.AddForce(Vector2.up * liftAmount, ForceMode2D.Force);

        // Optionally, limit vertical velocity for smoother flight
        if (sleighRigidbody.velocity.y > maxLiftForce)
        {
            sleighRigidbody.velocity = new Vector2(sleighRigidbody.velocity.x, maxLiftForce);
        }
    }

    void HandleRotation()
    {
        // Get the current angular velocity
        float currentRotation = sleighRigidbody.rotation;

        // Apply left or right rotation based on input (rotate clockwise or counterclockwise)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // Apply torque to rotate clockwise
            sleighRigidbody.AddTorque(-rotationSpeed * Time.deltaTime); // Negative for clockwise rotation
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            // Apply torque to rotate counterclockwise
            sleighRigidbody.AddTorque(rotationSpeed * Time.deltaTime); // Positive for counterclockwise rotation
        }

        // Limit the sleigh's rotation to a maximum angle
        if (Mathf.Abs(sleighRigidbody.rotation) > maxRotationAngle)
        {
            // Clamp the sleigh's rotation to the maximum angle
            sleighRigidbody.rotation = Mathf.Sign(sleighRigidbody.rotation) * maxRotationAngle;
        }
    }
}
