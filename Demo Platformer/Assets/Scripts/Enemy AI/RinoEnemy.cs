using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinoEnemy : MonoBehaviour
{
    public float initialSpeed;
    public float acceleration;
    public float maxSpeed;
    public int direction;
    public LayerMask wall;
    public Vector2 knockbackForce;

    private Rigidbody2D rb2d;
    private SpriteRenderer sr;
    private Animator anim;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        rb2d.velocity = new Vector2(initialSpeed, rb2d.velocity.y);
    }

    void FixedUpdate()
    {
        // Rino accelerates instead of just increasing velocity
        float acceleratedMovement = initialSpeed * acceleration * rb2d.velocity.x;
        rb2d.velocity = new Vector2(Mathf.Clamp(acceleratedMovement, -maxSpeed, maxSpeed), rb2d.velocity.y);

        // Check if rigidbody hit a wall
        Vector2 raycastPos = new Vector2(transform.position.x, transform.position.y - 1);
        RaycastHit2D hitRight = Physics2D.Raycast(raycastPos, Vector2.right, 1.7f, wall);
        RaycastHit2D hitLeft = Physics2D.Raycast(raycastPos, Vector2.left, 1.7f, wall);

        if (hitRight && direction == 1)
        {
            StartCoroutine("restingPeriod", -1);
            Debug.Log("right");
        }
        if (hitLeft && direction == -1)
        {
            StartCoroutine("restingPeriod", 1);
            Debug.Log("left");
        }

        // Face the right direction
        if (rb2d.velocity.x > 0.0f)
        {
            sr.flipX = true;
        }
        else if (rb2d.velocity.x < 0.0f)
        {
            sr.flipX = false;
        }

        // Rino Animation
        if (rb2d.velocity.x != 0.0f)
        {
            anim.SetBool("Is Running", true);
        }
        else 
        {
            anim.SetBool("Is Running", false);
        }
    }

    private void knockBack(int direction)
    {
        rb2d.AddForce(new Vector2(knockbackForce.x * direction, knockbackForce.y), ForceMode2D.Impulse);
    }

    // This is used by animation keyframe
    private void destroyMe()
    {
        rb2d.velocity = new Vector2(0.0f, 0.0f);
        Destroy(this.gameObject);
    }

    IEnumerator restingPeriod(int direction)
    {
        anim.SetTrigger("Has Crashed");
        knockBack(direction);
        this.direction = -this.direction;

        yield return new WaitForSeconds(1);
        rb2d.velocity = new Vector2(initialSpeed * direction, rb2d.velocity.y);
    }
}
