using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectable : MonoBehaviour
{

    private Animator anim;

    void Start()
    {
	anim = GetComponent<Animator>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            Debug.Log("Collected Coin!");
	    anim.SetTrigger("Is Collected"); // Item is destroyed through animation event, hence the functino DestroyObject()
        }
    }

    void DestroyObject()
    {
	Destroy(gameObject);
    }
}
