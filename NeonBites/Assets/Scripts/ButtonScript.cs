using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonScript : MonoBehaviour
{
    public GameObject background; // Reference to the background GameObject
    public TextMeshProUGUI deliveryTimeText; // Reference to the text displaying delivery time
    public TextMeshProUGUI moneyText; // Reference to the text displaying potential money guaranteed
    public List<ButtonScript> otherButtons; // Reference to other ButtonScripts in the group
    private Button button; // Reference to the Button component
    private bool isSelected = false; // Flag to track the selected state
    public int deliveryTime; // Estimated time for delivery in minutes
    public int potentialMoney; // Potential money guaranteed

    void Start()
    {
        // Get the Button component attached to this GameObject
        button = GetComponent<Button>();

        if(deliveryTime != -1)
        {
            // Add a listener for the button click event
            button.onClick.AddListener(OnClick);
        }

        // Update the UI text with initial values
        UpdateUIText();
    }

    // Function to handle the button click event
    void OnClick()
    {
        // Check if this button is already selected
        if (isSelected)
        {
            return; // Exit if already selected
        }

        // Toggle the isSelected flag
        isSelected = true;

        // Set the interactable state of the button to false
        button.interactable = false;

        // Enable or disable the background GameObject based on the selected state
        background.SetActive(true);

        // Deactivate other buttons in the group
        foreach (ButtonScript otherButton in otherButtons)
        {
            if (otherButton != this)
            {
                otherButton.Deselect();
            }
        }

        // Update the UI text with selected values
        UpdateUIText();
    }

    // Function to deselect the button
    public void Deselect()
    {
        isSelected = false;
        button.interactable = true;
        background.SetActive(false);
    }

    // Function to update the UI text with delivery time and potential money
    private void UpdateUIText()
    {
        if (isSelected)
        {
            deliveryTimeText.text = deliveryTime + " mins";
            moneyText.text = "$" + potentialMoney;
        }
        else
        {
            deliveryTimeText.text = "";
            moneyText.text = "";
        }
    }

    public void ActivateSelection()
    {
        Debug.Log("Track selected. Money: "+ moneyText.text + "  time: "+ deliveryTimeText.text);
    }

}
