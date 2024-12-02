using UnityEngine;

public class CarSwitcher : MonoBehaviour
{
    public GameObject car1Prefab;
    public GameObject car2Prefab;
    public GameObject car3Prefab;
    public Transform spawnPoint; // This will be the point used to follow the car's center
    public CameraFollow cameraFollowScript; // CameraFollow script to handle camera tracking

    private GameObject currentCar;
    private Rigidbody2D currentCarRigidbody;

    void Start()
    {
        // Instantiate the first car at the spawn point
        currentCar = Instantiate(car1Prefab, spawnPoint.position, spawnPoint.rotation);
        currentCarRigidbody = currentCar.GetComponent<Rigidbody2D>();

        // Position the spawn point at the center of the car
        spawnPoint.position = currentCar.transform.position;

        // Set the camera to follow the current car
        cameraFollowScript.target = currentCar.transform;
    }

    void Update()
    {
        // Continuously update the spawn point to match the car's center
        if (currentCar != null)
        {
            spawnPoint.position = currentCar.transform.position;  // Update spawn point position
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchCar(car1Prefab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchCar(car2Prefab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchCar(car3Prefab);
        }
    }

    void SwitchCar(GameObject newCarPrefab)
    {
        if (currentCar != null)
        {
            // Save the current velocity of the old car
            Vector2 oldVelocity = currentCarRigidbody.velocity;

            // Destroy the current car
            Destroy(currentCar);

            // Spawn the new car at the spawn point's position and rotation
            currentCar = Instantiate(newCarPrefab, spawnPoint.position, spawnPoint.rotation);

            // Get the Rigidbody2D of the new car
            currentCarRigidbody = currentCar.GetComponent<Rigidbody2D>();

            // Apply the old velocity to the new car
            currentCarRigidbody.velocity = oldVelocity;

            // Set the camera to follow the new car
            cameraFollowScript.target = currentCar.transform;

            // Update the spawn point to the new car's center position
            spawnPoint.position = currentCar.transform.position;
        }
    }
}
