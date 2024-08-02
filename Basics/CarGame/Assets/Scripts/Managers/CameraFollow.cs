using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign this in the inspector to the player
    public RoadManager roadManager; // Assign this in the inspector to the RoadManager
    public Vector3 offset = new Vector3(0, 2, -10); // The offset from the road center where the camera should be positioned
    public float followSpeed = 5f; // The speed at which the camera follows the player

    private Vector3 roadCenter;

    void Start()
    {
        // Set the initial position of the camera
        SetCameraPosition();
    }

    void Update()
    {
        // Update the road center and camera position
        UpdateRoadCenter();
        FollowPlayer();
    }

    void FollowPlayer()
    {
        // Calculate the desired position of the camera
        Vector3 desiredPosition = player.position + offset;

        // Smoothly interpolate the camera's position towards the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }

    void SetCameraPosition()
    {
        // Initialize camera position
        UpdateRoadCenter();
        transform.position = roadCenter + offset;
    }

    void UpdateRoadCenter()
    {
        // Calculate the average position of the road segments to find the center
        Vector3 totalRoadPosition = Vector3.zero;
        int roadCount = roadManager.roadSegments.Count;

        if (roadCount > 0)
        {
            foreach (GameObject road in roadManager.roadSegments)
            {
                totalRoadPosition += road.transform.position;
            }

            roadCenter = totalRoadPosition / roadCount;
        }
    }
}