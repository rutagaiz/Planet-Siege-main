using UnityEngine;

public class Crouch : MonoBehaviour
{
    public float crouchHeight = 0.5f;
    private Vector2 normalScale;
    private float yInput;
    private bool facingRight = true;

    void Start()
    {
        normalScale = transform.localScale;
    }

    void Update()
    {
        yInput = Input.GetAxisRaw("Vertical");

        // Crouch logic
        if (yInput < 0)
        {
            transform.localScale = new Vector2(normalScale.x, crouchHeight);
        }
        else
        {
            transform.localScale = normalScale;
        }

        // Movement and flipping
        float xInput = Input.GetAxisRaw("Horizontal");

        if (xInput > 0 && !facingRight)
        {
            Flip();
        }
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
