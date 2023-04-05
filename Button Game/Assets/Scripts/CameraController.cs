using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float smoothTime = 0.5f;
    public float cameraDistance = 5.0f;

    private Vector3 velocity = Vector3.zero;

    public ButtonGameControls loadedControls;

    private InputAction cameraHorizontalMovement;

    private Vector3 targetPosition;
    private void Awake()
    {
        loadedControls = new ButtonGameControls();
    }

    private void OnEnable()
    {
        cameraHorizontalMovement = loadedControls.Player.MoveHorizontally;
        cameraHorizontalMovement.Enable();
    }

    private void OnDisable()
    {
        cameraHorizontalMovement.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // Set the camera's target horizontal position in front of the player, direction based on the player's inputs
        targetPosition.x = cameraDistance * cameraHorizontalMovement.ReadValue<float>();

        // Create a dampened vector; we'll only be using the x value of the dampened vector
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref velocity, smoothTime);

        transform.localPosition = new Vector3(smoothedPosition.x, 1, -10);
    }
}
