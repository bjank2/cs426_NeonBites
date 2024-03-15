using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attach2Bike : MonoBehaviour
{
    public Transform bikeTransform; // Assign the bike's transform in the editor or from your bike script
    public bool isAttachedToBike = false;

    public GameObject playerCollider; 

    private Vector3 localPositionOffset; // The local position offset from the bike's transform
    private Quaternion localRotationOffset; // The local rotation offset from the bike's transform

    // Reference to the player's CharacterController
    private CharacterController characterController;
    public GameObject bikeCamera;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void AttachToBike(Transform bike)
    {
        isAttachedToBike = true;
        bikeTransform = bike;

        // Disable the CharacterController component to stop player movement and collisions
        if (characterController != null) characterController.enabled = false;

        // Disable the collider specified by playerCollider, if any
        if (playerCollider != null)
        {
            var collider = playerCollider.GetComponent<Collider>();
            if (collider != null) collider.enabled = false;
        }

    }

    public void DetachFromBike()
    {
        isAttachedToBike = false;
        bikeTransform = null;

        // Re-enable the CharacterController component
        if (characterController != null) characterController.enabled = true;

        // Re-enable the collider specified by playerCollider, if any
        if (playerCollider != null)
        {
            var collider = playerCollider.GetComponent<Collider>();
            if (collider != null) collider.enabled = true;
        }
    }

    void LateUpdate()
    {
        if (isAttachedToBike && bikeTransform != null)
        {
            // Directly match the player's position with the bike's position
            transform.position = bikeTransform.position;

            // Make the player look in the same direction as the bike camera
            transform.rotation = Quaternion.LookRotation(bikeCamera.transform.forward);
        }
    }
}
