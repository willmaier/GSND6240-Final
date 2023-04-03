using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private PlayerMovement playerMovementScript;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerMovementScript = player.GetComponent<PlayerMovement>();
        playerTransform = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovementScript.facingRight)
        {
            transform.position = new Vector3(playerTransform.position.x + 4, playerTransform.position.y + 1, -10);
        }
        else
        {
            transform.position = new Vector3(playerTransform.position.x - 4, playerTransform.position.y + 1, -10);
        }
    }
}
