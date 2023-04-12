using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    // private enum MovementState { idle, jumping, falling }

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

    [SerializeField] private bool _isMoving = false;

    public bool IsMoving 
    { 
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            anim.SetBool("isMoving", value);
        }
    }

    [SerializeField] private bool _isGrounded = true;

    [SerializeField] private bool _canInteract = false;

    public bool canInteract
    {
        get
        {
            return _canInteract;
        }
        private set
        {
            _canInteract = value;
            // do something here to show interact star
        }
    }

    [SerializeField] public GameObject interactingObject = null;

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

    float moveInput;
    bool jumpInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Move horizontally based on moveInput set by OnMove
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Check if grounded
        IsGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();

        IsMoving = moveInput != 0;

        SetFacingDirection(moveInput); // Set facing direction only when movement is inputted
    }

    private void SetFacingDirection(float moveInput)
    {
        if (moveInput > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        } else if (moveInput < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }
//double jump
    [SerializeField] private bool _canDoubleJump = true;

    public bool canDoubleJump
    {
        get
        {
            return _canDoubleJump;
        }
        private set
        {
            _canDoubleJump = value;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
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
        else if (IsGrounded)
        {
            canDoubleJump = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //known issues: glitchy interactions if touchin 2 interactible objects at one time
        canInteract = true;
        interactingObject = collision.gameObject;
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        //known issues: glitchy interactions if touchin 2 interactible objects at one time
        canInteract = false;
        interactingObject = null;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (canInteract)
        {
            InteractionController con = interactingObject.GetComponent<InteractionController>();
            con.Interact();
        }
    }
}
