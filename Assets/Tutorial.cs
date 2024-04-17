using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public GameObject mainPanel;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public GameObject moveInstructions;
    public GameObject lookInstructions;

    public GameObject orderInstructions;
    public GameObject GameUI;


    public GameObject mapInstructions;

    private CanvasGroup mainPanelCanvasGroup;

    private int tutorialStep = 0;
    private Vector3 offScreenPosition;

    void Start()
    {
        //Time.timeScale = 0; // Pause the game
        Cursor.visible = true; // Make cursor visible
        Cursor.lockState = CursorLockMode.None; // Free the cursor

        mainPanel.SetActive(true);
        text1.gameObject.SetActive(true);
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(false);

        offScreenPosition = new Vector3(mainPanel.transform.position.x, mainPanel.transform.position.y - 1500, mainPanel.transform.position.z);  // Set off-screen position


        StartCoroutine(DisplayTextAfterDelay(text2, 1)); // Display second text after 3 seconds
        StartCoroutine(DisplayTextAfterDelay(text3, 6)); // Display third text after 11 seconds total
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceTutorial();
        }
    }

    private void AdvanceTutorial()
    {
        tutorialStep++;
        switch (tutorialStep)
        {
            case 1:
                // First press: move the main panel downwards and resume the game
                StartCoroutine(MovePanelDownwards());
                break;
            case 2:
                // Second press: show game control instructions
                moveInstructions.SetActive(true);
                lookInstructions.SetActive(true);
                break;
            case 3:
                // Third press: end the tutorial and hide instructions
                moveInstructions.SetActive(false);
                lookInstructions.SetActive(false);
                orderInstructions.SetActive(true);
                GameUI.SetActive(true);
                this.enabled = false; // Optionally disable the script if the tutorial is over
                break;
        }
    }

    IEnumerator DisplayTextAfterDelay(TextMeshProUGUI textElement, float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // Use WaitForSecondsRealtime since timeScale is 0
        textElement.gameObject.SetActive(true);
    }

    IEnumerator MovePanelDownwards()
    {
        float duration = 1.0f;
        float elapsed = 0f;
        Vector3 startPosition = mainPanel.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaled delta time since the game is paused
            float t = elapsed / duration;
            t = Mathf.SmoothStep(0, 1, t);  // Use SmoothStep for a smoother animation

            mainPanel.transform.position = Vector3.Lerp(startPosition, offScreenPosition, t);
            yield return null;
        }

        mainPanel.SetActive(false);
        Time.timeScale = 1; // Resume game time
        Cursor.visible = false; // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
    }

    public void EnableGameObj(GameObject gooo)
    {
        if(gooo.activeSelf)
        {

            gooo.SetActive(false);
        }
        else gooo.SetActive(true);
    }

    public void DestroyThis()
    {
        
    }
}
