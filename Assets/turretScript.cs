using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;

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
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    // Kintamieji kad trackint kiek toweriu nuimta
    public static event Action<int> OnTowerDestroyed;
    private static int destroyedTowerCount = 0;

    // Victory / Defeat screenai
    public GameOverScreen GameOverScreen;
    public Victory_screen Victory_screen;

    [Header("PowerUps")]
    public bool isSpecial = false;
    public List<PowerUp> powerUps = new List<PowerUp>();
    
    // Issaugot zaidima
    public int GetCurrentHealth() => currentHealth;
    public bool IsDestroyed() => isDestroyed;

    public void Initialize() //for testing, does what start did previously but public possible call
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        UpdateHealthUI();
    }



    void Awake()
    {
        CreateHealthBar();
    }

    void Start()
    {
        Initialize();
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

    public void Shoot()
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
        if (isSpecial)
        {
            foreach (PowerUp powerUp in powerUps)
            {
                ShowPowerUp(powerUp.itemPrefab);
            }
        }
        else
        {
            foreach (PowerUp powerUp in powerUps)
            {
                if (Random.Range(0f, 100f) <= powerUp.dropChance)
                {
                    ShowPowerUp(powerUp.itemPrefab);
                    break;
                }
            }
        }
        isDestroyed = true;
        Debug.Log($"{turretFaction} turret destroyed!");
        
        if (turretFaction == TurretFaction.Enemy)
        {
            GameManager.Instance.AddTowerDestroyed(gameObject);
        }
        
        // 🧨 1. Sunaikinam visus koliderius (vaikų taip pat!)
        foreach (var col in GetComponentsInChildren<Collider2D>())
        {
            Destroy(col);
        }

        // 🧨 2. Sunaikinam visus Rigidbody2D (jei yra)
        foreach (var rb in GetComponentsInChildren<Rigidbody2D>())
        {
            Destroy(rb);
        }

        // 🧨 3. Jei turi Tilemap ar navmesh komponentus
        foreach (var comp in GetComponentsInChildren<Behaviour>())
        {
            if (comp.GetType().Name.Contains("Tilemap") || comp.GetType().Name.Contains("NavMesh"))
            {
                Destroy(comp);
            }
        }

        // 4. Išjungiam šaudymą (vizualiai)
        if (Gun != null)
        {
            Destroy(Gun);
        }

        // 5. Pakeičiam spalvą – liktų kaip "griuvėsiai"
        if (sr != null)
        {
            sr.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            sr.sortingOrder = -1;
        }

        // 6. UI
        if (towerHPUI != null)
            towerHPUI.SetActive(false);

        // 7. Statistikos + win/lose
        destroyedTowerCount++;
        OnTowerDestroyed?.Invoke(destroyedTowerCount);

        if (CompareTag("EnemyBase"))
        {
            Debug.Log("✅ Pergalė – EnemyBase sunaikinta");
            ShowVictoryScreen();
        }

        if (CompareTag("AllyBase"))
        {
            Debug.Log("❌ Pralaimėjimas – AllyBase sunaikinta");
            ShowDefeatScreen();
        }

        gameObject.tag = "Untagged";
    }

    void ShowPowerUp(GameObject powerUp)
    {
        if (powerUp)
        {
            GameObject droppedLoot = Instantiate(powerUp, transform.position, Quaternion.identity);

        }
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

        if (!Player_Stats.Instance.SpendCurrency(repairCost))
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

    void ShowVictoryScreen()
    {
        Victory_screen.Setup();
    }
    void ShowDefeatScreen()
    {
        GameOverScreen.Setup();
    }
    
    public void SetHealth(int hp)
    {
        currentHealth = hp;
        isDestroyed = (hp <= 0);
        UpdateHealthUI();

        if (isDestroyed)
            Die(); // or call internal logic without duplicating
    }
    public void ForceDestroy()
    {
        if (!isDestroyed)
            Die();
    }
}