using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inworld.UI;
using System;
using Inworld.Interactions;
using TMPro;

public class NPC : MonoBehaviour
{

    public string[] acceptablePhrases;
    public string npcName;
    private bool phraseMatched = false; // Flag to indicate a phrase has been matched

    public InworldAudioInteraction iai;
    public GameObject item;
    public GameObject promptTxt;

    public bool isPlayerNear = false;
    public bool isConversationActive = false;

    public Camera mainCamera;     // The main player camera
    public Camera npcCamera;      // The camera attached to the NPC

    public GameObject player;

    public GameObject ExtraCanvas;
    public GameObject gameCanvas;

    public GameObject itemRecTxt;

    public AudioSource audiosrc;
    public AudioClip openConv;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize things here if needed
        iai = GetComponent<InworldAudioInteraction>();
        iai.enabled = false;

        audiosrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isPlayerNear)
        {
            // Display prompt, e.g., using GUI or a UI Text element
            // Debug.Log("Press E to talk");

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isConversationActive)
                {
                    StartConversation();
                }
                else
                {
                    EndConversation();
                }
            }
        }

        MatchPhase();

    }

    private void MatchPhase()
    {
        // If a phrase has been matched, do nothing
        if (phraseMatched) return;


        ChatBubble[] chatBubbles = GetComponentsInChildren<ChatBubble>();

        // Iterate through each ChatBubble and check for matching phrases
        // Iterate through each ChatBubble and check for matching phrases
        foreach (ChatBubble chatBubble in chatBubbles)
        {
            // Ensure the ChatBubble belongs to this NPC and the title is case-insensitively equal to npcName
            if (string.Equals(chatBubble.Title.text, npcName, StringComparison.OrdinalIgnoreCase))
            {
                foreach (string phrase in acceptablePhrases)
                {
                    // Use IndexOf with StringComparison.OrdinalIgnoreCase for case-insensitive comparison
                    if (chatBubble.Text.IndexOf(phrase, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Debug.Log($"Phrase from {chatBubble.Title.text}: '{phrase}' found in ChatBubble: {chatBubble.Text}");
                        // Call the function here and set the flag
                        SomeFunction(phrase); // Replace with your actual function name
                        itemRecTxt.SetActive(true);
                        phraseMatched = true; // Set the flag to true as the phrase has been matched
                        return; // Exit the loop and method as we've found a match
                    }
                }
            }
        }
    }

    private void SomeFunction(string phrase)
    {
        // Handle the matched phrase
        Debug.Log($"Function called for matched phrase: {phrase}");
        item.SetActive(true);
    }

    public void ResetMatch()
    {
        // Call this method to reset the phraseMatched flag if you need to start matching phrases again
        phraseMatched = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            promptTxt.SetActive(true);
            StartCoroutine(DeactivateCoroutine());
            isPlayerNear = true;

            player = other.gameObject;

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            promptTxt.SetActive(false);
            isPlayerNear = false;

        }
    }

    IEnumerator DeactivateCoroutine()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(4f);

        // Deactivate the GameObject
        promptTxt.SetActive(false);
    }

    public GameObject instTxt;
    public GameObject instTxt2;

    void StartConversation()
    {
        npcCamera.enabled = true;
        mainCamera.enabled = false;
        isConversationActive = true;
        iai.enabled = true;

        player.SetActive(false);

        gameCanvas.SetActive(false);
        ExtraCanvas.SetActive(true);

        audiosrc.PlayOneShot(openConv);

        if (instTxt!= null)
        {
            instTxt.SetActive(false);
        }

    }

    void EndConversation()
    {
        npcCamera.enabled = false;
        mainCamera.enabled = true;
        isConversationActive = false;
        iai.enabled = false;


        player.SetActive(true);

        gameCanvas.SetActive(true);
        ExtraCanvas.SetActive(false);

        if (instTxt2 != null)
        {
            instTxt2.SetActive(true);
        }
    }
}
