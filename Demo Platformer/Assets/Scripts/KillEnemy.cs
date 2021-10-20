using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemy : MonoBehaviour
{
    public float jumpBoost;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" && transform.position.y > col.gameObject.transform.position.y)
        {
            Destroy(col.gameObject);
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(new Vector2(0, jumpBoost), ForceMode2D.Impulse);

            gameObject.GetComponent<PlayerMovement>().hasDoubleJumped = false;
        }
    }
}
