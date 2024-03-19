using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPlayer : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float rotationPlayer = 100.0f;
    public GameObject playerMesh;
    public float zoomAmount = 5f; // How far the camera zooms in
    public float zoomSpeed = 2f; // How smoothly the camera zooms in and out
    public float zoombackSpeed = 4f;

    public GameObject[] masks; // Assign your masks in the inspector
    private int currentMaskIndex = 0;

    public Animator playerAnimator; // Reference to the player's animator component
    private Vector3 originalPosition; // To store the camera's original position
    private bool isZooming = false; // To check if we're currently zooming
    private Vector3 targetPosition; // Target position for zoom based on current direction

    [SerializeField] private float minY = -60f; // Minimum vertical angle
    [SerializeField] private float maxY = 60f; // Maximum vertical angle
    [SerializeField] private float minX = -360f; // Minimum horizontal angle (optional for full rotation)
    [SerializeField] private float maxX = 360f; // Maximum horizontal angle (optional for full rotation)

    private float currentYRotation = 0f;
    private float currentXRotation = 0f;

    void Start()
    {
        originalPosition = transform.position; // Store the original position of the camera

        // Initialize rotations based on the current orientation in the scene view
        Vector3 angles = transform.eulerAngles;
        currentXRotation = angles.x;
        currentYRotation = angles.y;
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) // Right mouse button for rotation
        {
            //AdjustCameraRotation();
        }

        if (Input.GetKey(KeyCode.X)) // X key for player rotation
        {
            playerMesh.transform.Rotate(Vector3.up, rotationPlayer * Time.deltaTime);
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
    }

    private void AdjustCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        currentYRotation += mouseX;
        currentXRotation -= mouseY; // Inverting mouseY for natural 'looking' effect

        // Clamp X rotation within specified vertical limits
        currentXRotation = Mathf.Clamp(currentXRotation, minY, maxY);

        // Optionally, clamp Y rotation within specified horizontal limits
        currentYRotation = Mathf.Clamp(currentYRotation, minX, maxX);

        Quaternion localRotation = Quaternion.Euler(currentXRotation, currentYRotation, 0f);
        transform.rotation = localRotation;
    }

    // Call this method when the Next button is clicked or the right arrow key is pressed
    public void SelectNextMask()
    {
        masks[currentMaskIndex].SetActive(false); // Hide current mask
        currentMaskIndex = (currentMaskIndex + 1) % masks.Length; // Cycle through masks
        masks[currentMaskIndex].SetActive(true); // Show next mask
    }

    // Added to allow selection of the previous mask with the left arrow key
    public void SelectPreviousMask()
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
}
