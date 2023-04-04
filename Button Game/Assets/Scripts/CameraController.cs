using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Transform playerTransform;

    public float smoothTime = 0.5f;
    public float cameraDistance = 5.0f;

    private Vector3 velocity = Vector3.zero;

    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set the camera's target horizontal position in front of the player, direction based on the player's inputs
        targetPosition.x = playerTransform.position.x + (cameraDistance * Input.GetAxisRaw("Horizontal"));

        // Create a dampened vector; we'll only be using the x value of the dampened vector
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        transform.position = new Vector3(smoothedPosition.x, playerTransform.position.y + 1, -10);
    }
}
