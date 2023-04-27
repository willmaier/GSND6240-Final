using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointManager : MonoBehaviour
{
    public static RespawnPointManager instance;
    [SerializeField] public Vector3 respawnPosition; // Variable to store respawn position

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);// game object will not be destroyed on scene reload 
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public Vector3 GetRespawnPosition()
    {
        return respawnPosition;
    }

    public void SetRespawnPosition(Vector3 position)
    {
        respawnPosition = position;
    }
}
