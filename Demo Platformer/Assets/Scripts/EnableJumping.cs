using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableJumping : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();

        // If this is not true, then the collider was not the player, rather
        // some other object.
        if (player)
        {
            player.isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
	PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();

	if (player)
	{
	    player.isGrounded = false;
	}
    }
}
