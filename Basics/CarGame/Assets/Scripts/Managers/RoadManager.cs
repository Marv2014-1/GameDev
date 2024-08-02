using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public GameObject roadPrefab;
    public Transform car;
    public Transform roadParent;
    public Transform grassParent;
    public Transform buildingParent; // Parent for building segments
    public GameObject grassPrefab; // Add this field for the grass prefab
    public float spawnDistance = 100f;
    public int initialRoadSegments = 1;
    public float despawnDistance = 200f;

    public GameObject[] buildingPrefabs; // Array of building prefabs
    public float buildingDistanceFromRoad = 10f; // Distance of buildings from the road

    private Vector3 nextSpawnPoint;
    public List<GameObject> roadSegments = new List<GameObject>();
    private List<GameObject> buildingSegments = new List<GameObject>();
    private List<GameObject> grassFields = new List<GameObject>(); // List to track grass fields

    void Start()
    {
        nextSpawnPoint = Vector3.zero;
        for (int i = 0; i < initialRoadSegments; i++)
        {
            SpawnRoad();
        }

        // Position the car on the first road segment
        car.position = new Vector3(0, 1, 0); // Adjust the Y position as needed based on your road height
    }

    void Update()
    {
        if (Vector3.Distance(car.position, nextSpawnPoint) < spawnDistance)
        {
            SpawnRoad();
        }

        DespawnRoadsAndGrassFields(); // Updated method to handle both roads and grass fields
    }

    void SpawnRoad()
    {
        GameObject road = Instantiate(roadPrefab, nextSpawnPoint, Quaternion.identity);
        road.transform.SetParent(roadParent);
        roadSegments.Add(road);

        // Calculate the next spawn point based on the length of the road prefab
        float roadLength = roadPrefab.GetComponent<Renderer>().bounds.size.z;
        nextSpawnPoint += new Vector3(0, 0, roadLength);

        // Spawn grass field under the road
        SpawnGrassField(road.transform.position, roadLength);

        // Spawn buildings on both sides of the road
        SpawnBuildings(road.transform.position, roadLength);
    }

    void SpawnGrassField(Vector3 roadPosition, float roadLength)
    {
        // Instantiate the grass field prefab directly under the road
        GameObject grassField = Instantiate(grassPrefab, new Vector3(roadPosition.x, 0, roadPosition.z), Quaternion.Euler(0, 90, 90));

        // Adjust size and position based on road length and width
        Renderer grassRenderer = grassField.GetComponent<Renderer>();

        grassField.transform.localPosition = new Vector3(0, roadPosition.y -1 , roadPosition.z);
        grassField.transform.localScale = new Vector3(1, 10, 100);

        // Set parent to organize hierarchy
        grassField.transform.SetParent(grassParent);

        // Add to the list of grass fields for potential future despawning
        grassFields.Add(grassField);
    }

    void SpawnBuildings(Vector3 roadPosition, float roadLength)
    {
        // Randomly select building prefabs for the left and right sides
        GameObject leftBuildingPrefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
        GameObject rightBuildingPrefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];

        // Calculate positions for the buildings with additional spacing
        Vector3 leftBuildingPosition = roadPosition + new Vector3(-buildingDistanceFromRoad, 0, roadLength / 2);
        Vector3 rightBuildingPosition = roadPosition + new Vector3(buildingDistanceFromRoad, 0, roadLength / 2);

        // Calculate rotations for the buildings to face the road
        Quaternion leftBuildingRotation = Quaternion.Euler(0, 90, 0); // Facing right
        Quaternion rightBuildingRotation = Quaternion.Euler(0, -90, 0); // Facing left

        // Instantiate buildings
        GameObject leftBuilding = Instantiate(leftBuildingPrefab, leftBuildingPosition, leftBuildingRotation);
        GameObject rightBuilding = Instantiate(rightBuildingPrefab, rightBuildingPosition, rightBuildingRotation);

        // Set parent to organize hierarchy
        leftBuilding.transform.SetParent(buildingParent);
        rightBuilding.transform.SetParent(buildingParent);

        // Add to the list of building segments for potential future despawning
        buildingSegments.Add(leftBuilding);
        buildingSegments.Add(rightBuilding);
    }

    void DespawnRoadsAndGrassFields()
    {
        // Despawn roads
        if (roadSegments.Count > 0 && Vector3.Distance(car.position, roadSegments[0].transform.position) > despawnDistance)
        {
            Destroy(roadSegments[0]);
            roadSegments.RemoveAt(0);
        }

        // Despawn buildings
        if (buildingSegments.Count > 0 && Vector3.Distance(car.position, buildingSegments[0].transform.position) > despawnDistance)
        {
            Destroy(buildingSegments[0]);
            buildingSegments.RemoveAt(0);
        }

        // Despawn grass fields
        if (grassFields.Count > 0 && Vector3.Distance(car.position, grassFields[0].transform.position) > despawnDistance)
        {
            Destroy(grassFields[0]);
            grassFields.RemoveAt(0);
        }
    }
}