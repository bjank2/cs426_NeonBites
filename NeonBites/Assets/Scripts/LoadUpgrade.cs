using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUpgrade : MonoBehaviour
{
    public Transform headTransform; // Assign this to the character's head transform in the inspector
    public Transform trophyTransform;
    public GameObject[] Masks_;
    public GameObject[] Trophy_;

    [System.Serializable]
    public class ShoePair
    {
        public GameObject leftShoe;
        public GameObject rightShoe;
    }

    public ShoePair[] ShoePairs_;

    void Start()
    {
        LoadAccessories();
    }


    private void Update()
    {
        
    }

    private void LoadAccessories()
    {
        // Load the selected mask index
        int selectedMaskIndex = PlayerPrefs.GetInt("SelectedMaskIndex", 0);
        int selectedTrophyIndex = PlayerPrefs.GetInt("SelectedTrophyIndex", 0);
        int selectedShoeIndex = PlayerPrefs.GetInt("SelectedShoeIndex", 0);

        // Load the selected mask prefab using the index
        GameObject maskPrefab = Resources.Load<GameObject>($"Masks/Mask{selectedMaskIndex + 1}"); // Adjust path and naming as necessary

        if (headTransform != null)
        {
            // Instantiate the mask prefab
            //GameObject maskInstance = Instantiate(maskPrefab, headTransform.position, Quaternion.identity, headTransform);

            Masks_[selectedMaskIndex].SetActive(true);

            // Optionally set local position and rotation if needed
            //maskInstance.transform.localPosition = Vector3.zero;
            //maskInstance.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogError("Mask prefab or head transform not found");
        }

        // BIKE TROPHY
        if (trophyTransform != null)
        {

            Trophy_[selectedTrophyIndex].SetActive(true);
        }
        else
        {
            Debug.LogError("Trophy prefab or head transform not found");
        }
        /*
        if (trophyTransform != null)
        {
            ShoePairs_[selectedShoeIndex].rightShoe.SetActive(true);
            ShoePairs_[selectedShoeIndex].leftShoe.SetActive(true);
        }
        else
        {
            Debug.LogError("Shoe prefab or head transform not found");
        }

        */
    }

}
