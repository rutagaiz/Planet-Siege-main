using UnityEngine;

public class Crouch : MonoBehaviour
{
    public float crouchHeight = 0.5f;
    private Vector2 normalScale;
    private Vector2 normalColliderSize;
    private Vector2 normalColliderOffset;
    private float yInput;
    private bool facingRight = true;

    private BoxCollider2D boxCollider;

    void Start()
    {

        boxCollider = GetComponent<BoxCollider2D>();

        // Store visual model scale size 
        normalScale = transform.localScale;

        // Store original collider size and offset (crouching hitbox)
        normalColliderSize = boxCollider.size;
        normalColliderOffset = boxCollider.offset;
    }

    void Update()
    {
        yInput = Input.GetAxisRaw("Vertical");

        if (yInput < 0) // Crouching
        {
            // Reduce visual size of the model 
            transform.localScale = new Vector2(normalScale.x, crouchHeight);

            // Reduce collider size and adjust offset (crouching hitbox)
            boxCollider.size = new Vector2(normalColliderSize.x, normalColliderSize.y * 0.5f);
            boxCollider.offset = new Vector2(normalColliderOffset.x, normalColliderOffset.y - 0.5f);
        }
        else // Standing up
        {
            // Return visual size of the model to the normal value
            transform.localScale = normalScale;

            // Return collider size and offset to the original value (crouching hitbox)
            boxCollider.size = normalColliderSize;
            boxCollider.offset = normalColliderOffset;
        }

        // Handle character flipping and movement
        float xInput = Input.GetAxisRaw("Horizontal");

        // If moving right but facing left, flip to the right
        if (xInput > 0 && !facingRight)
        {
            Flip();
        }

        // If moving left but facing right, flip to the left
        else if (xInput < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector2(-normalScale.x, transform.localScale.y);
    }
}
