using UnityEngine;

public class UnitBulletScript : MonoBehaviour
{
    public enum BulletFaction { Ally, Enemy }
    private BulletFaction bulletFaction;

    [SerializeField] private float bulletDamage = 5f;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float speed = 10f;

    private Vector2 direction;
    private Rigidbody2D rb;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        Debug.Log($"🧭 Bullet direction set: {direction}");
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
            Debug.LogWarning("⚠️ Bullet direction was zero, defaulted to Vector2.right");
        }

        rb.linearVelocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"💥 Unit bullet hit {collision.gameObject.name}");

        // Ally unit bullet -> EnemyUnit
        if (bulletFaction == BulletFaction.Ally && collision.gameObject.CompareTag("EnemyUnits"))
        {
            if (collision.gameObject.TryGetComponent<Enemy_Stats>(out var enemy))
            {
                enemy.TakeDamage((int)bulletDamage);
                Debug.Log($"💥 Enemy unit took {bulletDamage} damage");
            }
        }

        // Enemy unit bullet -> AllyUnit
        if (bulletFaction == BulletFaction.Enemy && collision.gameObject.CompareTag("AllyTroop"))
        {
            if (collision.gameObject.TryGetComponent<HeavyScoutScript>(out var ally))
            {
                ally.TakeDamage((int)bulletDamage);
                Debug.Log($"💥 Ally unit took {bulletDamage} damage");
            }
        }

        // Bullet hits a tower
        if (collision.gameObject.TryGetComponent<turretScript>(out turretScript turret))
        {
            if ((bulletFaction == BulletFaction.Ally && turret.turretFaction == TurretFaction.Enemy) ||
                (bulletFaction == BulletFaction.Enemy && turret.turretFaction == TurretFaction.Ally))
            {
                turret.TakeDamage((int)bulletDamage);
                Debug.Log($"🏰 Tower hit for {bulletDamage}");
            }
        }

        Destroy(gameObject);
    }
}