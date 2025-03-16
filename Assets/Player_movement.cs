using UnityEngine;

public class Player_movement : MonoBehaviour
{
    private float movement;
    public float moveSpeed = 5f;
    private bool facingRight = true;
    public Rigidbody2D rb;
    public float flySpeed = 5f; // Fixed flying speed
    public float fallSpeed = 5f; // Fixed downward speed
    public float maxY = 11f; // Max Y position allowed

    void Update()
    {
        movement = Input.GetAxis("Horizontal");

        if (movement < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && !facingRight)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        if (Input.GetKey(KeyCode.W) && transform.position.y < maxY)
        {
            FlyUp();
        }

        if (Input.GetKey(KeyCode.S))
        {
            FlyDown();
        }

        if (transform.position.y >= maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); 
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement * moveSpeed, rb.linearVelocity.y); 
    }

    void FlyUp()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, flySpeed); 
    }

    void FlyDown()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -fallSpeed); 
    }
}
