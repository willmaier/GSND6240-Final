using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerShooting : MonoBehaviour
{
    public GameObject gunTip, shot, chargedShot, specialShot;

    public float shotSpeed = 5.0f;
    public float chargedShotSpeed = 2.5f;

    [SerializeField] public bool charging = false;
    [SerializeField] public float _chargeTime;
    [SerializeField] public float chargeTimeLimit = 1.0f;

    public float chargeTime
    { 
        get
        {
            return _chargeTime;
        }
        private set
        { 
            if (value <= chargeTimeLimit)
            {
                _chargeTime = value;
            }
            else
            {
                _chargeTime = chargeTimeLimit;
            }
        }
    } 

/*    public ButtonGameControls playerControls;
    private InputAction simpleFire;
    private InputAction chargedFire;

    private void Awake()
    {
        playerControls = new ButtonGameControls();
    }

    private void OnEnable()
    {
        simpleFire = playerControls.Player.SimpleFire;
        simpleFire.Enable();
        chargedFire = playerControls.Player.ChargedFire;
        chargedFire.Enable();
    }

    private void OnDisable()
    {
        simpleFire.Disable();
        chargedFire.Disable();
    }*/

    private void FixedUpdate()
    {
        if (charging)
        {
            chargeTime += Time.deltaTime;
        }
    }

    //Known issue: press down 9 and then pressing 8 will first perform the simple fire, then charge the charged fire.
    //If the player was just trying to make a charged fire, but accidentally pressed 9 before 8, they may feel that the simple fire is an unintended action
    //Ideally, there should be a super tiny delay grace period to see if the player wants simple fire or charged fire.
    public void OnSimpleFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (charging) //If you're also doing a charged fire...
            {
                return; /// then do nothing.
            }
            //Debug.Log("Simple shot.");
            Shoot();
        }
    }

    public void OnChargedFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //Debug.Log("Charge started.");
            charging = true;
        }
        if (context.canceled)
        {
            //Debug.Log("Charge ended.");
            if (chargeTime >= chargeTimeLimit)
            {
                ChargedShoot();
            }
            charging = false;
            chargeTime = 0;
        }
    }

    void Shoot()
    {
        var bullet = Instantiate(shot, gunTip.transform.position, transform.rotation);
        //bullet.velocity (Vector2.right * shotSpeed * Time.deltaTime);
        bool isFacingRight = GetComponent<PlayerController>().IsFacingRight;
        if (isFacingRight)
        {
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.right * shotSpeed;
        }
        else
        {
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.left * shotSpeed;
        }
        
        Destroy(bullet, 5);
    }

    void ChargedShoot()
    {
        var chargedBullet = Instantiate(chargedShot, gunTip.transform.position, transform.rotation);
        //bullet.velocity (Vector2.right * shotSpeed * Time.deltaTime);
        bool isFacingRight = GetComponent<PlayerController>().IsFacingRight;
        if (isFacingRight)
        {
            chargedBullet.GetComponent<Rigidbody2D>().velocity = Vector2.right * chargedShotSpeed;
        }
        else
        {
            chargedBullet.GetComponent<Rigidbody2D>().velocity = Vector2.left * chargedShotSpeed;
        }

        Destroy(chargedBullet, 5);
    }
}
