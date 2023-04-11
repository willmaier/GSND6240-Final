using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public ButtonGameControls loadedControls;

    private InputAction interact;

    private bool enterAllowed;

    private string sceneToLoad;
    private DoorController doorScript;

    private GameObject player;
    private bool teleportAllowed = false;
    private TeleportPlayer teleportScript;
    private Transform teleportLocation;

    private void Awake()
    {
        loadedControls = new ButtonGameControls();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        interact = loadedControls.Player.Interact;
        interact.Enable();
    }

    private void OnDisable()
    {
        interact.Disable();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<DoorController>())
        {
            enterAllowed = true;
            doorScript = col.gameObject.GetComponent<DoorController>();
            sceneToLoad = doorScript.sceneToLoad;
        }

        else if (col.GetComponent<TeleportPlayer>())
        {
            teleportAllowed = true;
            teleportScript = col.gameObject.GetComponent<TeleportPlayer>();
            teleportLocation = teleportScript.teleportTarget;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<DoorController>())
        {
            enterAllowed = false;
        }
        else if (col.GetComponent<TeleportPlayer>())
        {
            teleportAllowed = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enterAllowed && interact.WasPressedThisFrame()) // Enter door key is set to Return by default
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        if (teleportAllowed && interact.WasPressedThisFrame())
        {
            player.transform.position = teleportLocation.transform.position;
        }
    }
}