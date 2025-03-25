using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class turretScript : MonoBehaviour
{
    [Header("Turret Settings")]
    public float Range;
    public Transform Target;
    public LayerMask playerLayer;

    private bool Detected = false;
    private Vector2 Direction;

    public GameObject Gun;
    public GameObject Bullet;
    public float FireRate;
    private float nextTimeToFire = 0;
    public Transform ShootPoint;

    public float Force;

    [Header("Turret Stats")]
    [SerializeField] private int maxHealth = 50;
    private int currentHealth;
    private bool isDestroyed = false;

    private Collider2D col;
    private SpriteRenderer sr;

    void Start()
    {
        currentHealth = maxHealth;
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDestroyed) return;

        Vector2 targetPos = Target.position;
        Direction = targetPos - (Vector2)transform.position;

        // Debug ray
        Debug.DrawRay(transform.position, Direction.normalized * Range, Color.red);

        // Layer-masked raycast only for Player
        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction.normalized, Range, playerLayer);

        if (rayInfo.collider != null && rayInfo.collider.CompareTag("Player"))
        {
            Detected = true;
        }
        else
        {
            Detected = false;
        }

        if (Detected)
        {
            Gun.transform.right = Direction;

            if (Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / FireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject bulletIns = Instantiate(Bullet, ShootPoint.position, Quaternion.identity);

        TurretBullet bulletScript = bulletIns.GetComponent<TurretBullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(Direction);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDestroyed) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDestroyed = true;
        Debug.Log("Turret destroyed!");

        Detected = false;

        if (sr != null) sr.color = Color.gray;
        if (col != null) col.enabled = false;

        Destroy(gameObject, 1.5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}