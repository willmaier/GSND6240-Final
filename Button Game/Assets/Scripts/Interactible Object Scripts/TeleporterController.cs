using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterController : MonoBehaviour
{
    public GameObject player;
    public Transform teleportTarget;

    public void OnInteract()
    {
        Debug.Log("Teleporting!");
        player.transform.position = teleportTarget.transform.position;
    }
}
