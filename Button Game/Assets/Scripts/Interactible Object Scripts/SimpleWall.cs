using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet" || collision.gameObject.tag == "ChargedBullet")
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            AudioManager.instance.Play("SimpleWallBreak");
        }
    }
}
