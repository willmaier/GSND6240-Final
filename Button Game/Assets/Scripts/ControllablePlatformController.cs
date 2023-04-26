using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ControllablePlatformController : MonoBehaviour
{
    // AWAKE SETTINGS
    private Rigidbody2D rb;
    
    [SerializeField] private Transform boundingBoxTransform;
    private float boundaryX;
    private float boundaryY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        boundaryX = (boundingBoxTransform.lossyScale.x - this.transform.lossyScale.x) / 2;
        boundaryY = (boundingBoxTransform.lossyScale.y - this.transform.lossyScale.y) / 2;
    }

    // FIXEDUPDATE
    [SerializeField] private float moveSpeed = 3f;
    private void FixedUpdate()
    {
        //Move
        rb.velocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);

        //Clamp position inside bounding box
        float x = Mathf.Clamp(this.transform.localPosition.x, -boundaryX, boundaryX);
        float y = Mathf.Clamp(this.transform.localPosition.y, -boundaryY, boundaryY);
        this.transform.localPosition = new Vector2(x, y);
    }

    private Vector2 moveInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}