using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    private Rigidbody2D rb2d;

    void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 colliderDirection = col.GetContact(0).normal;
        Vector2 collisionPoint = col.GetContact(0).point;

        // Detect if enemy collided with the player
        if (col.gameObject.tag == "Player" && collisionPoint.y < transform.position.y)
        {
            Destroy(col.gameObject);
        }
    }
}
