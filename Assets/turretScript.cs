using System.Collections.Generic;
using UnityEngine;

public enum TurretFaction { Ally, Enemy }

public class turretScript : MonoBehaviour
{
    [Header("Turret Settings")]
    public TurretFaction turretFaction;

    public float Range;
    public string[] targetTags;         
    public LayerMask targetLayer;      

    public GameObject Gun;
    public GameObject Bullet;
    public float FireRate = 1f;
    private float nextTimeToFire = 0;
    public Transform ShootPoint;

    [Header("Turret Stats")]
    [SerializeField] private int maxHealth = 50;
    private int currentHealth;
    private bool isDestroyed = false;

    private Collider2D col;
    private SpriteRenderer sr;

    private Transform currentTarget;
    private Vector2 direction;

    void Start()
    {
        currentHealth = maxHealth;
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDestroyed) return;

        currentTarget = GetNearestTarget();

        if (currentTarget != null)
        {
            direction = (currentTarget.position - transform.position).normalized;

         
            Vector2 origin = (Vector2)transform.position + direction * 0.1f;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, Range, targetLayer);

            if (hit.collider != null && MatchesTargetTags(hit.collider.tag) && hit.transform == currentTarget)
            {
                Gun.transform.right = direction;

                if (Time.time > nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 1f / FireRate;
                    Shoot();
                }
            }

            Debug.DrawRay(origin, direction * Range, Color.red);
        }
     
            if (isDestroyed) return;

            currentTarget = GetNearestTarget();

            if (currentTarget != null)
            {
                direction = (currentTarget.position - transform.position).normalized;
                Debug.Log("🔍 Found a target: " + currentTarget.name);

                Vector2 origin = (Vector2)transform.position + direction * 0.1f;
                RaycastHit2D hit = Physics2D.Raycast(origin, direction, Range, targetLayer);

                if (hit.collider != null)
                {
                    Debug.Log("👁 Raycast hit: " + hit.collider.name + " | Tag: " + hit.collider.tag);
                }

                if (hit.collider != null && MatchesTargetTags(hit.collider.tag) && hit.transform == currentTarget)
                {
                    Debug.Log("✅ Valid target in line of sight: " + hit.collider.name);

                    Gun.transform.right = direction;

                    if (Time.time > nextTimeToFire)
                    {
                        Debug.Log("💥 Turret shooting at: " + currentTarget.name);
                        nextTimeToFire = Time.time + 1f / FireRate;
                        Shoot();
                    }
                }

                Debug.DrawRay(origin, direction * Range, Color.red);
            }
            else
            {
                Debug.Log("🚫 No valid target found in range.");
            }
        
    }

    void Shoot()
    {
        GameObject bulletIns = Instantiate(Bullet, ShootPoint.position, Quaternion.identity);

        TurretBullet bulletScript = bulletIns.GetComponent<TurretBullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
            bulletScript.SetFaction(turretFaction);
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
        Debug.Log($"{turretFaction} turret destroyed!");

        if (sr != null) sr.color = Color.gray;
        if (col != null) col.enabled = false;

        Destroy(gameObject, 1.5f);
    }

    private Transform GetNearestTarget()
    {
        List<GameObject> allTargets = new();

        foreach (string tag in targetTags)
        {
            allTargets.AddRange(GameObject.FindGameObjectsWithTag(tag));
        }

        float minDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject obj in allTargets)
        {
            float distance = Vector2.Distance(transform.position, obj.transform.position);
            if (distance < minDistance && distance <= Range)
            {
                minDistance = distance;
                nearest = obj.transform;
            }
        }

        return nearest;
    }

    private bool MatchesTargetTags(string tag)
    {
        foreach (string t in targetTags)
        {
            if (t == tag) return true;
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = turretFaction == TurretFaction.Ally ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}