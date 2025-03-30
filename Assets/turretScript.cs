using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("Repair Settings")]
    public bool isRepairing = false;
    public float repairCooldown = 10f;
    public float repairDuration = 3f;
    public int repairCost = 10;
    public int repairAmount = 20;
    public float repairRange = 2f;
    private float lastRepairTime = -Mathf.Infinity;
    private Transform playerTransform;

    [Header("UI References")]
    public GameObject healthBarPrefab;
    private GameObject towerHPUI;
    private Slider healthSlider;
    private TextMeshProUGUI healthText;

    void Awake()
    {
        CreateHealthBar();
    }

    void Start()
    {
        currentHealth = maxHealth;
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        UpdateHealthUI();
    }

    void Update()
    {
        if (isDestroyed) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(15);
            Debug.Log("☠️ Turret manually damaged for 15 HP.");
        }

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

        if (turretFaction == TurretFaction.Ally && Input.GetKeyDown(KeyCode.R))
        {
            TryRepair();
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
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthUI();

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

    public void TryRepair()
    {
        if (turretFaction != TurretFaction.Ally) return;
        if (isDestroyed || currentHealth >= maxHealth || isRepairing) return;

        if (Time.time < lastRepairTime + repairCooldown)
        {
            Debug.Log("⏳ Repair cooldown active.");
            return;
        }

        if (playerTransform == null || Vector2.Distance(transform.position, playerTransform.position) > repairRange)
        {
            Debug.Log("🚫 Player is too far to repair this turret.");
            return;
        }

        if (!PlayerStats.Instance.SpendCurrency(repairCost))
        {
            Debug.Log("❌ Not enough currency.");
            return;
        }

        StartCoroutine(RepairProcess());
    }

    private IEnumerator RepairProcess()
    {
        isRepairing = true;
        Debug.Log("🔧 Repairing...");

        yield return new WaitForSeconds(repairDuration);

        currentHealth = Mathf.Min(currentHealth + repairAmount, maxHealth);
        lastRepairTime = Time.time;
        isRepairing = false;

        UpdateHealthUI();

        Debug.Log("✅ Repaired! Health: " + currentHealth + "/" + maxHealth);
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (healthText != null)
            healthText.text = $"{currentHealth} / {maxHealth}";

        // Reset the rotation and scale of the UI to prevent flipping
        if (towerHPUI != null)
        {
            towerHPUI.transform.rotation = Quaternion.identity;
            towerHPUI.transform.localScale = new Vector3(2, 2, 1);
        }
    }

    private void CreateHealthBar()
    {
        if (healthBarPrefab == null) return;

        GameObject canvasInstance = Instantiate(healthBarPrefab, transform);
        towerHPUI = canvasInstance.transform.Find("TowerHP")?.gameObject;

        healthSlider = towerHPUI?.GetComponentInChildren<Slider>();
        healthText = towerHPUI?.GetComponentInChildren<TextMeshProUGUI>();

        if (towerHPUI != null)
            towerHPUI.SetActive(false);
    }

    void OnMouseEnter()
    {
        if (towerHPUI != null)
            towerHPUI.SetActive(true);
    }

    void OnMouseExit()
    {
        if (towerHPUI != null)
            towerHPUI.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = turretFaction == TurretFaction.Ally ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, repairRange);
    }
}