using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float dampening;

    public Transform target;

    private Vector3 vel = Vector3.zero;

    // Added: Flag to switch between auto and manual modes
    private bool isManualMode = false;

    // Added: Speed for manual camera movement
    [SerializeField] private float manualMoveSpeed = 5f;

    private void FixedUpdate()
    {
        if (isManualMode)
        {
            HandleManualMovement(); // Added: manual movement handling
        }
        else
        {
            Vector3 targetPosition = target.position + offset;
            targetPosition.z = transform.position.z;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, dampening);
        }
    }

    private void HandleManualMovement()
    {
        // Convert mouse position from screen space to world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Keep the camera's original z value
        mouseWorldPos.z = transform.position.z;

        // Smoothly move toward the mouse position
        transform.position = Vector3.Lerp(transform.position, mouseWorldPos, manualMoveSpeed * Time.deltaTime);
    }

    public void ToggleManualMode()
    {
        isManualMode = !isManualMode;
    }

    private void Update()
    {
        // Toggle between manual and follow mode when pressing the 'M' key
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleManualMode();
        }
    }
}
