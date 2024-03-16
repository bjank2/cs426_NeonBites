using StarterAssets;
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
        if(insideSphere)
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
    }

    private void Switch2Bike()
    {

        bikeMode = true;


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
        bikeCamera.GetComponent<CameraFollow>().SetTargetIndex(1);

        // Unparent the player and reset its position if needed
        playerTP.GetComponent<Attach2Bike>().DetachFromBike();

        //Transform package to holdpoint
        playerTP.GetComponent<PlayerNavMesh>().RestorePackageParent();
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
}
