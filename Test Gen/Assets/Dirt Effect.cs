using UnityEngine;

public class PlayerMovementWithParticles : MonoBehaviour
{
    [Header("Particle Effect Settings")]
    public ParticleSystem moveParticles; // Drag your particle system here in the Inspector

    [Header("Ground Check Settings")]
    public LayerMask groundLayer; // Set the ground layer in the Inspector
    public Transform groundCheck; // A small empty GameObject at the bottom of the player
    public float groundCheckRadius = 0.2f; // Radius for ground detection

    private bool isGrounded = false;

    void Update()
    {
        // Check if the player is touching the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Trigger the particle effect when the movement key is pressed and the player is grounded
        if (isGrounded && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)))
        {
            PlayParticleEffect();
        }
    }

    private void PlayParticleEffect()
    {
        if (moveParticles != null)
        {
            // Restart the particle effect to ensure it plays every time
            moveParticles.Stop();
            moveParticles.Play();
        }
        else
        {
            Debug.LogWarning("MoveParticles is not assigned in the Inspector!");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the ground check radius in the Scene view
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
