using UnityEngine;
using UnityEngine.U2D;

public class RandomTerrainGenerator : MonoBehaviour
{
    public SpriteShapeController shapeController;
    public int numberOfPoints = 20;  // Number of points in the terrain
    public float heightVariance = 5f; // Maximum height difference
    public float widthVariance = 2f;  // Width variation for each point

    void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        var spline = shapeController.spline;
        spline.Clear();  // Clear any existing points

        // Add the starting point
        spline.InsertPointAt(0, new Vector3(0, 0, 0));

        // Generate the terrain points
        for (int i = 1; i < numberOfPoints; i++)
        {
            float x = i * widthVariance;
            float y = Mathf.PerlinNoise(x * 0.1f, 0) * heightVariance; // Use Perlin noise for smooth randomness
            spline.InsertPointAt(i, new Vector3(x, y, 0));
        }

        // Add the end point
        spline.InsertPointAt(numberOfPoints, new Vector3(numberOfPoints * widthVariance, 0, 0));
    }
}
