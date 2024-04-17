using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This script is attached to the DropOffArea GameObject, the object needs to have two colliders
// one with trigger and one without. 
public class DropOffArea : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI TMP_Money;

    public TMPro.TextMeshProUGUI DeliveryStatusTMP;
    public TMPro.TextMeshProUGUI EnemyHealth;
    public GameObject EnemyTargetPrefab;
    public int requiredPickupID;
    public PlayerNavMesh navmesh;

    public TMPro.TextMeshProUGUI deleteButton;
    public TMPro.TextMeshProUGUI currentTaskTxt;


    private void Start()
    {
        navmesh = FindObjectOfType<PlayerNavMesh>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object entered drop off area");

        if (other.CompareTag("PickupObject"))
        {
            PickupObject pickupObject = other.GetComponent<PickupObject>();
            Debug.Log("pickupObject.pickupID: " + pickupObject.pickupID);
            Debug.Log("requiredPickupID: " + requiredPickupID);
            if (pickupObject != null && pickupObject.pickupID == requiredPickupID)
            {
                currentTaskTxt.text = "Select new order from tab";

                Debug.Log("Correct object delivered");
                MoneyManager.Instance.AddMoney(50);
                TMP_Money.text = "$" + MoneyManager.Instance.Money.ToString();
                if (DeliveryStatusTMP != null) DeliveryStatusTMP.text = "Delivery Status: Complete";
                Enemy enemyScript = FindObjectOfType<Enemy>();
                enemyScript.Wave();
                Destroy(transform.parent.gameObject);


            }

            else
            {
                if(deleteButton != null)
                {

                    deleteButton.gameObject.SetActive(false);
                }

                Debug.Log("Wrong object delivered");
                TMP_Money.text = "$" + MoneyManager.Instance.Money.ToString();
                if (DeliveryStatusTMP != null) DeliveryStatusTMP.text = "Delivery Status: Failed";
                // Vector3 spawnPoint = new Vector3(transform.parent.position.x, 0f, transform.parent.position.z);

                Enemy enemyScript = FindObjectOfType<Enemy>();
                enemyScript.EnableAttacking();
                Destroy(transform.parent.gameObject);

            }

            navmesh.routeAssigned = false;
            Destroy(other.gameObject);


        }
    }
}