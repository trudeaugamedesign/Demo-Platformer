using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomEnemy : MonoBehaviour
{
    public float speed;
    public float distanceBeforeTurning; 
    public LayerMask wall;

    public int direction;            
    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2d.velocity = new Vector2(speed * direction, rb2d.velocity.y);

        // Check if rigidbody hit a wall
        Vector2 raycastPos = new Vector2(transform.position.x, transform.position.y - 1);
        RaycastHit2D hitRight = Physics2D.Raycast(raycastPos, Vector2.right, distanceBeforeTurning, wall);
        RaycastHit2D hitLeft = Physics2D.Raycast(raycastPos, Vector2.left, distanceBeforeTurning, wall);

        if (hitRight && direction == 1)
        {
            direction = -direction;
        }
        else if (hitLeft && direction == -1)
        {
            direction = -direction;
        }

        // Check if player is running
        if (rb2d.velocity.x != 0.0f)
        {
            anim.SetBool("Is Running", true);
        }
        else 
        {
            anim.SetBool("Is Running", false);
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
    }

    // This is used by animation keyframe
    private void destroyMe()
    {
        rb2d.velocity = new Vector2(0.0f, 0.0f);
        Destroy(this.gameObject);
    }
}
