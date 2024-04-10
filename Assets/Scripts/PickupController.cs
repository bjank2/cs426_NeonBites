using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PickupController : MonoBehaviour
{
    public Transform holdPoint;
    private GameObject currentPickedObject = null;
    public Collider PickupCollider;
    public TMPro.TextMeshProUGUI DeliveryStatusTMP;

    public SetRoute setRoute;
    public PlayerNavMesh navMesh;
    public Transform destination;

    public ActivatePickup actPickup;

    private void Start()
    {
        setRoute = GetComponent<SetRoute>();
        navMesh = FindObjectOfType<PlayerNavMesh>();
    }
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (currentPickedObject == null)
            {
                TryPickupObject();
            }
            else
            {
                DropObject();
            }
        }
    }
    private void TryPickupObject()
    {
        Collider[] colliders = Physics.OverlapBox(PickupCollider.bounds.center, PickupCollider.bounds.extents, PickupCollider.transform.rotation);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("PickupObject") && actPickup.isthis)
            {
                PickupObject(collider.gameObject);

                break;
            }
        }
    }

    private void PickupObject(GameObject obj)
    { 
        currentPickedObject = obj;
        currentPickedObject.transform.SetParent(holdPoint);
        currentPickedObject.transform.localPosition = Vector3.zero;
        var rb = currentPickedObject.GetComponent<Rigidbody>();
        PickupObject pickupComponent = currentPickedObject.GetComponent<PickupObject>();
        if (pickupComponent != null)
        {
            Debug.Log("Picked up object with ID: " + pickupComponent.pickupID);

            //setRoute.AssignRoute(); // function called to assign roue on mimimap
            Debug.Log(destination.name + " destination gameobject name  ");
            navMesh.AssignRoute(gameObject.transform, destination);
            navMesh.AssignPackage(currentPickedObject);
            //navMesh.SetAnimState("pickup");

        }
        if (rb != null) rb.isKinematic = true;
        //currentPickedObject.GetComponent<BoxCollider>().enabled = false;
        if (DeliveryStatusTMP != null) DeliveryStatusTMP.text = "Status: In Progress";

    }

    private void DropObject()
    {
        if (currentPickedObject == null) return;
        var rb = currentPickedObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;
        currentPickedObject.transform.SetParent(null);
        currentPickedObject.GetComponent<BoxCollider>().enabled = true;
        currentPickedObject = null;
        navMesh.currentPackage = null; 

        navMesh.routeAssigned = false; // When we drop object, we disable the route on minimap

    }

}
