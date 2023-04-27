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
    private float boundaryXMax;
    private float boundaryXMin;
    private float boundaryYMax;
    private float boundaryYMin;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        float xDiff = (boundingBoxTransform.localScale.x - gameObject.transform.localScale.x) / 2;
        float yDiff = (boundingBoxTransform.localScale.y - gameObject.transform.localScale.y) / 2;

        float boundingBoxPosX = boundingBoxTransform.position.x;
        float boundingBoxPosY = boundingBoxTransform.position.y;

        // Boundary X and Y in world positions
        boundaryXMax = boundingBoxPosX + xDiff;
        boundaryXMin = boundingBoxPosX - xDiff;
        boundaryYMax = boundingBoxPosY + yDiff;
        boundaryYMin = boundingBoxPosY - yDiff;

        jumpableGround = LayerMask.GetMask("SolidEnvironment");
    }

    // FIXEDUPDATE
    [SerializeField] private float moveSpeed = 3f;
    private float x;
    private float y;

    private void FixedUpdate()
    {
        //Get current x and y in world position
         x = gameObject.transform.position.x;
         y = gameObject.transform.position.y;

        //Move x and y according to input
        x += moveInput.x * moveSpeed * Time.deltaTime;
        y += moveInput.y * moveSpeed * Time.deltaTime;

        //Clamp according to world position boundary
        x = Mathf.Clamp(x, boundaryXMin, boundaryXMax);
        y = Mathf.Clamp(y, boundaryYMin, boundaryYMax);

        //Finalize move
        gameObject.transform.position = new Vector2(x, y);
    }

    private Vector2 moveInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private Transform pTransform; //player collider for cast
    private RaycastHit2D castHit;
    private bool isPlayerGroundedOnMe = false;
    private LayerMask jumpableGround;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            pTransform = col.gameObject.GetComponent<Transform>();
            castHit = Physics2D.Raycast(pTransform.position, Vector2.down, 0.7f, jumpableGround); //Shoot a ray down from player position

            if (!castHit) // if nothing is hit (i.e. if castHit = false)
            {
                isPlayerGroundedOnMe = false;
            }
            else
            {
                isPlayerGroundedOnMe = castHit.collider.gameObject == gameObject;
            }
        }
        else
        {
            return;
        }

        if (isPlayerGroundedOnMe)
        {
            col.gameObject.transform.SetParent(gameObject.transform, true);
        }
    }
    
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isPlayerGroundedOnMe = false;
            col.gameObject.transform.parent = null;
        }
    }

}