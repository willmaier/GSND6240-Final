using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float smoothTime = 0.5f;
    public float cameraDistance = 5.0f;

    private Vector3 velocity = Vector3.zero;

    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Set the camera's target horizontal position in front of the player, direction based on the player's inputs
        targetPosition.x = cameraDistance * Input.GetAxisRaw("Horizontal");

        // Create a dampened vector; we'll only be using the x value of the dampened vector
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref velocity, smoothTime);

        transform.localPosition = new Vector3(smoothedPosition.x, 1, -10);
    }
}
