using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding : MonoBehaviour
{
    private PlayerIsHiding playerIsHiding;

    // Start is called before the first frame update
    void Start()
    {
        // Get reference to the PlayerIsHiding script attached to the player
        playerIsHiding = GameObject.FindWithTag("Player").GetComponent<PlayerIsHiding>();

        // Check if playerIsHiding was found, log a warning if not
        if (playerIsHiding == null)
        {
            Debug.LogWarning("PlayerIsHiding script not found on Player object.");
        }
    }

    // Called when an object enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set IsHiding to true when the player enters the trigger
            if (playerIsHiding != null)
            {
                playerIsHiding.IsHiding = true;
                Debug.Log("Player has entered hiding area.");
            }
        }
    }

    // Called when an object exits the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set IsHiding to false when the player exits the trigger
            if (playerIsHiding != null)
            {
                playerIsHiding.IsHiding = false;
                Debug.Log("Player has left hiding area.");
            }
        }
    }
}
