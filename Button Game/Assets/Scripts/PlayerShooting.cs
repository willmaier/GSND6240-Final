using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerShooting : MonoBehaviour
{
    public GameObject gunTip, shot, chargedShot, specialShot;

    public float shotSpeed = 5.0f;

    public ButtonGameControls loadedControls;

    private InputAction playerShoot;
    private void Awake()
    {
        loadedControls = new ButtonGameControls();
    }

    private void OnEnable()
    {
        playerShoot = loadedControls.Player.Fire;
        playerShoot.Enable();
    }

    private void OnDisable()
    {
        playerShoot.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerShoot.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        var bullet = Instantiate(shot, gunTip.transform.position, transform.rotation);
        //bullet.velocity (Vector2.right * shotSpeed * Time.deltaTime);
        bool isFacingRight = GetComponent<PlayerController>().facingRight;
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
}
