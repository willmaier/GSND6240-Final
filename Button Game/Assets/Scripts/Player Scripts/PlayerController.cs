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
    [SerializeField] private bool _isFacingRight = true;
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

        if (!chargedModeButton || (verticalMoveInput == 0 && horizontalMoveInput == 0)) //If charged mode button isn't pressed, or if no input is made
        {
            // Move horizontally based on horizontalMoveInput set by OnMove
            rb.gravityScale = 1;
            rb.velocity = new Vector2(horizontalMoveInput * moveSpeed, rb.velocity.y);

            if (IsGrounded)
            {
                canDoubleJump = true;
            }

        }
        else //If charged mode is pressed, and that some input is made
        {
            //Move using jetpack move
            rb.gravityScale = 0;
            rb.velocity = new Vector2(horizontalMoveInput * jetpackMoveSpeed, verticalMoveInput * jetpackMoveSpeed);
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
    // HORIZONTAL MOVEMENT
    //
    //-----

    float horizontalMoveInput;

    public void OnMoveHorizontal(InputAction.CallbackContext context)
    {
        horizontalMoveInput = context.ReadValue<float>();
        if (!chargedModeButton)
        {
            IsGroundMoving = horizontalMoveInput != 0; // Set is ground moving for animator
        }
        else
        {
            IsGroundMoving = false;
        }
        SetFacingDirection(horizontalMoveInput); // Set facing direction only when movement is inputted
    }


    // JUMP MOVEMENT
    // 
    // DISABLED WHEN PRESSING CHARGED MODE BUTTON

    [SerializeField] private bool canDoubleJump = true;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!chargedModeButton) // Disable jump when pressing down charged mode
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
    // VERTICAL MOVEMENT WHEN PRESSING CHARGED MODE BUTTON
    //
    //-----

    float verticalMoveInput;


    public void OnMoveVertical(InputAction.CallbackContext context)
    {
            verticalMoveInput = context.ReadValue<float>();
    }

    //-----
    //
    // MISCELLANEOUS INPUT CHECKS
    //
    //-----

    [SerializeField] private bool _touchingInteractible = false;
    public bool TouchingInteractible
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

    public IInteractible interactingController;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //known issues: glitchy interactions if touchin 2 interactible objects at one time
        if (collision.gameObject.TryGetComponent<IInteractible>(out IInteractible inter)) // Check if collision object has an interaction controller
        {
            TouchingInteractible = true;
            interactingController = inter;
            // Debug Log: can interact with game object
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        //known issues: glitchy interactions if touchin 2 interactible objects at one time
        TouchingInteractible = false;
        interactingController = null;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (TouchingInteractible && IsGrounded && !chargedModeButton)
        {
            interactingController.OnInteract();
        }
    }

    [SerializeField] private bool chargedModeButton;

    public void OnChargedMode(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            chargedModeButton = true;
        }
        if (context.canceled)
        {
            chargedModeButton = false;
        }
    }
}
