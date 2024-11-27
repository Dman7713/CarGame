using UnityEngine;
using UnityEngine.U2D;

public class TerrainGenerator : MonoBehaviour
{
    public SpriteShapeController spriteShapeController;
    public EdgeCollider2D edgeCollider; // Reference to the Edge Collider 2D
    public int terrainWidth = 100;
    public float heightMultiplier = 3f;
    public float widthMultiplier = 0.1f;
    public float smoothness = 0.1f;
    public float seed = 0f;

    private void Awake()
    {
        // Ensure spriteShapeController and edgeCollider are assigned automatically if not set in the Inspector
        if (!spriteShapeController)
            spriteShapeController = GetComponent<SpriteShapeController>();

        if (!edgeCollider)
            edgeCollider = GetComponent<EdgeCollider2D>();
    }

    private void Start()
    {
        GenerateTerrain();
        UpdateEdgeCollider();
    }

    void GenerateTerrain()
    {
        var spline = spriteShapeController.spline;

        // Set random seed for terrain generation
        Random.InitState((int)seed);

        // Clear previous spline points
        spline.Clear();

        // Generate spline points using Perlin noise
        for (int i = 0; i <= terrainWidth; i++)
        {
            float x = i;
            float y = Mathf.PerlinNoise(x * widthMultiplier + seed, 0) * heightMultiplier;
            spline.InsertPointAt(i, new Vector3(x, y, 0));

            // Smooth the terrain if needed
            if (i > 1)
            {
                float smooth = Mathf.PerlinNoise(x * widthMultiplier + seed, 0) * smoothness;
                spline.SetPosition(i, new Vector3(x, y + smooth, 0));
            }

            // Ensure points are linear (no curves)
            spline.SetTangentMode(i, ShapeTangentMode.Linear);
        }

        spriteShapeController.BakeMesh();
    }

    void UpdateEdgeCollider()
    {
        var spline = spriteShapeController.spline;
        int pointCount = spline.GetPointCount();

        // Create a new array to hold edge collider points
        Vector2[] edgePoints = new Vector2[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            Vector3 splinePoint = spline.GetPosition(i);
            edgePoints[i] = new Vector2(splinePoint.x, splinePoint.y);
        }

        // Assign the points to the Edge Collider 2D
        edgeCollider.points = edgePoints;
    }
}
