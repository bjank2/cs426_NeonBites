using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateShoes : MonoBehaviour
{
    public GameObject itemNearTxt;

    public GameObject shoesOnPlayer;
    public GameObject shoesOnPlayer2;

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

        if (other.gameObject.tag == "Player")
        {
            itemNearTxt.SetActive(true);
            shoesOnPlayer.SetActive(true);
            shoesOnPlayer2.SetActive(true);
            Destroy(itemNearTxt, 3f);
            Destroy(gameObject, 3f);
            

        }


    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {

        }

    }
}
