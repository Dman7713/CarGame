using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CaveGenerator : MonoBehaviour
{
    [Header("Cave Generation Settings")]
    public int width = 50;  // Width of the cave
    public int height = 10; // Height of the cave
    public float smoothness = 0.5f; // Smoothness of the cave edges
    public float verticalShift = 0.0f; // Vertical offset for the cave shape

    [Header("SpriteShape Settings")]
    public SpriteShapeController spriteShapeController;
    public SpriteShape shape; // Reference to a SpriteShape asset (not Sprite)

    private Vector2[] points;

    void Start()
    {
        GenerateCave();
    }

    void GenerateCave()
    {
        points = new Vector2[width];

        // Generate cave shape points using Perlin noise or any random logic
        for (int i = 0; i < width; i++)
        {
            float x = i;
            float y = Mathf.PerlinNoise(i * smoothness, 0f) * height + verticalShift;
            points[i] = new Vector2(x, y);
        }

        // Set up the SpriteShapeController with the generated points
        CreateSpriteShape();
    }

    void CreateSpriteShape()
    {
        // Check if SpriteShapeController is assigned
        if (spriteShapeController == null)
        {
            Debug.LogError("SpriteShapeController not assigned!");
            return;
        }

        // Create the path points for the cave (convert Vector2 to Vector3)
        List<Vector3> pathPoints = new List<Vector3>();

        // Set up the top and bottom cave paths
        for (int i = 0; i < width; i++)
        {
            pathPoints.Add(new Vector3(points[i].x, points[i].y, 0f));  // Convert Vector2 to Vector3
        }

        // Clear existing points from the spline
        spriteShapeController.spline.Clear();

        // Add points to the spline
        for (int i = 0; i < pathPoints.Count; i++)
        {
            spriteShapeController.spline.InsertPointAt(i, pathPoints[i]);  // Insert each point into the spline
        }

        // Set the custom SpriteShape for the cave
        spriteShapeController.spriteShape = shape;  // Use a SpriteShape asset here
    }
}
