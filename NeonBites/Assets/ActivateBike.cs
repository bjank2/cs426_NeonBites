using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VehicleBehaviour;
using VehicleBehaviour.Utils;

public class ActivateBike : MonoBehaviour
{
    public GameObject interactUI; // A UI element that says "Press E to Ride"
    public GameObject bikeCamera;
    public GameObject bike;
    public GameObject playerSeat;
    private CameraFollow bikeCameraFollow;
    public GameObject playerCamera;
    public GameObject playerTP;
    public GameObject packagePlace;
    public float crashAngle = 50f;

    private bool bikeMode = false;
    private bool insideSphere = false;


    // Start is called before the first frame update
    void Start()
    {
        bikeCameraFollow = bikeCamera.GetComponent<CameraFollow>();

    }

    // Update is called once per frame
    void Update()
    {
        if(insideSphere && !bikeMode)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Switch2Bike();
            }
        }

        if(bikeMode)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Switch2Player();
            }
        }

        if (bikeMode && CheckForCrash())
        {
            Debug.Log("Crashing...");
            Crash();
        }
    }

    private void Switch2Bike()
    {

        bikeMode = true;

        IgnoreRagdollCollisions(playerTP, bike, true);
        // Calculate the relative position and rotation from the bike to the player camera
        Vector3 relativePosition = bike.transform.InverseTransformPoint(playerCamera.transform.position);
        Quaternion relativeRotation = Quaternion.Inverse(bike.transform.rotation) * playerCamera.transform.rotation;

        // Set the bike camera's local position and rotation to match the player camera's, adjusted relative to the bike
        bikeCamera.transform.localPosition = relativePosition;
        bikeCamera.transform.localRotation = relativeRotation;

        bikeCamera.SetActive(true);
        bikeCamera.GetComponent<AudioListener>().enabled = true;
        bike.GetComponent<WheelVehicle>().IsPlayer = true;
        bikeCamera.GetComponent<CameraFollow>().SetTargetIndex(0);

        interactUI.SetActive(false);

        playerCamera.SetActive(false);
        playerCamera.GetComponent<AudioListener>().enabled = false;
        playerTP.GetComponent<ThirdPersonController>().enabled = false;

        // Parent the player to the bike
        playerTP.GetComponent<Attach2Bike>().AttachToBike(bike.transform ,playerSeat.transform);

        //Transform package to seat
        playerTP.GetComponent<PlayerNavMesh>().TranslatePackage(packagePlace);


    }

    private void Switch2Player()
    {
        bikeMode = false;


        // Calculate the relative position and rotation from the bike to the player camera
        Vector3 relativePosition = playerCamera.transform.InverseTransformPoint(bike.transform.position);
        Quaternion relativeRotation = Quaternion.Inverse(playerCamera.transform.rotation) * bikeCamera.transform.rotation;

        // Set the bike camera's local position and rotation to match the player camera's, adjusted relative to the bike
        playerCamera.transform.localPosition = relativePosition;
        playerCamera.transform.localRotation = relativeRotation;

        playerCamera.SetActive(true);
        playerCamera.GetComponent<AudioListener>().enabled = true;
        playerTP.GetComponent<ThirdPersonController>().enabled = true;

        bikeCamera.SetActive(false) ;
        bikeCamera.GetComponent<AudioListener>().enabled = false;
        bike.GetComponent<WheelVehicle>().IsPlayer = false;
        bikeCamera.GetComponent<CameraFollow>().SetTargetIndex(default);

        // Unparent the player and reset its position if needed
        playerTP.GetComponent<Attach2Bike>().DetachFromBike();

        //Transform package to holdpoint
        playerTP.GetComponent<PlayerNavMesh>().RestorePackageParent();


        IgnoreRagdollCollisions(playerTP, bike, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the player GameObject has the tag "Player"
        {
            insideSphere = true;

            interactUI.SetActive(true); // Show the UI prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            insideSphere = false;

            interactUI.SetActive(false); // Hide the UI prompt
        }
    }

    void IgnoreRagdollCollisions(GameObject player, GameObject bike, bool avoid)
    {
        Collider[] playerColliders = player.GetComponentsInChildren<Collider>();
        Collider[] bikeColliders = bike.GetComponentsInChildren<Collider>();

        foreach (Collider playerCol in playerColliders)
        {
            foreach (Collider bikeCol in bikeColliders)
            {
                Physics.IgnoreCollision(playerCol, bikeCol, avoid);
            }
        }
    }

    private bool CheckForCrash()
    {
        // Get the absolute value of the z-axis rotation
        float zRotation = Mathf.Abs(bike.transform.eulerAngles.z);

        // Normalize the value to the range of 0-180
        if (zRotation > 180)
            zRotation = 360 - zRotation;

        // Check if the zRotation exceeds 50 degrees
        return zRotation > crashAngle;
    }

    private void Crash()
    {
        bikeMode = false;
        bikeCamera.GetComponent<CameraFollow>().SetTargetIndex(2);
        // Unparent the player and reset its position if needed
        playerTP.GetComponent<Attach2Bike>().DetachFromBike();

        // Call function to switch to player control
        //Switch2Player();

        playerTP.GetComponent<Animator>().SetTrigger("Crash");

        // Translate the player above the bike
        Vector3 newPosition = bike.transform.position + Vector3.up * 10f; // Adjust the 3f to whatever height you want above the bike
        StartCoroutine(SmoothTranslate(playerTP.transform, newPosition, 0.5f)); // Smooth translate over half a second


    }

    private void ApplyForceToRagdoll(GameObject player, Vector3 direction, float force)
    {
        Rigidbody[] rigidbodies = player.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            // Apply a force to the Rigidbody components of the ragdoll
            rb.AddForce(direction * force, ForceMode.VelocityChange);
        }
    }

    private IEnumerator WaitForRagdollToSettle()
    {
        // Wait for the physics to simulate for a few frames to get accurate readings (optional)
        yield return new WaitForFixedUpdate();

        // Initialize a variable to track whether the player has settled
        bool ragdollSettled = false;
        while (!ragdollSettled)
        {
            // Assume the ragdoll has settled
            ragdollSettled = true;

            // Check all rigidbodies to see if they've come to a rest
            Rigidbody[] ragdollRigidbodies = playerTP.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in ragdollRigidbodies)
            {
                // If any rigidbody is still moving above a certain velocity threshold, the ragdoll has not settled
                if (rb.velocity.sqrMagnitude > 0.1f) // Use a squared magnitude to avoid square root calculation for efficiency
                {
                    ragdollSettled = false;
                    break; // Exit the loop as we found a Rigidbody that's still moving
                }
            }

            // Wait until the next fixed frame to recheck velocities
            yield return new WaitForFixedUpdate();
        }

        // Once settled, re-enable the Animator and CharacterController
        Animator playerAnimator = playerTP.GetComponent<Animator>();
        if (playerAnimator != null)
        {
            playerAnimator.enabled = true;
        }

        CharacterController characterController = playerTP.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = true;
        }

        playerAnimator.SetTrigger("Stand");
        Switch2Player();
        

    }

    private IEnumerator SmoothTranslate(Transform objectToMove, Vector3 targetPosition, float duration)
    {
        // Wait a short moment before starting the translation to simulate a smooth start
        yield return new WaitForSeconds(0.05f);

        // Move to the target position
        objectToMove.position = targetPosition;

        // Wait for the physics to simulate for a few frames to get accurate readings (optional)
        yield return new WaitForSeconds(duration);

        // Disable the player's Animator and ThirdPersonController (or any similar script controlling the player's movement)
        playerTP.GetComponent<Animator>().enabled = false;

        playerTP.GetComponent<CharacterController>().enabled = false;

        // Enable the ragdoll by enabling the Rigidbody components and disabling the CharacterController
        //EnableRagdoll(playerTP);

        // Start the coroutine to check for the ragdoll stopping
        StartCoroutine(WaitForRagdollToSettle());
    }
}
