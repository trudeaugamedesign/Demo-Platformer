using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    private Rigidbody2D rb2d;

    void OnCollisionEnter2D(Collision2D col)
    {
        // Detect if enemy collided with the player (if they have playermovement script)
        if (col.gameObject.tag == "Player" && !GameManager.playerInvincible)
        {
            GameManager.playerHit = true;
            Destroy(col.gameObject);
        }
    }
}
