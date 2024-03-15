using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePickup : MonoBehaviour
{
    public bool isthis = false;

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
    }
    private void OnTriggerExit(Collider other)
    {
        isthis = false;
    }

}
