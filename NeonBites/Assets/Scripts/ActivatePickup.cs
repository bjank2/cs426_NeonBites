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
        
    }

    private void OnTriggerEnter(Collider other)
    {
        isthis = true;

        if(other.gameObject.tag == "Player")
        {

            itemNearTxt.SetActive(true);
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

}
