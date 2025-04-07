using UnityEngine;


public class CrouchController : MonoBehaviour
{
    public Transform torso; // Assign the "Torso" child object in the Inspector
    public Transform head; // Assign the "Head" child object in the Inspector
    public BoxCollider2D playerCollider; // Reference to the main collider on the Player object
    public Sprite crouchingSprite; // Assign crouching sprite in the Inspector
    public Sprite standingSprite; // Assign standing sprite in the Inspector
    public float crouchHeightReduction = 0.5f; // How much shorter the collider should be when crouching
    public float crouchSpriteLowering = 0.5f; // How much lower the torso should move when crouching
    public float headLowering = 0.3f; // How much lower the head should move when crouching

    private SpriteRenderer torsoRenderer;
    private bool isCrouching = false;

    private Vector2 standingSize;  // Stores the original collider size
    private Vector2 standingOffset; // Stores the original collider offset
    private Vector2 crouchingSize; // Will be calculated based on standing size
    private Vector2 crouchingOffset; // Will be auto-adjusted to keep feet on the ground
    private Vector3 standingTorsoPosition; // Stores original torso position
    private Vector3 crouchingTorsoPosition; // Will be calculated
    private Vector3 standingHeadPosition; // Stores original head position
    private Vector3 crouchingHeadPosition; // Will be calculated

    void Start()
    {
        torsoRenderer = torso.GetComponent<SpriteRenderer>();

        if (playerCollider != null)
        {
            // Store original standing values
            standingSize = playerCollider.size;
            standingOffset = playerCollider.offset;
            standingTorsoPosition = torso.localPosition;
            standingHeadPosition = head.localPosition;

            // Calculate crouching size based on reduction amount
            crouchingSize = new Vector2(standingSize.x, standingSize.y - crouchHeightReduction);

            // Adjust the offset so that the feet stay in place
            float offsetAdjustment = (standingSize.y - crouchingSize.y) / 2f;
            crouchingOffset = new Vector2(standingOffset.x, standingOffset.y - offsetAdjustment);

            // Calculate new positions when crouching
            crouchingTorsoPosition = standingTorsoPosition + new Vector3(0, -crouchSpriteLowering, 0);
            crouchingHeadPosition = standingHeadPosition + new Vector3(0, -headLowering, 0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            StandUp();
        }
    }

    void Crouch()
    {
        if (!isCrouching)
        {
            isCrouching = true;
            torsoRenderer.sprite = crouchingSprite;
            torso.localPosition = crouchingTorsoPosition; // Move torso down
            head.localPosition = crouchingHeadPosition; // Move head down

            if (playerCollider != null)
            {
                playerCollider.size = crouchingSize;
                playerCollider.offset = crouchingOffset;
            }
        }
    }

    void StandUp()
    {
        if (isCrouching)
        {
            isCrouching = false;
            torsoRenderer.sprite = standingSprite;
            torso.localPosition = standingTorsoPosition; // Move torso back up
            head.localPosition = standingHeadPosition; // Move head back up

            if (playerCollider != null)
            {
                playerCollider.size = standingSize;
                playerCollider.offset = standingOffset;
            }
        }
    }
}