using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableGravaity : MonoBehaviour
{

    public GameObject endPanel;
    public Image endImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has a Rigidbody component
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();



        // If Rigidbody component exists, disable gravity
        if (rb != null && collision.gameObject.CompareTag("Player"))
        {
            rb.useGravity = false;
            Debug.Log("Player entered");
            endPanel.SetActive(true);
            StartCoroutine(FadeInImage(5));  // Fade in over 3 seconds
        }
    }

    IEnumerator FadeInImage(float duration)
    {
        float currentTime = 0;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(0, 1, currentTime / duration);
            SetImageAlpha(alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        SetImageAlpha(1);  // Ensure it ends up fully visible
    }

    void SetImageAlpha(float alpha)
    {
        Color color = endImage.color;
        color.a = alpha;
        endImage.color = color;
    }
}
