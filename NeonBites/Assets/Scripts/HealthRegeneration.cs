using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegeneration : MonoBehaviour
{
    public float healthBoost = 10f; // Amount of health to regenerate

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure it's the player who collides with the capsule
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.isDead)
            {
                playerHealth.IncreaseHealth(healthBoost);
                Destroy(gameObject); // Destroy the capsule after picking it up
            }
        }
    }
}
