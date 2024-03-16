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
    private PlayerNavMesh navmesh;
    // Reference to the player's CharacterController
    private CharacterController characterController;
    public GameObject bikeCamera;
    private Transform playerSeat;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        navmesh= GetComponent<PlayerNavMesh>();
    }

    public void AttachToBike(Transform bike, Transform seat)
    {
        isAttachedToBike = true;
        bikeTransform = bike;
        playerSeat = seat;

        // Disable the CharacterController component to stop player movement and collisions
        if (characterController != null) characterController.enabled = false;

        // Disable the collider specified by playerCollider, if any
        if (playerCollider != null)
        {
            var collider = playerCollider.GetComponent<Collider>();
            if (collider != null) collider.enabled = false;
        }

        navmesh.pointA = bike.gameObject.transform;
        navmesh.SetAnimState("drive");
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

        navmesh.pointA =  gameObject.transform;
        navmesh.SetAnimState("detach");
    }

    void LateUpdate()
    {
        if (isAttachedToBike && bikeTransform != null)
        {
            // Directly match the player's position with the bike's position
            transform.position = playerSeat.position;

            // Make the player look in the same direction as the bike camera
            transform.rotation = Quaternion.LookRotation(bikeTransform.transform.forward);
            transform.rotation = bikeTransform.rotation;
        }
    }
}
