using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonScript : MonoBehaviour
{

    public GameObject background;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackgroundddAct()
    {
        if (background.activeSelf)
        {
            background.SetActive(false);
        }
        else
        {
            background.SetActive(true);
        }
    }


}
