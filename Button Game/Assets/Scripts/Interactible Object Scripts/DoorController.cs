using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour, IInteractible
{
    [SerializeField] public string sceneToLoad;
    public void OnInteract()
    {
        Debug.Log("Loading new scene");
        SceneManager.LoadScene(sceneToLoad);
    }

}
