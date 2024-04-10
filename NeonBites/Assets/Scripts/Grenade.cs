using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject grenadePrefab; // Assign your grenade prefab in the inspector
    public Transform spawnPoint; // Assign a spawn point in front of the player
    public float throwForce = 40f;

    public GameObject trajectoryPointPrefab; // Assign your small sphere prefab here

    public int resolution = 30; // How many points to calculate - higher gives a smoother line

    private List<GameObject> trajectoryPoints = new List<GameObject>();

    void Start()
    {

    }

    void Update()
    {
        // Check if right mouse button is held down
        if (Input.GetMouseButton(1)) // Right mouse button is pressed
        {
            //ShowTrajectoryPoints(resolution);

            trajectoryPointPrefab.SetActive(true);

            // Check if G key is pressed
            if (Input.GetKeyDown(KeyCode.G))
            {
                ThrowGrenade();
                ClearTrajectoryPoints(); // Clear points after throwing

                trajectoryPointPrefab.SetActive(false);
            }
        }
        else
        {
            ClearTrajectoryPoints();

            trajectoryPointPrefab.SetActive(false);
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

    void ShowTrajectoryPoints(int resolution)
    {
        ClearTrajectoryPoints(); // Clear existing points before showing new ones

        Vector3 velocity = spawnPoint.forward * throwForce;
        float flightDuration = (2 * velocity.y) / Physics.gravity.y;
        float stepTime = flightDuration / resolution;

        for (int i = 0; i < resolution; i++)
        {
            float stepTimePassed = stepTime * i;
            Vector3 newPointPosition = spawnPoint.position + velocity * stepTimePassed + Physics.gravity * stepTimePassed * stepTimePassed / 2f;
            GameObject point = Instantiate(trajectoryPointPrefab, newPointPosition, Quaternion.identity);
            trajectoryPoints.Add(point);
        }
    }

    void ClearTrajectoryPoints()
    {
        foreach (GameObject point in trajectoryPoints)
        {
            Destroy(point);
        }
        trajectoryPoints.Clear();
    }
}
