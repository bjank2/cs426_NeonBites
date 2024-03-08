using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player;
    public RectTransform minimapRectTransform;
    public RectTransform playerIcon; // Reference to the UI player icon
    public float height = 20f; // Height of the minimap above the terrain
    public float followSpeed = 5f; // Speed of the minimap following the player

    private Camera minimapCamera;

    private Vector3 targetPosition;

    void LateUpdate()
    {
        // Set the target position for the minimap to follow the player
        targetPosition = new Vector3(player.position.x, height, player.position.z);

        // Smoothly move the minimap towards the target position using lerp
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        // Update the position of the player icon relative to the minimap's position
        Vector2 screenPosition = WorldToMiniMapViewport(player.position);
        //playerIcon.anchoredPosition = new Vector2(screenPosition.x * minimapRectTransform.rect.width, screenPosition.y * minimapRectTransform.rect.height);

        // Rotate the minimap to match the player camera's rotation (yaw only)
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);

        if (player == null || playerIcon == null)
            return;

        // Check if the player is within the minimap camera's view
        Vector3 screenPoint = minimapCamera.WorldToViewportPoint(player.position);
        if (screenPoint.z > 0 && screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1)
        {
            // If the player is within the camera's view, update the position of the player icon
            Vector2 viewportPosition = minimapCamera.WorldToViewportPoint(player.position);

            // Convert viewport position to anchored position relative to the minimap UI element
            Vector2 anchoredPosition = new Vector2(viewportPosition.x * minimapCamera.pixelWidth, viewportPosition.y * minimapCamera.pixelHeight);
            anchoredPosition -= new Vector2(minimapCamera.pixelWidth / 2f, minimapCamera.pixelHeight / 2f); // Offset to center
            playerIcon.anchoredPosition = anchoredPosition;
        }
        else
        {
            // If the player is not within the camera's view, hide the player icon
            playerIcon.gameObject.SetActive(false);
        }
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

    private void Start()
    {
        height = gameObject.transform.position.y;

        // Get the minimap camera component
        minimapCamera = GetComponent<Camera>();

        // Disable shadows rendering
        minimapCamera.renderingPath = RenderingPath.Forward;
    }

}
