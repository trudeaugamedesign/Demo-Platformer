using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float jumpSpeed;
    public float velocityToFall; // The y velocity in which to play fall animation

    [HideInInspector] public bool isGrounded;
    private Rigidbody2D playerRb;
    private Animator anim;
    private SpriteRenderer sr;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
	anim = GetComponent<Animator>();
	sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // For player movement
        float inputX = Input.GetAxisRaw("Horizontal");
        playerRb.velocity = new Vector2 (inputX * movementSpeed, playerRb.velocity.y);

        // Check if able to jump
        float inputJump = Input.GetAxis("Jump");
        if (isGrounded && inputJump > 0.0f)
        {
            StartCoroutine("Jump");
	    anim.SetTrigger("Is Jumping");
        }

	// Make sure the player is facing the right direction
	if (inputX > 0.0f)
	{
	    sr.flipX = false;
	}
	else if (inputX < 0.0f)	// Needed to make explicit since player should not have a default position to face
	{
	    sr.flipX = true;
	}

	// Animate movement
	if (inputX != 0.0f)
	{
	    anim.SetBool("Is Running", true);
	}
	else
	{
	    anim.SetBool("Is Running", false);
	}

	// Check if player is falling
	if (playerRb.velocity.y < velocityToFall)
	{
	    anim.SetBool("Is Falling", true);
	}
	else
	{
	    anim.SetBool("Is Falling", false);
	}
    }

    IEnumerator Jump()
    {
        playerRb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        isGrounded = false;
        yield return null;
    }
}
