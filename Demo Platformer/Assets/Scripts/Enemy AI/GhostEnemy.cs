using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    public Vector2[] spawnLocations;
    public float waitTime;
    public float spawnSpeed;

    private bool appearing;
    private bool disappearing;
    private Rigidbody2D rb2d;
    private Animator anim;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        StartCoroutine("spawnToLocation");
    }

    IEnumerator spawnToLocation()
    {
        // Continually get the ghost to spawn in various locations
        while (true)
        {
            foreach (Vector2 location in spawnLocations)
            {
                yield return new WaitForSeconds(waitTime);

                rb2d.position = location;
                yield return new WaitForSeconds(spawnSpeed);
            }
        }
    }

    // This is used by animation keyframe
    private void destroyMe()
    {
        rb2d.velocity = new Vector2(0.0f, 0.0f);
        Destroy(this.gameObject);
    }
}
