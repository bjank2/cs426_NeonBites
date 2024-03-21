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
    public float crashAngle = 75f;

    private bool bikeMode = false;
    private bool insideSphere = false;

    public float ragdollSettleTime = 3f; // Time to wait for ragdoll to settle
    public GameObject speedometer;

    public GameObject minimap_camera;

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
            //Crash();
        }
    }

    private void Switch2Bike()
    {

        bikeMode = true;

        //IgnoreRagdollCollisions(playerTP, bike, true);
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

        speedometer.SetActive(true);

        minimap_camera.GetComponent<Camera>().orthographicSize = 28;
        minimap_camera.GetComponent<Minimap>().bikeVisible = false;
        minimap_camera.GetComponent<Minimap>().mainCamera = bikeCamera.GetComponent<Camera>();
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

        bike.GetComponent<WheelVehicle>().IsPlayer = false;

        playerCamera.SetActive(true);
        playerCamera.GetComponent<AudioListener>().enabled = true;
        playerTP.GetComponent<ThirdPersonController>().enabled = true;

        bikeCamera.SetActive(false) ;
        bikeCamera.GetComponent<AudioListener>().enabled = false;
        
        bikeCamera.GetComponent<CameraFollow>().SetTargetIndex(3);

        // Unparent the player and reset its position if needed
        playerTP.GetComponent<Attach2Bike>().DetachFromBike(false);

        //Transform package to holdpoint
        playerTP.GetComponent<PlayerNavMesh>().RestorePackageParent();

        playerTP.GetComponent<Animator>().SetBool("Crashed", false);
        //IgnoreRagdollCollisions(playerTP, bike, false);

        speedometer.SetActive(false);

        minimap_camera.GetComponent<Camera>().orthographicSize = 18;
        minimap_camera.GetComponent<Minimap>().bikeVisible = true;

        minimap_camera.GetComponent<Minimap>().mainCamera = playerCamera.GetComponent<Camera>();
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
        playerTP.GetComponent<Animator>().SetBool("Crashed", true);

        bikeMode = false;
        bikeCamera.GetComponent<CameraFollow>().SetTargetIndex(2);

        // Unparent the player and reset its position if needed
        playerTP.GetComponent<Attach2Bike>().DetachFromBike(true);

        // Disable the player's Animator and CharacterController (or any similar script controlling the player's movement)
        playerTP.GetComponent<Animator>().enabled = false;
        playerTP.GetComponent<CharacterController>().enabled = false;

        // Translate the player above the bike using SmoothTranslate
        Vector3 newPosition = bike.transform.position + Vector3.up * 10f; // Adjust the height as needed
        StartCoroutine(SmoothTranslate(playerTP.transform, newPosition, 0.2f)); // Smooth translate over 0.2 seconds
    }

    private IEnumerator SmoothTranslate(Transform objectToMove, Vector3 targetPosition, float duration)
    {
        // Wait a short moment before starting the translation to simulate a smooth start
        yield return new WaitForSeconds(0.05f);

        // Move to the target position
        objectToMove.position = targetPosition;

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Start the coroutine to check for the ragdoll stopping
        StartCoroutine(WaitForRagdollToSettle());
    }

    private IEnumerator WaitForRagdollToSettle()
    {
        // Wait for the ragdoll settle time
        yield return new WaitForSecondsRealtime(ragdollSettleTime);

        // Re-enable the Animator and CharacterController
        Animator playerAnimator = playerTP.GetComponent<Animator>();
        if (playerAnimator != null)
        {
            playerAnimator.enabled = true;
            playerAnimator.SetBool("Crashed", false);
            playerAnimator.SetTrigger("Stand");
        }

        CharacterController characterController = playerTP.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = true;
        }

        // Switch back to player control
        Switch2Player();
    }
}
