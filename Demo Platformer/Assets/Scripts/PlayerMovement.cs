using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float jumpSpeed;
    public float wallJumpSpeed;
    public float wallVelocity;
    public float velocityToFall; // The y velocity in which to play fall animation
    public LayerMask groundMask;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool onWall;
    [HideInInspector] public bool hasDoubleJumped;
    private bool inputJump;
    
    private Rigidbody2D playerRb;
    private Animator anim;
    private SpriteRenderer sr;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
	anim = GetComponent<Animator>();
	sr = GetComponent<SpriteRenderer>();

	hasDoubleJumped = false;
	inputJump = false;
	onWall = false;
    }

    void Update()
    {
	// Check if able to jump
	if (Input.GetButtonDown("Jump"))
	{
	    /*
	     * Due to the way Unity handles Update() and FixedUpdate(), whether
	     * the player can jump has to be handled at all frames, where as
	     * if the player shouldn't jump, then inputJump is set to false
	     * in FixedUpdate(). Setting a conditional statement to set it
	     * as off would not sync the input hande with the actual jumping.
	     */
	    inputJump = true;
	}
    }

    void FixedUpdate()
    {

	// Check if player is grounded through raycast
	RaycastHit2D hitGround = Physics2D.Raycast(transform.position, -Vector2.up, 1.0f, groundMask);
	if (hitGround)
	{
	    isGrounded = true;
	    hasDoubleJumped = false;
	}

	// Check if player is hitting wall (For walljumping)
	RaycastHit2D hitWall = Physics2D.Raycast(transform.position, new Vector2(transform.position.x,0), 1.0f, groundMask);
	if (hitWall)
	{
	    onWall = true;
	    playerRb.velocity = new Vector2(playerRb.velocity.x, wallVelocity);
	    anim.SetBool("Is On Wall", true);
	}
        else
	{
	    onWall = false;
	    anim.SetBool("Is On Wall", false);
	}
	
        // For player movement
        float inputX = Input.GetAxisRaw("Horizontal");
        playerRb.velocity = new Vector2 (inputX * movementSpeed, playerRb.velocity.y);

	// For player jumping
        if (isGrounded && inputJump && !onWall)
        {
	    playerRb.velocity = new Vector2 (playerRb.velocity.x, 0); // Reset y velocity for jump
            StartCoroutine("Jump");
	    anim.SetTrigger("Is Jumping");
	    inputJump = false;
        }
	// For double jumping
	else if (!hasDoubleJumped && inputJump && !onWall)
	{
	    playerRb.velocity = new Vector2 (playerRb.velocity.x, 0);
	    StartCoroutine("Jump");
	    anim.SetTrigger("Is Double Jumping");
	    hasDoubleJumped = true;
	    inputJump = false;
	}
	// For wall jumping
	if (inputJump && onWall)
	{
	    playerRb.velocity = new Vector2 (0, 0);
	    playerRb.AddForce(new Vector2(wallJumpSpeed * -inputX, jumpSpeed), ForceMode2D.Impulse);
	    onWall = false;
	    inputJump = false;
	    hasDoubleJumped = false; // Allow layer to double jump after wall jumping
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
