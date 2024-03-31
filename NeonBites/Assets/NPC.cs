using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inworld.UI;
using System;

public class NPC : MonoBehaviour
{

    public string[] acceptablePhrases;
    public string npcName;
    private bool phraseMatched = false; // Flag to indicate a phrase has been matched

    public GameObject item;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize things here if needed
    }

    // Update is called once per frame
    void Update()
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
}
