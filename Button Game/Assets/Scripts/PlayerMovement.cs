using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private float jumpForce = 10f;

    private enum MovementState { idle, jumping, falling }

    public bool facingRight = true;
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
        dirX = Input.GetAxisRaw("Horizontal");

        if (dirX < 0)
        {
            facingRight = false;
        }

        if (dirX == 1)
        {
            facingRight = true;
        }

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        // traditional 'spacebar' input is commented out
        // if (Input.GetButtonDown("Jump") && IsGrounded())
            if (Input.GetKey(KeyCode.Keypad3) && Input.GetKey(KeyCode.Keypad7) && IsGrounded())
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
