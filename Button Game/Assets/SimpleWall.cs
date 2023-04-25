using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
