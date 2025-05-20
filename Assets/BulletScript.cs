using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bullet_Damage = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float force = 20f;

    private Camera mainCam;
    private Rigidbody2D rb;
    private int dammageMultiplier = 1;
 

    void Start()
    {
        Player_Stats playerStats = Object.FindFirstObjectByType<Player_Stats>();
        if (playerStats != null)
        {
            playerStats.OnDamageChanged += UpdateDamageMultiplier;
        }

        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();

        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 direction = (mousePos - transform.position).normalized;

        rb.linearVelocity = direction * force;

        // Rotate bullet towards cursor
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);

        // Destroy bullet after lifetime
        Destroy(gameObject, lifetime);
    }
    private void UpdateDamageMultiplier(int newDamage)
    {
        dammageMultiplier = newDamage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy_Stats>(out Enemy_Stats enemy))
        {
            enemy.TakeDamage(bullet_Damage + dammageMultiplier);
        }

        if (collision.gameObject.CompareTag("EnemyTurret") || collision.gameObject.CompareTag("EnemyBase"))
        {
            if (collision.gameObject.TryGetComponent<turretScript>(out turretScript turret))
            {
                turret.TakeDamage((int)bullet_Damage);
            }
        }
            Destroy(gameObject);
    }
}