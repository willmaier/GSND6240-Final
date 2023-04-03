using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Transform playerTransform;

    public float smoothTime = 0.3f;
    public float cameraDistance = 10.0f;

    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set the camera's target position in front of the player
        Vector3 targetPosition = playerTransform.position + playerTransform.forward * cameraDistance;
        targetPosition.y = playerTransform.position.y + 1; // Lock the camera's height at player's y vector + 1
        targetPosition.z = -10;

        // Smoothly move the camera to the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
