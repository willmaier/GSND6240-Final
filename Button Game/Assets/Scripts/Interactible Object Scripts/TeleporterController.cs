using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterController : MonoBehaviour
{
    public GameObject player;
    public Transform teleportTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke("Teleport", 3);
        }
    }

    public void Teleport()
    {
        Debug.Log("Teleporting!");
        player.transform.position = teleportTarget.transform.position;
    }
}
