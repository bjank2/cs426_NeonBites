using UnityEngine;
using UnityEngine.InputSystem;

public class PickupController : MonoBehaviour
{
    public Transform holdPoint;
    private GameObject currentPickedObject = null;


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
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

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
