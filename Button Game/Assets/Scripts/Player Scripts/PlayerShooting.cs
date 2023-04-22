using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerShooting : MonoBehaviour
{
    public GameObject gunTip, shot, chargedShot, specialShot;

    public float shotSpeed = 5.0f;
    public float chargedShotSpeed = 2.5f;

    [SerializeField] public bool chargedModeButton;
    [SerializeField] public bool fireButton;

    [SerializeField] public bool cShotCharging = false;
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
    private void FixedUpdate()
    {
        if (cShotCharging)
        {
            chargeTime += Time.deltaTime;
        }
    }

    public void OnSimpleFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            fireButton = true;
            CShotChargingStartCheck();
            if (!cShotCharging)
            {
                //Debug.Log("Simple shot.");
                Shoot();
            }
        }
        if (context.canceled)
        {
            fireButton = false;
            CShotChargingEndCheck();
        }
    }
    public void OnChargedMode(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            chargedModeButton = true;
            CShotChargingStartCheck();
        }
        if (context.canceled)
        {
            chargedModeButton = false;
            CShotChargingEndCheck();
        }
    }

    void CShotChargingStartCheck()
    {
        if (chargedModeButton && fireButton)
        {
            //Debug.Log("Charge started.");
            cShotCharging = true; // Turn on charge tracker
        }
    }

    void CShotChargingEndCheck()
    {
        cShotCharging = false; // Turn off charge time tracker
        if (chargeTime >= chargeTimeLimit) //check if successful charged shot
        {
            //Debug.Log("Successful charged shot.");
            ChargedShoot();
        } // else debug log unsuccessful charged shot
        chargeTime = 0; // then set charge time to 0
    }



    void Shoot()
    {
        var bullet = Instantiate(shot, gunTip.transform.position, transform.rotation);
        //bullet.velocity (Vector2.right * shotSpeed * Time.deltaTime);
        bool isFacingRight = GetComponent<PlayerMovement>().IsFacingRight;
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
        bool isFacingRight = GetComponent<PlayerMovement>().IsFacingRight;
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
