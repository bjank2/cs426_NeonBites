using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public GameObject explosionEffect; // Assign your particle effect prefab in the Inspector
    public float delay = 3f; // Delay before the grenade explodes

    public GameObject grenade;

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine to explode after a delay
        StartCoroutine(ExplodeAfterDelay());
    }

    IEnumerator ExplodeAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Instantiate the explosion effect at the grenade's position and rotation
         

        // Optionally, play sound effects or trigger other events here
        explosionEffect.SetActive(true);
        grenade.SetActive(false);

        yield return new WaitForSeconds(1.9f);
        Destroy(gameObject);

    }
}
