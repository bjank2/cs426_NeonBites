using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    public Transform target; 
    public Transform player;

    void Update()
    {
        if (target != null)
        {
            Vector3 targetDirection = target.position - player.position;
            targetDirection.y = 0;  
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10);  
        }
    }
}