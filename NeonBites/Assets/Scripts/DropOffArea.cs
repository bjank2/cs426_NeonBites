using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 // This script is attached to the DropOffArea GameObject, the object needs to have two colliders
 // one with trigger and one without. 
public class DropOffArea : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI TMP_Money;
    public TMPro.TextMeshProUGUI DeliveryStatusTMP;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickupObject"))
        {
            MoneyManager.Instance.AddMoney(50);
            TMP_Money.text = "$" +  MoneyManager.Instance.Money.ToString();
            if (DeliveryStatusTMP != null) DeliveryStatusTMP.text = "Delivery Status: Complete";
            Destroy(other.gameObject);
            Destroy(transform.parent.gameObject);
        }
    }
}
