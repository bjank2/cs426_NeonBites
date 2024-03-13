using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This script is attached to the DropOffArea GameObject, the object needs to have two colliders
// one with trigger and one without. 
public class DropOffArea : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI TMP_Money;

    public TMPro.TextMeshProUGUI DeliveryStatusTMP;
    public int requiredPickupID;
    private void OnTriggerEnter(Collider other)
    {
                    Debug.Log("Object entered drop off area");

        if (other.CompareTag("PickupObject"))
        {
            PickupObject pickupObject = other.GetComponent<PickupObject>();

            if (pickupObject != null && pickupObject.pickupID == requiredPickupID)
            {
                Debug.Log("Correct object delivered");
                MoneyManager.Instance.AddMoney(50);
                TMP_Money.text = "$" + MoneyManager.Instance.Money.ToString();
                if (DeliveryStatusTMP != null) DeliveryStatusTMP.text = "Delivery Status: Complete";
                Destroy(transform.parent.gameObject);
            }

            else
            {
                Debug.Log("Wrong object delivered");
                MoneyManager.Instance.AddMoney(-20);
                TMP_Money.text = "$" + MoneyManager.Instance.Money.ToString();
                if (DeliveryStatusTMP != null) DeliveryStatusTMP.text = "Delivery Status: Failed";
            }

            Destroy(other.gameObject);
        }
    }
}