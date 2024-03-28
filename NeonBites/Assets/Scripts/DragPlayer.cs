using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DragPlayer : MonoBehaviour
{
    [System.Serializable]
    public class ShoePair
    {
        public GameObject leftShoe;
        public GameObject rightShoe;
    }

    public float rotationSpeed = 5f;
    public float rotationPlayer = 100.0f;
    public GameObject playerMesh;
    public GameObject bikeMesh;
    public GameObject platformMesh;
    public float zoomAmount = 5f; // How far the camera zooms in
    public float zoomSpeed = 2f; // How smoothly the camera zooms in and out
    public float zoombackSpeed = 4f;

    public GameObject[] masks; // Assign your masks in the inspector
    public GameObject[] trophies;
    public ShoePair[] shoePairs;

    private int currentMaskIndex = 0;
    private int currentTrophyIndex = 0;
    private int currentShoePairIndex = 0;

    public Animator playerAnimator; // Reference to the player's animator component
    private Vector3 originalPosition; // To store the camera's original position
    private bool isZooming = false; // To check if we're currently zooming
    private Vector3 targetPosition; // Target position for zoom based on current direction

    public GameObject target_rn; // The player GameObject to be rotated
    public Camera playerCamera; // The camera attached to the player
    public float rotationSensitivity = 1f; // Sensitivity of the rotation
    public float maxVerticalAngle = 20f; // Maximum vertical angle for camera rotation
    private float originalCameraRotationX; // Original X rotation of the camera
    private bool isRotating = false; // Tracks if the camera is currently rotating
    private float transitionSpeed = 5f; // Speed of the rotation transition when releasing the right mouse button

    private Vector3 originalCameraRotation; // Use Vector3 to store the original rotation of the camera
    private bool isRightMouseButtonPressed = false; // Tracks the state of the right mouse button

    public TextMeshProUGUI modetext;
    void Start()
    {
        originalPosition = transform.position; // Store the original position of the camera
        originalCameraRotation = playerCamera.transform.localEulerAngles; // Store the original rotation of the camera
        modetext.text = "VIEW MODE";
        target_rn = playerMesh;
    } 

    void Update()
    {
        // Toggle between viewing mode and cursor mode with the Escape key
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleCursorMode();
        }

        // If the cursor is not locked, return to avoid camera movement interference
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            return;
        }

        // Check for right mouse button press
        if (Input.GetMouseButtonDown(1))
        {
            isRightMouseButtonPressed = true;
        }

        // Check for right mouse button release
        if (Input.GetMouseButtonUp(1))
        {
            isRightMouseButtonPressed = false;
        }

        if (isRightMouseButtonPressed)
        {
            RotatePlayerAndCamera();
        }
        else
        {
            // Smoothly transition back to the original camera rotation when not rotating
            SmoothReturnToOriginalRotation();
        }

        if (Input.GetKey(KeyCode.X)) // X key for player rotation
        {
            platformMesh.transform.Rotate(Vector3.up, rotationPlayer * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
        {
            isZooming = true;
            targetPosition = transform.position + (transform.forward * zoomAmount);
        }

        if (Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            isZooming = false;
        }

        // Smooth zoom in towards target position or back to original position
        if (isZooming)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * zoomSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * zoombackSpeed);
        }

        // Change masks with arrow keys
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectNextMask();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectPreviousMask();
        }

        if (platformMesh != null)
        {
            // Change masks with arrow keys
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SelectNextShoePair();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SelectPreviousShoePair();
            }
        }


        // Toggle dance state with O key
        if (Input.GetKeyDown(KeyCode.O))
        {
            bool isDancing = playerAnimator.GetBool("Dance");
            SetDance(!isDancing); // Toggle the state
        }

        // Toggle fight state with P key
        if (Input.GetKeyDown(KeyCode.P))
        {
            bool isFighting = playerAnimator.GetBool("Fight");
            SetFight(!isFighting); // Toggle the state
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
    }

    void RotatePlayerAndCamera()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * rotationPlayer;
        float verticalRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;

        // Adjusting camera's vertical rotation with wrapping consideration
        float deltaRotationX = verticalRotation * rotationSensitivity;
        float newCameraRotationX = originalCameraRotationX - deltaRotationX; // Adjust based on input

        // Clamping the normalized angle
        newCameraRotationX = Mathf.Clamp(newCameraRotationX, originalCameraRotation.x - maxVerticalAngle, originalCameraRotation.x + maxVerticalAngle);
        originalCameraRotationX = newCameraRotationX; // Store the new rotation as the "original" for smooth adjustment

        playerCamera.transform.localEulerAngles = new Vector3(newCameraRotationX, playerCamera.transform.localEulerAngles.y, playerCamera.transform.localEulerAngles.z);
    }

    void SmoothReturnToOriginalRotation()
    {
        Vector3 currentRotation = playerCamera.transform.localEulerAngles;
        float currentRotationX = NormalizeAngle(currentRotation.x); // Normalize current rotation for smooth interpolation
        float originalRotationX = NormalizeAngle(originalCameraRotation.x);

        // Smoothly interpolate the X rotation back to its original value
        float newXRotation = Mathf.LerpAngle(currentRotationX, originalRotationX, Time.deltaTime * transitionSpeed);
        playerCamera.transform.localEulerAngles = new Vector3(newXRotation, currentRotation.y, currentRotation.z);
    }

    // Normalize angles to -180 to 180 to handle wrapping
    float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }

    // Call this method when the Next button is clicked or the right arrow key is pressed
    public void SelectNextMask()
    {
        if (target_rn == playerMesh)
        {
            masks[currentMaskIndex].SetActive(false); // Hide current mask
            currentMaskIndex = (currentMaskIndex + 1) % masks.Length; // Cycle through masks
            masks[currentMaskIndex].SetActive(true); // Show next mask
        }
        else
        {
            trophies[currentTrophyIndex].SetActive(false); // Hide current mask
            currentTrophyIndex = (currentTrophyIndex + 1) % trophies.Length; // Cycle through masks
            trophies[currentTrophyIndex].SetActive(true); // Show next mask
        }

    }

    // Added to allow selection of the previous mask with the left arrow key
    public void SelectPreviousMask()
    {
        if(target_rn == playerMesh)
        {
            masks[currentMaskIndex].SetActive(false); // Hide current mask
            if (currentMaskIndex == 0)
            {
                currentMaskIndex = masks.Length - 1;
            }
            else
            {
                currentMaskIndex--;
            }
            masks[currentMaskIndex].SetActive(true); // Show previous mask
        }
        else
        {
            trophies[currentTrophyIndex].SetActive(false); // Hide current mask
            if (currentTrophyIndex == 0)
            {
                currentTrophyIndex = trophies.Length - 1;
            }
            else
            {
                currentTrophyIndex--;
            }
            trophies[currentTrophyIndex].SetActive(true); // Show previous mask
        }

    }

    public void SelectNextShoePair()
    {
        // Hide current shoe pair
        SetShoePairActive(shoePairs[currentShoePairIndex], false);

        // Move to next shoe pair
        currentShoePairIndex = (currentShoePairIndex + 1) % shoePairs.Length;

        // Show next shoe pair
        SetShoePairActive(shoePairs[currentShoePairIndex], true);
    }

    public void SelectPreviousShoePair()
    {
        // Hide current shoe pair
        SetShoePairActive(shoePairs[currentShoePairIndex], false);

        // Move to previous shoe pair
        currentShoePairIndex--;
        if (currentShoePairIndex < 0)
        {
            currentShoePairIndex = shoePairs.Length - 1;
        }

        // Show previous shoe pair
        SetShoePairActive(shoePairs[currentShoePairIndex], true);
    }
    private void SetShoePairActive(ShoePair pair, bool isActive)
    {
        pair.leftShoe.SetActive(isActive);
        pair.rightShoe.SetActive(isActive);
    }

    // Call this method to toggle the dance animation state
    public void SetDance(bool isDancing)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("Dance", isDancing);
        }
    }

    // Call this method to toggle the fight animation state
    public void SetFight(bool isFighting)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("Fight", isFighting);
        }
    }

    public void SaveMask()
    {
        // Save the selected mask index
        PlayerPrefs.SetInt("SelectedMaskIndex", currentMaskIndex);
        PlayerPrefs.SetInt("SelectedTrophyIndex", currentTrophyIndex);
        PlayerPrefs.SetInt("SelectedShoeIndex", currentShoePairIndex);
        PlayerPrefs.Save(); // Save PlayerPrefs changes
    }

    public void LoadGame()
    {
        SaveMask();
        SceneManager.LoadScene(1);   
    }

    // Function to toggle cursor mode
    public void ToggleCursorMode()
    {
        // If cursor is currently visible, lock it and hide for camera control
        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            modetext.text = "VIEW MODE";
        }
        else // If cursor is not visible, unlock it and show for UI interaction
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            modetext.text = "BUTTON MODE";
        }
    }

    public void ToggleCustomization()
    {
        if(target_rn == playerMesh)
        {
            ChangeTargetAndPlayerMesh(bikeMesh, playerMesh);
        }
        else if (target_rn == bikeMesh)
        {
            ChangeTargetAndPlayerMesh(playerMesh, bikeMesh);
        }
    }


    // Call this function when you want to change the target position and player mesh
    public void ChangeTargetAndPlayerMesh(GameObject newPlayerMesh, GameObject oldMesh)
    {

        if (newPlayerMesh != null)
        {

            oldMesh.SetActive(false);

            newPlayerMesh.SetActive(true);
            target_rn = newPlayerMesh;
        }
    }

}
