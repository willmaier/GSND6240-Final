using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointManager : MonoBehaviour
{
    public static RespawnPointManager instance;
    public Vector3 respawnPosition; // new variable to store respawn point

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);// game object will not be destroyed on scene reload 
            instance = this;
            respawnPosition = new Vector3(-6.95f, 0.82f, 0f); // set initial respawn position here
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
