using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerShooting : MonoBehaviour
{
    public GameObject gunTip, shot, chargedShot, specialShot;

    public float shotSpeed = 5.0f;
    public float chargedShotSpeed = 2.5f;

    public void OnSimpleFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Shoot();
        }
    }

    public void OnChargedFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Charge started.");
            ChargedShoot();
        }
        if (context.canceled)
        {
            Debug.Log("Charge ended.");
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
