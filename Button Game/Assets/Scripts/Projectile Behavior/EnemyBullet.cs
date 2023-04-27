using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBullet : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Transform respawnPoint;
    Rigidbody2D rb;
    GameObject player;

    Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        moveDirection = (player.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Destroy(gameObject, 3.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            // Set respawn position to respawnPoint position
            RespawnPointManager.instance.respawnPosition = respawnPoint.position;

            // Load current scene to restart level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            AudioManager.instance.Play("ExplosionPlayer");
            AudioManager.instance.Stop("Walking");
            AudioManager.instance.Stop("BoosterHolding");
            AudioManager.instance.Stop("BoosterStart");
            AudioManager.instance.Stop("Ignition");

        }
        else if (collision.gameObject.tag == "Enemy")
        {

        }
        else if (collision.gameObject.tag == "Shield")
        {

        }
        else if (collision.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag != "Shield")
        {
            Destroy(gameObject);
        }
    }
}
