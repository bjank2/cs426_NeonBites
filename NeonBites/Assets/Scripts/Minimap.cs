using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player;

    public Transform bike;
    public RectTransform minimapRectTransform;
    public RectTransform playerIcon; // Reference to the UI player icon
    public float height = 20f; // Height of the minimap above the terrain
    public float followSpeed = 5f; // Speed of the minimap following the player

    private Camera minimapCamera;
    public Camera mainCamera;
    public RectTransform bikeIcon;

    private Vector3 targetPosition;
    private Vector3 targetPositionBike;
    public bool bikeVisible = true;

    public RectTransform enemyIconPrefab; // Assign this in the inspector
    private List<RectTransform> enemyIcons = new List<RectTransform>();
    private Dictionary<GameObject, RectTransform> enemyIconMap = new Dictionary<GameObject, RectTransform>();


    private void Start()
    {
        height = gameObject.transform.position.y;

        // Get the minimap camera component
        minimapCamera = GetComponent<Camera>();

        // Disable shadows rendering
        minimapCamera.renderingPath = RenderingPath.Forward;
    }

    void LateUpdate()
    {
        // Set the target position for the minimap to follow the player
        targetPosition = new Vector3(player.position.x, height, player.position.z);

        if (bike!= null)
        {
            targetPositionBike = new Vector3(bike.position.x, height, bike.position.z);
            Vector2 screenPositionBike = WorldToMiniMapViewport(bike.position);
        }

        // Smoothly move the minimap towards the target position using lerp
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        // Update the position of the player icon relative to the minimap's position
        Vector2 screenPosition = WorldToMiniMapViewport(player.position);


        // Rotate the minimap to match the player camera's rotation (yaw only)
        transform.rotation = Quaternion.Euler( 90f, mainCamera.gameObject.transform.eulerAngles.y, 0f);

        // Update the player icon's position and rotation
        if (player != null && playerIcon != null)
        {
            UpdatePlayerIcon();
            CheckWithinMap(player, playerIcon, true);
        }

        if (bike != null)
        {
            CheckWithinMap(bike, bikeIcon, bikeVisible);
        }

        UpdateEnemyIcons();
    }
    private void UpdatePlayerIcon()
    {
        // Get a vector from the player to the camera projected onto the XZ plane
        Vector3 cameraForwardOnXZ = mainCamera.transform.forward;
        cameraForwardOnXZ.y = 0;
        cameraForwardOnXZ.Normalize(); // Normalize to ensure it's a direction vector

        // Get the player's forward direction also projected onto the XZ plane
        Vector3 playerForwardOnXZ = player.forward;
        playerForwardOnXZ.y = 0;
        playerForwardOnXZ.Normalize(); // Normalize to ensure it's a direction vector

        // Calculate the angle between the player's forward direction and the camera's forward direction on XZ plane
        float angle = Vector3.SignedAngle(cameraForwardOnXZ, playerForwardOnXZ, Vector3.up);

        // Apply the calculated angle to the player icon rotation, adjusting by the minimap's current rotation
        // so that the icon points in the correct direction relative to the minimap
        playerIcon.localEulerAngles = new Vector3(0, 0, -angle + 90f);
    }


    private void CheckWithinMap(Transform obj, RectTransform icon, bool makeVisible)
    {
        // Get the viewport position of the object
        Vector3 viewportPosition = minimapCamera.WorldToViewportPoint(obj.position);

        // If the object is visible and within bounds, directly update its icon position
        if (viewportPosition.z > 0 && viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1)
        {
            if (makeVisible) { icon.gameObject.SetActive(true); } else { icon.gameObject.SetActive(false); }

            // Update the icon's position
            icon.anchoredPosition = ViewportToAnchoredPosition(viewportPosition);
        }
        else
        {
            // Get the direction from the player to the object
            Vector3 direction = (obj.position - player.position).normalized;

            // Project the direction onto the minimap plane to get an edge position
            Vector3 edgePosition = player.position + direction * 10000f; // Use a large number to ensure it's out of bounds
            viewportPosition = minimapCamera.WorldToViewportPoint(edgePosition);

            // Clamp the projected viewport position to the bounds of the minimap
            viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0.05f, 0.95f); // Add a small margin to ensure the icon stays fully within the frame
            viewportPosition.y = Mathf.Clamp(viewportPosition.y, 0.05f, 0.95f);

            // Update the icon's position
            icon.anchoredPosition = ViewportToAnchoredPosition(viewportPosition);
        }
    }

    private Vector2 ViewportToAnchoredPosition(Vector3 viewportPosition)
    {
        // Convert the viewport position to an anchored position relative to the minimap UI element
        Vector2 anchoredPosition = new Vector2(
            (viewportPosition.x * minimapCamera.pixelWidth) - (minimapCamera.pixelWidth / 2f),
            (viewportPosition.y * minimapCamera.pixelHeight) - (minimapCamera.pixelHeight / 2f));

        return anchoredPosition;
    }


    private Vector2 WorldToMiniMapViewport(Vector3 worldPosition)
    {
        // Convert world position to viewport coordinates relative to the minimap's RectTransform
        Vector3 localPosition = minimapRectTransform.InverseTransformPoint(worldPosition);

        // Adjust position based on minimap's anchor
        float offsetX = minimapRectTransform.anchorMax.x * minimapRectTransform.rect.width;
        float offsetY = minimapRectTransform.anchorMax.y * minimapRectTransform.rect.height;
        localPosition.x += offsetX;
        localPosition.y += offsetY;

        return new Vector2(localPosition.x / minimapRectTransform.rect.width, localPosition.z / minimapRectTransform.rect.height);
    }

    private void UpdateEnemyIcons()
    {
        // Find all enemy GameObjects
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Update existing icons or deactivate them if the enemy is out of bounds
        foreach (var enemyIconPair in new Dictionary<GameObject, RectTransform>(enemyIconMap))
        {
            GameObject enemy = enemyIconPair.Key;
            RectTransform icon = enemyIconPair.Value;

            if (!enemy || !Array.Exists(enemies, e => e == enemy))
            {
                Destroy(icon.gameObject); // Destroy the icon
                enemyIconMap.Remove(enemy); // Remove from map
            }
            else
            {
                Vector3 viewportPosition = minimapCamera.WorldToViewportPoint(enemy.transform.position);

                // Check if the enemy is within the camera's viewport
                bool isWithinBounds = viewportPosition.z > 0 && viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1;

                // If the enemy is within bounds, update the icon's position and rotation
                if (isWithinBounds)
                {
                    icon.gameObject.SetActive(true); // Make sure the icon is active

                    // Update the icon's position and rotation
                    icon.anchoredPosition = ViewportToAnchoredPosition(viewportPosition);
                    //icon.localEulerAngles = new Vector3(0, 0, enemy.transform.eulerAngles.y);
                }
                else
                {
                    // If the enemy is out of bounds, deactivate the icon
                    icon.gameObject.SetActive(false);
                }
            }
        }

        // Instantiate icons for new enemies
        foreach (GameObject enemy in enemies)
        {
            if (!enemyIconMap.ContainsKey(enemy))
            {
                Vector3 viewportPosition = minimapCamera.WorldToViewportPoint(enemy.transform.position);
                bool isWithinBounds = viewportPosition.z > 0 && viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1;

                if (isWithinBounds)
                {
                    // Instantiate a new icon for this enemy
                    RectTransform icon = Instantiate(enemyIconPrefab, minimapRectTransform);
                    enemyIconMap[enemy] = icon;

                    // Update the icon's position and rotation
                    icon.anchoredPosition = ViewportToAnchoredPosition(viewportPosition);
                    //icon.localEulerAngles = new Vector3(0, 0, enemy.transform.eulerAngles.y);
                }
            }
        }
    }


}
