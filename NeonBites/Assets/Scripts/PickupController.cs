using UnityEngine;
using UnityEngine.InputSystem;

public class PickupController : MonoBehaviour
{
    public Transform holdPoint;
    private GameObject currentPickedObject = null;
    public Collider PickupCollider;
    public TMPro.TextMeshProUGUI DeliveryStatusTMP;
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
    }
}
