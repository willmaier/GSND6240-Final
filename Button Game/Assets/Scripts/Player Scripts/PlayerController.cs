using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{

    // AWAKE SETTINGS
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // ANIMATOR CHECKS
    [SerializeField] public bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            if (_isFacingRight != value) // If value is new (i.e player has flipped their controls)...
            {
                //... then also flip transform horizontally.
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    [SerializeField] private bool _isGroundMoving = false;

    public bool IsGroundMoving 
    { 
        get
        {
            return _isGroundMoving;
        }
        private set
        {
            _isGroundMoving = value;
            anim.SetBool("isMoving", value);
        }
    }

    [SerializeField] private bool _isGrounded = true;
    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        private set
        {
            _isGrounded = value;
            anim.SetBool("isGrounded", value);
        }
    }

    // FIXEDUPDATE

    [SerializeField] private LayerMask jumpableGround;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float jetpackMoveSpeed = 6f;

    private void FixedUpdate()
    {
        // Check if grounded
        IsGrounded = Physics2D.CapsuleCast(coll.bounds.center, coll.bounds.size, CapsuleDirection2D.Vertical, 0f, Vector2.down, 0.05f, jumpableGround);

        if (!chargedModeButton)
        {
            // Move horizontally based on horizontalMoveInput set by OnMove
            rb.velocity = new Vector2(horizontalMoveInput * moveSpeed, rb.velocity.y);

            if (IsGrounded)
            {
                canDoubleJump = true;
            }

        }
        else
        {
            //Move using jetpack move
            rb.velocity = new Vector2(moveInputVector2.x * jetpackMoveSpeed, moveInputVector2.y * jetpackMoveSpeed);
        }
    }


    private void SetFacingDirection(float horizontalMoveInput)
    {
        if (horizontalMoveInput > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (horizontalMoveInput < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    //-----
    //
    // GROUND MOVEMENT WHEN NOT PRESSING CHARGED MODE BUTTON
    //
    //-----

    float horizontalMoveInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!chargedModeButton)
        {
            horizontalMoveInput = context.ReadValue<float>();

            IsGroundMoving = horizontalMoveInput != 0; // Set is ground moving for animator

            SetFacingDirection(horizontalMoveInput); // Set facing direction only when movement is inputted
        }
    }

//double jump
    [SerializeField] private bool canDoubleJump = true;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!chargedModeButton)
        {
            if (context.started && IsGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            // else if can double jump
            // double jump
            else if (context.started && canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false;
            }
        }
    }


    //-----
    //
    // JETPACK MOVEMENT WHEN PRESSING CHARGED MODE BUTTON
    //
    //-----

    Vector2 moveInputVector2;


    public void OnMoveVector2(InputAction.CallbackContext context)
    {
        if (chargedModeButton)
        {
            moveInputVector2 = context.ReadValue<Vector2>();

            horizontalMoveInput = moveInputVector2.x;
            SetFacingDirection(horizontalMoveInput); // Set facing direction only when movement is inputted

        }
    }



    //-----
    //
    // MISCELLANEOUS INPUT CHECKS
    //
    //-----

    [SerializeField] private bool _touchingInteractible = false;

    public bool touchingInteractible
    {
        get
        {
            return _touchingInteractible;
        }
        private set
        {
            _touchingInteractible = value;
            // do something here to show interact star
        }
    }

    [SerializeField] public GameObject interactingObject = null;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //known issues: glitchy interactions if touchin 2 interactible objects at one time
        if (collision.gameObject.GetComponent<InteractionController>()) // Check if collision object has an interaction controller
        {
            touchingInteractible = true;
            interactingObject = collision.gameObject;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        //known issues: glitchy interactions if touchin 2 interactible objects at one time
        touchingInteractible = false;
        interactingObject = null;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (touchingInteractible && IsGrounded && !chargedModeButton)
        {
            InteractionController con = interactingObject.GetComponent<InteractionController>();
            con.Interact();
        }
    }

    [SerializeField] public bool chargedModeButton;

    public void OnChargedMode(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            chargedModeButton = true;
        }
        if (context.canceled)
        {
            chargedModeButton = false;
            moveInputVector2 = Vector2.zero;
        }
    }
}
