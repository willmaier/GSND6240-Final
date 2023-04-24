using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintPopup : MonoBehaviour
{
    public GameObject messagePrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            messagePrefab.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            messagePrefab.SetActive(false);
        }
    }
}
