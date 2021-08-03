using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float jumpSpeed;

    [HideInInspector] public bool canJump = false;
    private Rigidbody2D playerRb;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // For player movement
        float inputX = Input.GetAxis("Horizontal");
        playerRb.velocity = new Vector2 (inputX * movementSpeed, playerRb.velocity.y);

        // Check if able to jump
        float inputJump = Input.GetAxis("Jump");
        if (canJump && inputJump > 0.0f)
        {
            StartCoroutine("Jump");
        }
    }

    IEnumerator Jump()
    {
        playerRb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        canJump = false;
        yield return null;
    }
}
