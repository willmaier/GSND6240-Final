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

    private void Awake()
    {
        loadedControls = new ButtonGameControls();
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
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<DoorController>())
        {
            enterAllowed = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enterAllowed && interact.WasPressedThisFrame()) // Enter door key is set to Return by default
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}