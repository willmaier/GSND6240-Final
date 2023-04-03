using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;

    public float fireRate = 5f;
    float nextShot;

    // Start is called before the first frame update
    void Start()
    {
        nextShot = Time.time;    
    }

    // Update is called once per frame
    void Update()
    {
        CheckForTimeToFire();
    }

    void CheckForTimeToFire()
    {
        if (Time.time > nextShot)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            nextShot = Time.time + fireRate;
        }
    }
}
