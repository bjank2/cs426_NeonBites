using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject grenadePrefab; // Assign your grenade prefab in the inspector
    public Transform spawnPoint; // Assign a spawn point in front of the player
    public float throwForce = 40f;

    void Update()
    {
        // Check if right mouse button is held down
        if (Input.GetMouseButton(1)) // Right mouse button is pressed
        {
            // Check if G key is pressed
            if (Input.GetKeyDown(KeyCode.G))
            {
                ThrowGrenade();
            }
        }
    }

    void ThrowGrenade()
    {
        // Instantiate the grenade at the spawn point
        GameObject grenade = Instantiate(grenadePrefab, spawnPoint.position, spawnPoint.rotation);

        // Add force to the grenade to throw it
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(spawnPoint.forward * throwForce, ForceMode.VelocityChange);
        }
    }
}
