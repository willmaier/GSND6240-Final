using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMScript : MonoBehaviour
{
    private static bool created = false;

    void Awake()
    {
        if (!created)
        {
            // If this is the first instance of the BGM audio clip, keep it alive between scenes
            DontDestroyOnLoad(gameObject);
            created = true;
        }
        else
        {
            // If there is already an instance of the BGM audio clip, destroy this one
            Destroy(gameObject);
        }
    }

    void Update()
    {
    }
}