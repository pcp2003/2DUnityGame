using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    private Transform target;       // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Smoothing speed for the camera movement
    public Vector3 offset;         // Offset to keep the camera at a certain distance from the player

    void LateUpdate()
    {
        if (target != null)
        {
            // Desired position with offset
            Vector3 desiredPosition = target.position + offset;
            // Smoothly move the camera to that position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    public void SetTarget (GameObject player) {
        target = player.transform;
    }
}

