using UnityEngine;
using UnityEngine.UI;

public class MinimapIcon : MonoBehaviour
{
    public Transform worldObject;  // The object this icon represents
    public RectTransform icon;     // The UI Image representing this object on the minimap
    public Transform minimapCamera; // Reference to the minimap camera
    public float minimapScale = 1.5f; // Scale factor for minimap positioning

    void Update()
    {
        if (worldObject == null || icon == null || minimapCamera == null) return;

        // Convert world position to minimap position
        Vector3 minimapPosition = minimapCamera.InverseTransformPoint(worldObject.position);
        icon.anchoredPosition = new Vector3(minimapPosition.x * minimapScale, minimapPosition.y * minimapScale, minimapPosition.z * minimapScale);
    }
}