using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // Array to hold different obstacle prefabs
    public Transform startPoint; // Start point for spawning obstacles
    public Transform endPoint; // End point for despawning obstacles
    public float spawnInterval = 1f; // Interval between spawns

    private float timeSinceLastSpawn = 0f;

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnObstacle();
            timeSinceLastSpawn = 0f;
        }
    }

    void SpawnObstacle()
    {
        // Choose a random obstacle prefab
        GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // Calculate random position to the left or right of the start point
        float randomX = Random.Range(-2.5f, 2.5f); // Adjust these values as needed
        Vector3 spawnPosition = new Vector3(startPoint.position.x + randomX, startPoint.position.y, startPoint.position.z);

        // Instantiate the obstacle
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }
}
