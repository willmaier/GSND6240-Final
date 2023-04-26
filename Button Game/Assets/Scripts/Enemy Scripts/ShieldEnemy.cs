using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : MonoBehaviour
{
    public GameObject shield;

    bool isShielded = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ChargedBullet" && !isShielded)
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == "ChargedBullet" && isShielded)
        {
            Destroy(shield);
            Destroy(collision.gameObject);
            isShielded = false;
        }

        else if (collision.gameObject.tag == "PlayerBullet" && !isShielded)
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == "PlayerBullet" && isShielded)
        {
            Destroy(collision.gameObject);
        }


    }
}
