using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float jumpSpeed;
    public Vector2 wallJumpSpeed;
    public float wallSlideSpeed;
    public float wallDetectDistance; // distance where player can be sliding on wall
    public float wallJumpTime;
    public bool wallJumped;
    public bool movementLocked;
    public float velocityToFall; // The y velocity in which to play fall animation 
    public float deathJump;
    public LayerMask groundMask;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool onWall;
    [HideInInspector] public bool hasDoubleJumped;
    private bool inputJump;

    private Rigidbody2D playerRb;
    private Animator anim;
    private SpriteRenderer sr;
    private BoxCollider2D bc;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

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
        // RaycastHit2D hitWall = Physics2D.Raycast(transform.position, new Vector2(transform.position.x, 0), wallDetectDistance, groundMask);
        // if (hitWall)
        // {
        //     onWall = !isGrounded && playerRb.velocity.y < 0; // Only set to true if not on ground and falling
        //     playerRb.velocity = new Vector2(playerRb.velocity.x, wallSlideSpeed);
        // }
        // else
        // {
        //     onWall = false;
        // }

        // For player movement
        float inputX = Input.GetAxisRaw("Horizontal");
        if (!movementLocked)
        {
            if (!wallJumped) playerRb.velocity = new Vector2(inputX * movementSpeed, playerRb.velocity.y); // Regular movement
            else// How to move player if they just jumped off a wall
            {
                // This movement preserves the in air momentum, you can actually make the regular movement like this if you want and then it will always preserve in air momentum
                // making regular movement like this would prob make this cleaner and you can get rid of wall jumped variable but you have to make a new max airspeed variable
                Vector2 velocity = playerRb.velocity;
                velocity.x = Mathf.Clamp(velocity.x + inputX * movementSpeed, -wallJumpSpeed.x, wallJumpSpeed.x);
                playerRb.velocity = velocity;
            }
        }





        // For wall jumping
        // if (inputJump && onWall)
        // {
        // 	playerRb.velocity = new Vector2 (0, 0);
        // 	playerRb.AddForce(new Vector2(wallJumpSpeed * -inputX, jumpSpeed), ForceMode2D.Impulse);
        // 	onWall = false;
        // 	inputJump = false;
        // 	hasDoubleJumped = false; // Allow layer to double jump after wall jumping
        // }

        // Make sure the player is facing the right direction
        if (inputX > 0.0f)
        {
            sr.flipX = false;
        }
        else if (inputX < 0.0f) // Needed to make explicit since player should not have a default position to face
        {
            sr.flipX = true;
        }

        // Animate movement
        anim.SetBool("Is Running", inputX != 0.0f);

        // Check if player is falling
        anim.SetBool("Is Falling", !isGrounded);

        Jumping();
        WallMovement();

        inputJump = false;
    }
    void WallMovement()
    {

        // Raycast for walls on either side of player
        RaycastHit2D rwall = Physics2D.Raycast(bc.bounds.center, Vector2.right, bc.bounds.size.x / 2 + wallDetectDistance, groundMask);
        RaycastHit2D lwall = Physics2D.Raycast(bc.bounds.center, Vector2.left, bc.bounds.size.x / 2 + wallDetectDistance, groundMask);

        onWall = (lwall || rwall) && !isGrounded && playerRb.velocity.y < 0; // Record if player is on wall

        anim.SetBool("Is On Wall", onWall);

        if (onWall)
        {
            // Check if the player is on the right or left wall, if rwall is false, then that means the player is on the left wall and dir will be -1, else then dir will be 1
            int dir = (System.Convert.ToInt32(rwall) * 2) - 1;

            // Get player movement
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            // Flip player in direction opposite to wall
            sr.flipX = !rwall;

            playerRb.velocity = new Vector2(playerRb.velocity.x, wallSlideSpeed);

            // Check for jump key and jump opposite to the wall
            if (inputJump || input.x == -dir)
            {
                // Set Animation
                anim.SetTrigger("Is Jumping");
                hasDoubleJumped = false;

                // Move player off wall
                transform.position = new Vector2(transform.position.x + (-(wallDetectDistance + 0.1f) * dir), transform.position.y);


                playerRb.velocity = new Vector2(0, 0); // Reset velocity for jump
                playerRb.AddForce(new Vector2(wallJumpSpeed.x * dir, wallJumpSpeed.y), ForceMode2D.Impulse);

                StartCoroutine(WallJumpTime(dir));
                inputJump = false;
            }
        }

    }
    IEnumerator WallJumpTime(int dir)
    {
        wallJumped = true;
        movementLocked = true;
        playerRb.velocity = new Vector2(-dir * wallJumpSpeed.x, playerRb.velocity.y);
        yield return new WaitForSeconds(wallJumpTime);
        movementLocked = false;
        wallJumped = false;
    }
    void Jumping()
    {
        // For player jumping
        if (isGrounded && inputJump && !onWall)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0); // Reset y velocity for jump
            StartCoroutine("Jump");
            anim.SetTrigger("Is Jumping");
            inputJump = false;
        }
        // For double jumping
        else if (!hasDoubleJumped && inputJump && !onWall)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
            StartCoroutine("Jump");
            anim.SetTrigger("Is Double Jumping");
            hasDoubleJumped = true;
            inputJump = false;
        }

        if (onWall) hasDoubleJumped = false;
    }
    IEnumerator Jump()
    {
        playerRb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        isGrounded = false;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && other.transform.position.y + 1 >= transform.position.y)
        {
            movementLocked = true;
            playerRb.AddForce(new Vector2(0, deathJump), ForceMode2D.Impulse);
            anim.SetTrigger("Died");
            GameManager.instance.ReloadScene();
        }
    }
}
