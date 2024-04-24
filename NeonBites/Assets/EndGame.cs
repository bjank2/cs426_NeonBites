using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{

    public GameObject endPanel;
    public Image endImage;

    void Start()
    {
        // Ensure the end panel is initially disabled
        endPanel.SetActive(false);
        // Set the initial alpha of the image to 0
        SetImageAlpha(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered");
            endPanel.SetActive(true);
            StartCoroutine(FadeInImage(3));  // Fade in over 3 seconds
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
