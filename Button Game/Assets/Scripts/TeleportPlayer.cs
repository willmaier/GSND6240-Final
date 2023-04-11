using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public GameObject player;
    public Transform teleportTarget;

   /* private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("colliding");
        //if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Debug.Log("Teleporting!");
            player.transform.position = teleportTarget.transform.position;
        }
    }
   */
}
