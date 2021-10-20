using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    private Rigidbody2D rb2d;

    void OnCollisionEnter2D(Collision2D col)
    {
        // Detect if enemy collided with the player
        if (col.gameObject.tag == "Player" && col.gameObject.transform.position.y <= transform.position.y)
        {
            GameManager.playerHit = true;
            Destroy(col.gameObject);
        }
    }
}
