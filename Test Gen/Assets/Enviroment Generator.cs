using UnityEngine;
using UnityEngine.U2D;

[ExecuteAlways] // Ensures updates in the editor
public class EnvironmentGenerator : MonoBehaviour
{
    [Range(10, 200)] public int levelLength = 50;
    [Range(1f, 10f)] public float xMultiplier = 2f;
    [Range(1f, 10f)] public float yMultiplier = 2f;
    [Range(0f, 1f)] public float curveSmoothness = 0.5f;
    [Range(0.01f, 1f)] public float noiseStep = 0.1f;
    public float bottom = 10f;

    private Vector3 _lastPos;
    private SpriteShapeController _spriteShapeController;

    private void OnValidate()
    {
        // Get the SpriteShapeController if not already assigned
        if (_spriteShapeController == null)
        {
            _spriteShapeController = GetComponent<SpriteShapeController>();
        }

        // Clear existing spline points
        var spline = _spriteShapeController.spline;
        spline.Clear();

        // Generate new points for the terrain
        for (int i = 0; i < levelLength; i++)
        {
            // Calculate Perlin-based position
            _lastPos = transform.position + new Vector3(
                i * xMultiplier,
                Mathf.PerlinNoise(0, i * noiseStep) * yMultiplier,
                0
            );

            // Insert points into the spline
            spline.InsertPointAt(i, _lastPos);
            spline.SetTangentMode(i, ShapeTangentMode.Continuous);

            // Smooth tangents for natural curves
            spline.SetLeftTangent(i, Vector3.left * xMultiplier * curveSmoothness);
            spline.SetRightTangent(i, Vector3.right * xMultiplier * curveSmoothness);
        }

        // Add closing bottom points for the terrain
        spline.InsertPointAt(levelLength, new Vector3(_lastPos.x, transform.position.y - bottom, 0));
        spline.InsertPointAt(levelLength + 1, new Vector3(transform.position.x, transform.position.y - bottom, 0));

        // The SpriteShape will automatically update when points are modified.
    }
}
