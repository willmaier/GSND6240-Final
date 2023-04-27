using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChrageBulletCollisionBehavior : MonoBehaviour
{
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shield")//Charge shot destory shield tag.
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            AudioManager.instance.Play("ShieldBreak");
        }

        else if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
        /*else if (collision.gameObject.tag == "PlayerBullet")
        {

        }
        else if (collision.gameObject.tag == "ChargedBullet")
        {

        }*/


        else if (collision.gameObject.layer == 3) // 3 is layer of solid environment
        {
            Destroy(gameObject);
            AudioManager.instance.Play("ChargeHitOnWall");
        }

        /*else if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }*/

    }
}
