using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectable : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            // Do Something when collected
            Debug.Log("Collected Coin!");
            Destroy(gameObject);
        }
    }
}
