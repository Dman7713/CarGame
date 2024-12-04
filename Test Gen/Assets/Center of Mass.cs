using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomCenterOfMass : MonoBehaviour
{
    [SerializeField] private Vector2 centerOfMassOffset;

    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = centerOfMassOffset;
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying == false)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector2 worldCenterOfMass = (Vector2)transform.position + centerOfMassOffset;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(worldCenterOfMass, 0.1f);
        }
    }
}
