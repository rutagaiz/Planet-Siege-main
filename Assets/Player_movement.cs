using UnityEngine;

public class Player_movement : MonoBehaviour
{
    public float movement;
    public float moveSpeed = 5f;
    public bool facingRight = true;
    public Rigidbody2D rb;
    public float flySpeed = 5f; // Unused now
    public float fallSpeed = 5f; // Unused now
    public float maxY = 11f; // Unused for flying, but kept
    public GameManager cm;

    public SpriteRenderer helmetSpriteRenderer;
    public SpriteRenderer torsoSpriteRenderer;

    public bool simulateWPressed = false; // Unused now
    public bool simulateSPressed = false; // Unused now

    private int jumpCount = 0;
    private int maxJumps = 2;
    public float jumpForce = 10f; 

    public void Start()
    {
        rb.freezeRotation = true;

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

        Transform torso = transform.Find("torso");
        if (torso != null)
        {
            torsoSpriteRenderer = torso.GetComponent<SpriteRenderer>();
            if (torsoSpriteRenderer == null)
            {
                Debug.LogError("Torso SpriteRenderer component not found!");
            }
        }
        else
        {
            Debug.LogError("Torso child object not found!");
        }
    }

    public void Update()
    {
        movement = Input.GetAxis("Horizontal");

        if (movement < 0f && facingRight)
        {
            Flip(false);
        }
        else if (movement > 0f && !facingRight)
        {
            Flip(true);
        }

        // Jumping logic
        if ((Input.GetKeyDown(KeyCode.W) || simulateWPressed) && jumpCount < maxJumps)
        {
            Jump();
        }
    }

    public void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement * moveSpeed, rb.linearVelocity.y);
    }

    public void Flip(bool faceRight)
    {
        facingRight = faceRight;

        if (torsoSpriteRenderer != null)
        {
            torsoSpriteRenderer.flipX = !faceRight;
        }

        if (helmetSpriteRenderer != null)
        {
            helmetSpriteRenderer.flipX = !faceRight;
        }
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); 
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        jumpCount++;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            jumpCount = 0;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            GameManager.Instance.AddCoin(1);
            //Destroy(other.gameObject);
        }
    }

    public void TestMove(float horizontalInput)
    {
        movement = horizontalInput;
        FixedUpdate(); 
    }
}
