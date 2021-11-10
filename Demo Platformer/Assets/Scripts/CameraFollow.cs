using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offset;
    public float zPos;
    public float cameraSpeed;

    void FixedUpdate()
    {
        // Current and target position
        Vector3 targetPosition = new Vector3(objectToFollow.position.x, objectToFollow.position.y, zPos) + offset;

        // Transform to position
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.deltaTime);
    }
}
