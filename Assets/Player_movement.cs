using UnityEngine;

public class Player_movement : MonoBehaviour
{
    private float movement;
    public float moveSpeed = 5f;
    private bool facingRight = true;
    public Rigidbody2D rb;
    public float flySpeed = 5f;
    public float fallSpeed = 5f;
    public float maxY = 11f;
    public GameManager cm;

    // References to sprite renderers
    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer helmetSpriteRenderer;

    void Start()
    {
        // LOCK rotation so player NEVER tilts or spins
        rb.freezeRotation = true;

        // Get the SpriteRenderer components
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer == null)
        {
            Debug.LogError("Player SpriteRenderer component not found!");
        }

        // Find the helmet child object
        Transform helmet = transform.Find("helmet_0");
        if (helmet != null)
        {
            helmetSpriteRenderer = helmet.GetComponent<SpriteRenderer>();
            if (helmetSpriteRenderer == null)
            {
                Debug.LogError("Helmet SpriteRenderer component not found!");
            }
        }
        else
        {
            Debug.LogError("Helmet child object not found!");
        }
    }

    void Update()
    {
        movement = Input.GetAxis("Horizontal");

        // Flip left/right
        if (movement < 0f && facingRight)
        {
            Flip(false);
        }
        else if (movement > 0f && !facingRight)
        {
            Flip(true);
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

    // Flip both player and helmet
    private void Flip(bool faceRight)
    {
        facingRight = faceRight;

        // Flip player sprite
        playerSpriteRenderer.flipX = !faceRight;

        // Flip helmet sprite if it exists
        if (helmetSpriteRenderer != null)
        {
            helmetSpriteRenderer.flipX = !faceRight;
        }
    }

    void FlyUp()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, flySpeed);
    }

    void FlyDown()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -fallSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            GameManager.Instance.AddCoin(1); 
            //Destroy(other.gameObject);      
        }
    }
}