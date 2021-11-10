using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinoEnemy : MonoBehaviour
{
    public float speed;
    public float direction;
    public float acceleration;
    public float distanceBeforeTurning = 1.0f;
    public LayerMask wall;
    public GameObject objectToTriggerMovement;

    private SpriteRenderer sr;
    private Rigidbody2D rb2d;
    private Animator anim;
    private float accelerationCount;
    private int shouldMove = 1;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject == objectToTriggerMovement)
        {
            shouldMove = 1;
        }
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        accelerationCount = acceleration;

        shouldMove = 0;
    }

    void FixedUpdate()
    {
        accelerationCount += acceleration;

        // Move forwards
        rb2d.AddForce(new Vector2(speed * direction * accelerationCount * shouldMove, 0), ForceMode2D.Impulse);
        
        // Check if hit the wall (depending on direction)
        RaycastHit2D hit = new RaycastHit2D();
        Vector2 raycastTransform = new Vector2(rb2d.position.x, rb2d.position.y - 1);
        if (direction == 1)
        {
            hit = Physics2D.Raycast(raycastTransform, Vector2.right, distanceBeforeTurning, wall);
            accelerationCount = 1;
        }
        else if (direction == -1)
        {
            hit = Physics2D.Raycast(raycastTransform, Vector2.left, distanceBeforeTurning, wall);
            accelerationCount = 1;
        }

        // If hit wall, turn around
        if (hit.collider != null)
        {
            StartCoroutine("knockedBack");  
            direction = -direction;
        }

        // Turn the right direction (and with animation)
        if (rb2d.velocity.x < 0.0f)
        {
            anim.SetBool("Is Running", true);
            sr.flipX = false;
        }
        else if (rb2d.velocity.x > 0.0f)
        {
            anim.SetBool("Is Running", true);
            sr.flipX = true;
        }
        else 
        {
            anim.SetBool("Is Running", false);
        }
    }

    IEnumerator knockedBack()
    {
        shouldMove = 0;
        rb2d.AddForce(new Vector2(5.0f * -direction, 4.0f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(1.5f);
        shouldMove = 1;
    }

    void destroyMe()
    {
        Destroy(this.gameObject);
    }
}
