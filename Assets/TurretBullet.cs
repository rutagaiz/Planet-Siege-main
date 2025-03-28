using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    public enum BulletFaction { Ally, Enemy }
    private BulletFaction bulletFaction;

    [SerializeField] private float bulletDamage = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float speed = 10f;

    private Vector2 direction;
    private Rigidbody2D rb;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public void SetFaction(TurretFaction turretFaction)
    {
        bulletFaction = turretFaction == TurretFaction.Ally ? BulletFaction.Ally : BulletFaction.Enemy;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (direction == Vector2.zero)
        {
            direction = Vector2.right;
        }

        rb.linearVelocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"🔍 Bullet collided with: {collision.gameObject.name} (Tag: {collision.gameObject.tag})");
        // Jei kulka pataikė į objektą su turretScript
        if (collision.gameObject.TryGetComponent<turretScript>(out turretScript turret))
        {
            // Patikriname ar kulkos frakcija priešinga bokšto frakcijai
            if ((bulletFaction == BulletFaction.Ally && turret.turretFaction == TurretFaction.Enemy) ||
                (bulletFaction == BulletFaction.Enemy && turret.turretFaction == TurretFaction.Ally))
            {
                turret.TakeDamage((int)bulletDamage);
                
            }
        }

        // Jei pataikė į žaidėją
        if (bulletFaction == BulletFaction.Enemy && collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<PlayerStats>(out PlayerStats player))
            {
                player.TakeDamage((int)bulletDamage);
                Debug.Log($"💥 Player hit for {bulletDamage}");
            }
        }

        // Jei pataikė į priešą
        if (bulletFaction == BulletFaction.Ally && collision.gameObject.CompareTag("EnemyUnits"))
        {
            if (collision.gameObject.TryGetComponent<Enemy_Stats>(out Enemy_Stats enemy))
            {
                enemy.TakeDamage((int)bulletDamage);
                Debug.Log($"💥 Enemy unit hit for {bulletDamage}");
            }
        }

        Destroy(gameObject); // sunaikinam kulką
    }
}