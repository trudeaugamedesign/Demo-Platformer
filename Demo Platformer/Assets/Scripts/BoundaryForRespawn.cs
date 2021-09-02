using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryForRespawn : MonoBehaviour
{
    public Vector2 respawnPosition;
    
    void OnTriggerEnter2D(Collider2D col)
    {
	if (col.gameObject.GetComponent<PlayerMovement>())
	{
	    col.gameObject.transform.position = respawnPosition;
	}
    }
}
