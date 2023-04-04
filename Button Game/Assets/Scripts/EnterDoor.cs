using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor : MonoBehaviour
{
    private bool enterAllowed;
    private string sceneToLoad;
    private DoorController doorScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<DoorController>())
        {
            enterAllowed = true;
            doorScript = collision.GetComponent<DoorController>();
            sceneToLoad = doorScript.sceneToLoad;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<DoorController>())
        {
            enterAllowed = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enterAllowed && Input.GetKey(KeyCode.Return)) // Enter door key is set to Return by default
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
