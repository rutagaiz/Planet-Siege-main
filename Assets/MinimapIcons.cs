using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MinimapIcon : MonoBehaviour
{
    public Transform worldObject; // The object in the game world
    public RectTransform icon;    // The UI Image (minimap icon)
    public Camera minimapCamera;  // The minimap camera
    public RectTransform minimapUI; // The minimap UI RectTransform (Raw Image)

    private Vector2 minimapSize;
    private Vector3 mapStart, mapEnd;

    void Start()
    {
        // Get minimap UI size for calculations
        minimapSize = minimapUI.rect.size;

        // Get Minimap Camera World Bounds
        mapStart = minimapCamera.ViewportToWorldPoint(new Vector3(0, 0, minimapCamera.nearClipPlane));
        mapEnd = minimapCamera.ViewportToWorldPoint(new Vector3(1, 1, minimapCamera.nearClipPlane));
    }

    void Update()
    {
        if (worldObject == null || icon == null || minimapCamera == null || minimapUI == null) return;

        if (worldObject == null || worldObject.gameObject == null)
        {
            Destroy(icon.gameObject);
            icon = null;
            return;
        }

        // Get world object position
        Vector3 worldPosition = worldObject.position;

        // Normalize position within minimap world bounds
        float normalizedX = Mathf.InverseLerp(mapStart.x, mapEnd.x, worldPosition.x);
        float normalizedY = Mathf.InverseLerp(mapStart.y, mapEnd.y, worldPosition.y);

        // Convert to minimap UI coordinates
        float xPos = (normalizedX - 0.5f) * minimapSize.x;
        float yPos = (normalizedY - 0.5f) * minimapSize.y;

        // Apply calculated position
        icon.anchoredPosition = new Vector2(xPos, yPos);
    }
}