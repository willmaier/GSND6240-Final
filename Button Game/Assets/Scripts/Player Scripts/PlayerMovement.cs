using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    // AWAKE SETTINGS
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private Animator anim;

    private void Awake()
    {
        jetpackConsumptionRechargeSpeed = jetpackFuelLimit / jetpackConsumptionRechargeTime;
        JetpackFuel = jetpackFuelLimit;
        myFuelBar.SetMaxFuel(jetpackFuelLimit);

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }

    // ANIMATOR CHECKS
    private bool _isFacingRight = true;
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

    private bool _isGroundMoving = false;
    public bool IsGroundMoving
    {
        get
        {
            return _isGroundMoving;
        }
        private set
        {
            if (_isGroundMoving != value) //If a new value is being set (i.e. if IsGroundMoving's status changes)
            {
                // Set walking audio
                if (value)
                {
                    AudioManager.instance.Play("Walking");
                }
                else
                {
                    AudioManager.instance.Stop("Walking");
                }
            }


            _isGroundMoving = value;
            anim.SetBool("isMoving", value); //Set walking animation
        }
    }

    private bool _isGrounded = true;
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

    private static float jetpackFuelLimit = 100;
    [SerializeField] private float jetpackConsumptionRechargeTime = 5;
    private float jetpackConsumptionRechargeSpeed;
    [SerializeField] private float _jetpackFuel;

    [SerializeField] private FuelBar myFuelBar;

    public float JetpackFuel
    {
        get
        {
            return _jetpackFuel;
        }
        private set
        {
            if (value >= jetpackFuelLimit)
            {
                _jetpackFuel = jetpackFuelLimit;
            }
            else if (value <= 0)
            {
                _jetpackFuel = 0;
            }
            else
            {
                _jetpackFuel = value;
            }

        }
    }

    private bool _jetpackOn = false;

    public bool JetpackOn 
    {
        get
        {
            return _jetpackOn;
        }
        private set
        {
            if (_jetpackOn != value) //If a new value is being set (i.e. if JetpackOn's status changes)
            {
                // Set jetpack audio
                if (value) //If the new status is that the jetpack is on...
                {
                    AudioManager.instance.Play("BoosterStart");
                    AudioManager.instance.Play("BoosterHolding");
                }
                else //If the new status is that the jetpack is off...
                {
                    AudioManager.instance.Stop("BoosterStart");
                    AudioManager.instance.Stop("BoosterHolding");
                }
            }

            _jetpackOn = value;
            anim.SetBool("jetpackOn", value);
        }
    }
    private void FixedUpdate()
    {
        // Check if grounded
        IsGrounded = Physics2D.CapsuleCast(coll.bounds.center, coll.bounds.size, CapsuleDirection2D.Vertical, 0f, Vector2.down, 0.05f, jumpableGround);
        SetGroundMoving(); //Set IsGroundMoving for animator and audio

        // Check if jetpack on
        // Jetpack is on if jetpack button is held & jetpack has fuel & player is not grounded
        if (jetpackButton && JetpackFuel > 0 && !IsGrounded)
        { JetpackOn = true; } 
        else 
        { JetpackOn = false; }

        //Horizontal move
        rb.velocity = new Vector2(horizontalMoveInput * moveSpeed, rb.velocity.y);

        if (JetpackOn)
        {
            //Move using jetpack
            rb.velocity = new Vector2(rb.velocity.x, verticalMoveInput * jetpackMoveSpeed);
        }

        //Move with moving platforms
        if (isOnMovingPlatform)
        {
            rb.velocity += platformRBody.velocity;
        }

        // Recharge or consume jetpack
        SetJetpackCharging();
        JetpackFuel += jetpackConsumptionRechargeSpeed * Time.deltaTime * chargingStatus;
        myFuelBar.SetFuel(JetpackFuel);
    }

    private float chargingStatus = 0;
    private void SetJetpackCharging()
    {
        // Check if can recharge jetpack
        if (JetpackOn) // If player is using jetpack movement midair
        {
            chargingStatus = -1; // Draining
            return;
        } 

        if (IsGrounded) // If grounded
        { 
            chargingStatus = 1; // Charging
            return;
        }  // also scrapped double jump var: canDoubleJump = true;

        if (!JetpackOn && !IsGrounded && chargingStatus != 1) // If the player turns off jetpack midair
        {
            chargingStatus = 0; // Maintaining
        } 

        // Otherwise maintain previous charging status
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

    private void SetGroundMoving()
    {
        if (!IsGrounded)
        {
            IsGroundMoving = false;
            return;
        }
        
        IsGroundMoving = horizontalMoveInput != 0; // Set is ground moving for animator and audio
    }

    private float horizontalMoveInput;

    public void OnMoveHorizontal(InputAction.CallbackContext context)
    {
        horizontalMoveInput = context.ReadValue<float>();
        SetFacingDirection(horizontalMoveInput); // Set facing direction only when movement is inputted
    }

    private bool isOnMovingPlatform = false;

    private Rigidbody2D platformRBody;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsGrounded)
        {
            AudioManager.instance.Play("Landing");
        }

        if (IsGrounded && collision.gameObject.CompareTag("MovablePlatform"))
        {
            isOnMovingPlatform = true;
            platformRBody = collision.gameObject.GetComponent<Rigidbody2D>();
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovablePlatform"))
        {
            isOnMovingPlatform = false;
            platformRBody = null;
        }
    }

    //    [SerializeField] private bool canDoubleJump = true;
    private float verticalMoveInput;

    public void OnMoveUp(InputAction.CallbackContext context)
    {
        verticalMoveInput = context.ReadValue<float>();

        if (context.started && IsGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); //Jump
            AudioManager.instance.Play("Jumping");
        }
    }

        /* Scrapped double jump
        if (context.started && canDoubleJump)
        {
          rb.velocity = new Vector2(rb.velocity.x, jumpForce);
          canDoubleJump = false;
        }
        */

    private bool jetpackButton = false;

    public void OnJetpackButton(InputAction.CallbackContext context)
    {
        // "Hold"-type on-off switch
        if (context.started)
        {
            jetpackButton = true;
            AudioManager.instance.Play("Ignition"); //Play an ignition sound regardless of actual jetpack status
        }
        if (context.canceled)
        {
            jetpackButton = false;
        }
    }
}

/*

Scrapped Interaction
----

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

*/
