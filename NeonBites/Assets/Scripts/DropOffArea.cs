using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 // This script is attached to the DropOffArea GameObject, the object needs to have two colliders
 // one with trigger and one without. 
public class DropOffArea : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickupObject"))
        {
            Destroy(other.gameObject);
            Debug.Log("Order received.");
        }
    }
}
