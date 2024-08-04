using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign this in the inspector to the player
    public RoadManager roadManager; // Assign this in the inspector to the RoadManager
    public Vector3 offset = new Vector3(0, 2, -10); // The offset from the road center where the camera should be positioned
    public float followSpeed = 5f; // The speed at which the camera follows the player

    public float minZoomDistance = 2f; // Minimum zoom distance
    public float maxZoomDistance = 15f; // Maximum zoom distance
    public float zoomSpeed = 10f; // Speed at which the camera zooms

    private Vector3 roadCenter;
    private float currentZoom = 10f; // Initial zoom level

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

        // Handle camera zoom
        HandleZoom();
    }

    void FollowPlayer()
    {
        // Calculate the desired position of the camera
        Vector3 desiredPosition = player.position + offset.normalized * currentZoom;

        // Smoothly interpolate the camera's position towards the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }

    void SetCameraPosition()
    {
        // Initialize camera position
        UpdateRoadCenter();
        transform.position = roadCenter + offset.normalized * currentZoom;
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

    void HandleZoom()
    {
        // Get the scroll wheel input
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the current zoom level based on the scroll input
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoomDistance, maxZoomDistance);
    }
}
