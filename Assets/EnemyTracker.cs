using UnityEngine;
using UnityEngine.UI;

public class EnemyTracker : MonoBehaviour
{

    private Transform worldObject;
    private RectTransform icon;
    private Camera minimapCamera;
    private RectTransform minimapUI;
    private Vector2 minimapSize;
    private Vector3 mapStart, mapEnd;

    public void Initialize(Transform worldObj, RectTransform iconTransform, Camera camera, RectTransform ui)
    {
        worldObject = worldObj;
        icon = iconTransform;
        minimapCamera = camera;
        minimapUI = ui;
        
        if (minimapCamera == null || minimapUI == null || icon == null || worldObject == null)
        {
            Debug.LogError("EnemyTracker initialization failed - null reference provided");
            Destroy(this); // Destroy this component if initialization fails
            return;
        }
        
        // Get minimap UI size
        minimapSize = minimapUI.rect.size;

        // Get minimap camera world bounds
        mapStart = minimapCamera.ViewportToWorldPoint(new Vector3(0, 0, minimapCamera.nearClipPlane));
        mapEnd = minimapCamera.ViewportToWorldPoint(new Vector3(1, 1, minimapCamera.nearClipPlane));
    }

    void Update()
    {
        if (worldObject == null || icon == null || minimapCamera == null || minimapUI == null)
             {
                 // Clean up if any references are lost
                 if (icon != null)
                 {
                     Destroy(icon.gameObject);
                 }
                 Destroy(this); // Destroy this component
                 return;
             }
        
        // Get world position and normalize within minimap bounds
        Vector3 worldPosition = worldObject.position;
        float normalizedX = Mathf.InverseLerp(mapStart.x, mapEnd.x, worldPosition.x);
        float normalizedY = Mathf.InverseLerp(mapStart.y, mapEnd.y, worldPosition.y);

        // Convert to minimap UI coordinates
        float xPos = (normalizedX - 0.5f) * minimapSize.x;
        float yPos = (normalizedY - 0.5f) * minimapSize.y;

        // Apply position to icon
        icon.anchoredPosition = new Vector2(xPos, yPos);
    }

    void OnDestroy()
    {
        if (icon != null)
        {
            Destroy(icon.gameObject); // Remove minimap icon when enemy is destroyed
        }
    }

}
