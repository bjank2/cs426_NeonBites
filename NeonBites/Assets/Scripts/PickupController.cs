using UnityEngine;
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
            if (collider.CompareTag("PickupObject"))
            {
                PickupObject(collider.gameObject);

                setRoute.AssignRoute(); // function called to assign roue on mimimap

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
        }
        if (rb != null) rb.isKinematic = true;
        if (DeliveryStatusTMP != null) DeliveryStatusTMP.text = "Delivery Status: In Progress";

    }

    private void DropObject()
    {
        if (currentPickedObject == null) return;
        var rb = currentPickedObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;
        currentPickedObject.transform.SetParent(null);
        currentPickedObject = null;

        navMesh.routeAssigned = false; // When we drop object, we disable the route on minimap

    }

}
