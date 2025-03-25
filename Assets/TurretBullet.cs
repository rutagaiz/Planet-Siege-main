using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    [SerializeField] private float bulletDamage = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float speed = 10f;

    private Vector2 direction;
    private Rigidbody2D rb;

    // Call this from the turret when instantiating the bullet
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (direction == Vector2.zero)
        {
            Debug.LogWarning("Bullet direction was not set. Defaulting to right.");
            direction = Vector2.right;
        }

        rb.linearVelocity = direction * speed;

        // Optional: rotate the bullet to face the direction it's moving
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<PlayerStats>(out PlayerStats player))
            {
                player.TakeDamage((int)bulletDamage);
            }

            Destroy(gameObject); // destroy bullet on impact
        }
    }
    void Update()
    {
        
    }
}
