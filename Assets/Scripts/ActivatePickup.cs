using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivatePickup : MonoBehaviour
{
    public bool isthis = false;
    public GameObject itemNearTxt;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isthis)
        {
            if (Input.GetKey(KeyCode.E))
            {
                itemNearTxt.SetActive(false);
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        isthis = true;

        if(other.gameObject.tag == "Player")
        {
            itemNearTxt.SetActive(true);

            // Start the coroutine to deactivate the GameObject after the specified delay
            StartCoroutine(DeactivateCoroutine());

        }


    }
    private void OnTriggerExit(Collider other)
    {
        isthis = false;

        if (other.gameObject.tag == "Player")
        {

            itemNearTxt.SetActive(false);
        }

    }

    IEnumerator DeactivateCoroutine()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(2f);

        // Deactivate the GameObject
        itemNearTxt.SetActive(false);
    }

}
