using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Player variables
    public static float playerInvincibilityTime;
    [HideInInspector] public static bool playerHit;
    public static bool playerInvincible;

    void Update()
    {
        // Activate player invincibility when hit
        if (playerHit)
        {
            playerInvincible = true;
            countdownPlayerInvincibility();
        }
    }

    static IEnumerator countdownPlayerInvincibility()
    {
        float duration = playerInvincibilityTime;
        duration -= Time.deltaTime;
        if (duration < 0)
        {
            playerInvincible = false;
            Debug.Log("End of player invincibility");
            yield return null;
        }
    }
}
