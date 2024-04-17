using System.Collections;
using System.Collections.Generic;
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

    private AudioSource audioSource;

    public AudioClip[] audioClips;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource= GetComponent<AudioSource>();
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
        Vector3 forward = transform.forward;
        Vector3 toPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(forward, toPlayer);
        bool withinDetectionCone = angleToPlayer < detectionAngle;

        // Check distance and angle to player to see if within "detection cone"
        if (Vector3.Distance(transform.position, player.position) <= detectionRange && withinDetectionCone)
        {
            // Raycast to confirm line-of-sight
            RaycastHit hit;
            if (Physics.Raycast(transform.position, toPlayer, out hit, detectionRange))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    currentState = DroneState.Detected;
                    detected = true;

                    Debug.Log("DETECTED LMAOOOO");
                }
            }

            Color debugColor = Color.white;
            // Debugging: Draw the line of sight in green if player is detected, red otherwise
            Debug.DrawLine(transform.position, player.position, detected ? Color.green : Color.red);

            Debug.DrawLine(transform.position, player.position, detected ? debugColor = Color.green : debugColor = Color.red);

            if(debugColor == Color.red)
            {
                currentState = DroneState.Detected;
                detected = true;

                Debug.Log("DETECTED LMAOOOO");
            }

        }

        // Debugging: Draw the detection cone
        DrawDetectionCone();
    }

    void DrawDetectionCone()
    {
        float totalFOV = detectionAngle;
        float rayRange = detectionRange;
        float halfFOV = totalFOV / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Debug.DrawRay(transform.position, leftRayDirection * rayRange, Color.blue);
        Debug.DrawRay(transform.position, rightRayDirection * rayRange, Color.blue);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        float totalFOV = detectionAngle;
        float rayRange = detectionRange;
        float halfFOV = totalFOV / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);
    }


    void ChasePlayer()
    {
        audioSource.clip = audioClips[0];

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        agent.SetDestination(player.position);
        fallbackEffect.SetActive(false);
        detectedEffect.SetActive(true);
    }

    void Fallback()
    {
        agent.SetDestination(fallbackPoint.position);

        fallbackEffect.SetActive(true);
        detectedEffect.SetActive(false);

        audioSource.clip = audioClips[1];

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        if (Vector3.Distance(transform.position, fallbackPoint.position) < agent.stoppingDistance)
        {
            // Optionally reset to Idle or maintain Fallback behavior
            // currentState = DroneState.Idle;
            // Start Couroutine  set to idel/ repair
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
        switch (currentState)
        {
            case DroneState.Idle:
                spotlight.color = spotlights[0];
                screen.GetComponent<Renderer>().material = screenMAT[0];
                break;

            case DroneState.Detected:
                spotlight.color = spotlights[1];
                screen.GetComponent<Renderer>().material = screenMAT[1];
                break;

            case DroneState.Fallback:
                spotlight.color = spotlights[2];
                screen.GetComponent<Renderer>().material = screenMAT[2];
                break;
        }

        if (detected)
        {

        }
        else if (fallback)
        {


        }
        else 
        {

        }

    }
}
