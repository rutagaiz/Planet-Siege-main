using UnityEngine;

public class Player_movement : MonoBehaviour
{
    public float movement;
    public float moveSpeed = 5f;
    public bool facingRight = true;
    public Rigidbody2D rb;
    public float flySpeed = 5f;
    public float fallSpeed = 5f;
    public float maxY = 11f;
    public GameManager cm;

    // References to sprite renderers
    public SpriteRenderer helmetSpriteRenderer;
    public SpriteRenderer torsoSpriteRenderer;

    public bool simulateWPressed = false;


    public void Start()
    {
        // LOCK rotation so player NEVER tilts or spins
        rb.freezeRotation = true;

        // Get the SpriteRenderer components

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

        // Find the torso child object
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

        // Flip left/right
        if (movement < 0f && facingRight)
        {
            Flip(false);
        }
        else if (movement > 0f && !facingRight)
        {
            Flip(true);
        }

        if ((Input.GetKey(KeyCode.W) || simulateWPressed) && transform.position.y < maxY)
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

    public void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement * moveSpeed, rb.linearVelocity.y);
    }

    // Flip both torso and helmet
    public void Flip(bool faceRight)
    {
        facingRight = faceRight;

        // Flip torso sprite if it exists
        if (torsoSpriteRenderer != null)
        {
            torsoSpriteRenderer.flipX = !faceRight;
        }


        // Flip helmet sprite if it exists
        if (helmetSpriteRenderer != null)
        {
            helmetSpriteRenderer.flipX = !faceRight;
        }
    }

    public void FlyUp()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, flySpeed);
    }

    public void FlyDown()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -fallSpeed);
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
        FixedUpdate(); // Directly call physics update
    }
}