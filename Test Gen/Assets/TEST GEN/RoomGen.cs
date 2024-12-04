using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs; // Array of room prefabs to use
    public GameObject doorPrefab;    // Reference to a door prefab
    public int totalRooms = 100;     // Total number of rooms to generate

    private List<Room> rooms;

    void Start()
    {
        rooms = new List<Room>();
        GenerateLevel();
    }

    void GenerateLevel()
    {
        // Create the first room at the starting position
        Room firstRoom = GenerateRoom(Vector2Int.zero);
        rooms.Add(firstRoom);

        // Generate subsequent rooms, each connected to the previous one
        Room previousRoom = firstRoom;
        for (int i = 1; i < totalRooms; i++)
        {
            // Generate a new room at a position offset from the previous one
            Vector2Int roomPosition = new Vector2Int(previousRoom.Position.x + previousRoom.Size.x, previousRoom.Position.y);
            Room newRoom = GenerateRoom(roomPosition);
            rooms.Add(newRoom);

            // Connect the previous room to the new room
            ConnectRooms(previousRoom, newRoom);

            // Update the previous room to be the newly created room
            previousRoom = newRoom;
        }

        // Spawn all the rooms and doors in the scene
        SpawnLevel();
    }

    Room GenerateRoom(Vector2Int position)
    {
        // Randomly choose a room prefab from the available ones
        GameObject chosenPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];

        // Calculate the size based on the prefab's bounds
        Vector2Int roomSize = new Vector2Int(Mathf.FloorToInt(chosenPrefab.GetComponent<SpriteRenderer>().bounds.size.x), Mathf.FloorToInt(chosenPrefab.GetComponent<SpriteRenderer>().bounds.size.y));

        return new Room(position, roomSize, chosenPrefab);
    }

    void ConnectRooms(Room roomA, Room roomB)
    {
        // Create doors to connect two rooms
        Vector2Int doorPositionA = new Vector2Int(roomA.Position.x + roomA.Size.x, roomA.Position.y + roomA.Size.y / 2);
        Vector2Int doorPositionB = new Vector2Int(roomB.Position.x, roomB.Position.y + roomB.Size.y / 2);

        roomA.Doors.Add(new Door(doorPositionA, Direction.Right)); // Right door for room A
        roomB.Doors.Add(new Door(doorPositionB, Direction.Left));  // Left door for room B
    }

    void SpawnLevel()
    {
        foreach (Room room in rooms)
        {
            // Instantiate the chosen room prefab at its position
            Instantiate(room.RoomPrefab, new Vector3(room.Position.x, room.Position.y, 0), Quaternion.identity);

            // Instantiate the doors at the correct positions
            foreach (Door door in room.Doors)
            {
                Vector3 doorPosition = new Vector3(door.Position.x, door.Position.y, 0);
                Instantiate(doorPrefab, doorPosition, Quaternion.identity);
            }
        }
    }
}

public class Room
{
    public Vector2Int Position { get; set; }
    public Vector2Int Size { get; set; }
    public GameObject RoomPrefab { get; set; }
    public List<Door> Doors { get; set; }

    public Room(Vector2Int position, Vector2Int size, GameObject roomPrefab)
    {
        Position = position;
        Size = size;
        RoomPrefab = roomPrefab;
        Doors = new List<Door>();
    }
}

public class Door
{
    public Vector2Int Position { get; set; }
    public Direction DoorDirection { get; set; }

    public Door(Vector2Int position, Direction direction)
    {
        Position = position;
        DoorDirection = direction;
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
