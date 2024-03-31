using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    public enum DroneState
    {
        Idle,
        Detected,
        Fallback
    }


    public NavMeshAgent agent;
    public float range; // Radius of sphere for idle movement
    public Light spotlight;
    public Color[] spotlights; // Color for spotlight [0] for idle/detected, [1] for fallback
    public Material[] screenMAT; // Screen materials [0] for idle/detected, [1] for fallback
    public Transform centrePoint; // Centre of the area the agent wants to move around in
    public GameObject screen;
    public Transform player; // Assign the player's transform in the Inspector
    public Transform fallbackPoint; // Location to recharge/call for backup
    public float detectionRange = 10f; // Range to detect the player
    public float detectionAngle = 30f; // Angle for detection cone

    private DroneState currentState = DroneState.Idle;
    private bool playerInRange = false;
    public bool detected = false;
    public bool fallback = false;

    public GameObject fallbackEffect;
    public GameObject detectedEffect;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        switch (currentState)
        {
            case DroneState.Idle:
                Patrol();
                DetectPlayer();
                break;
            case DroneState.Detected:
                ChasePlayer();
                break;
            case DroneState.Fallback:
                Fallback();
                break;
        }

        Changeheadlight();
    }

    void Patrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point))
            {
                agent.SetDestination(point);
            }
        }
    }

    void DetectPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(directionToPlayer, transform.forward);

        if (directionToPlayer.magnitude < detectionRange && angleToPlayer < detectionAngle)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, detectionRange))
            {
                if (hit.transform == player)
                {
                    currentState = DroneState.Detected;
                    detected = true; // Assuming 'detected' changes visual/audio cues
                }
            }
        }
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        fallbackEffect.SetActive(false);
        detectedEffect.SetActive(true);
    }

    void Fallback()
    {
        agent.SetDestination(fallbackPoint.position);

        fallbackEffect.SetActive(true);
        detectedEffect.SetActive(false);

        if (Vector3.Distance(transform.position, fallbackPoint.position) < agent.stoppingDistance)
        {
            // Optionally reset to Idle or maintain Fallback behavior
            // currentState = DroneState.Idle;

        }
    }

    void UpdateVisuals()
    {
        // Update light and screen based on state
        int index = currentState == DroneState.Fallback ? 1 : 0;
        spotlight.color = spotlights[index];
        screen.GetComponent<Renderer>().material = screenMAT[index];
    }

    void OnTriggerEnter(Collider other)
    {
        // Check for grenade explosion
        if (other.CompareTag("Grenade"))
        {
            currentState = DroneState.Fallback;
            fallback = true;
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    public void Changeheadlight()
    {
        if (detected)
        {
            spotlight.color = spotlights[1];
            screen.GetComponent<Renderer>().material = screenMAT[1];
        }
        else if (fallback)
        {
            spotlight.color = spotlights[2];
            screen.GetComponent<Renderer>().material = screenMAT[2];

        }
        else 
        {
            spotlight.color = spotlights[0];

            screen.GetComponent<Renderer>().material = screenMAT[0];
        }

    }
}
