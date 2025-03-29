using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player; // Drag your Player GameObject here
    public Vector3 offset = new Vector3(0, 10, 0); // Adjust for a side-scrolling minimap

    void LateUpdate()
    {
        if (player != null)
        {
            // Keep the camera following the player's X movement
            Vector3 newPosition = transform.position;
            newPosition.x = player.position.x; // Follow player horizontally
            newPosition.y = player.position.y + offset.y; // Optional vertical follow
            transform.position = newPosition;
        }
    }
}
