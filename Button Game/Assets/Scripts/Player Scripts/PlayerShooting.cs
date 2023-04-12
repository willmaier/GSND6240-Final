using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerShooting : MonoBehaviour
{
    public GameObject gunTip, shot, chargedShot, specialShot;

    public float shotSpeed = 5.0f;
    public float chargedShotSpeed = 2.5f;

    [SerializeField] public float chargeTime = 1.0f;

//    private bool charging = false;
/*    [SerializeField] private float _chargingPercent = 0;

    public float chargingPercent
    {
        get
        {
            return _chargingPercent;
        }
        set
        {
            if (value >= 100)
            {
                _chargingPercent = 100;
            }
            else
            {
                _chargingPercent = value;
            }
        }
    }*/

    public void OnSimpleFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Simple shot.");
            Shoot();
        }
    }

    public void OnChargedFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Charge started.");
        }
        if (context.canceled)
        {
            Debug.Log("Charge ended.");
            double x = context.duration;
            Debug.Log(x);
            if (x >= chargeTime)
            {
                ChargedShoot();
            }
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
