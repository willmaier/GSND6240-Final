using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    public ButtonGameControls loadedControls;

    private InputAction playerHorizontalMovement;
    private InputAction playerJump;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private float jumpForce = 10f;

    private enum MovementState { idle, jumping, falling }

    public bool facingRight = true;

    private void Awake()
    {
        loadedControls = new ButtonGameControls();
    }
    
    private void OnEnable()
    {
        playerHorizontalMovement = loadedControls.Player.MoveHorizontally;
        playerHorizontalMovement.Enable();
        playerJump = loadedControls.Player.Jump;
        playerJump.Enable();
    }

    private void OnDisable()
    {
        playerHorizontalMovement.Disable();
        playerJump.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(facingRight);
        dirX = playerHorizontalMovement.ReadValue<float>();

        if (dirX < 0)
        {
            facingRight = false;
        }

        if (dirX == 1)
        {
            facingRight = true;
        }

        // Move horizontally
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        // Jump
            if (playerJump.WasPressedThisFrame() && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    // better way to check for button presses?
    /* private bool isJumping()
    {
        if (Input.GetKey(KeyCode.Keypad3)
        {
            return true;
        }
    } */
}
