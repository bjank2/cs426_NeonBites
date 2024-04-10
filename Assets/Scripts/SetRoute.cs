using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRoute : MonoBehaviour
{
    public PlayerNavMesh navMesh;
    public Transform destination;

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
        if(other.gameObject.tag == "Player")
        {
            //AssignRoute();
        }
    }

    public void AssignRoute()
    {
        navMesh.AssignRoute(gameObject.transform, destination);
        Debug.Log("Assign route called");
    }
}
