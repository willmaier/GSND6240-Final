using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadzoneRespawn : MonoBehaviour
{
    public Transform respawnPoint;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.transform.position = respawnPoint.position;
            AudioManager.instance.Play("Explosion1");
        }
    }
}
