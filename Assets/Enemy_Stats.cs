using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Stats : MonoBehaviour
{
    public static event Action<Enemy_Stats> OnEnemyKilled;
    private GUnitGunScript GUnitGunScript;
    private bool HasTarget; 
    [SerializeField] float health, MaxHealth = 3f;
    public GameObject popUpDamagePrefab;
    public TMP_Text popUpText;
    public float popupYOffset = 1f;
    private HealthBarUI healthUI;
    [SerializeField] private GameObject healthBarPrefab;

    [SerializeField] float moveSpeed = 5f;

    public Rigidbody2D rb;

    [SerializeField]
    int expAmount = 10;
    int currencyAmount = 10;

    [Header("PowerUps")]
    public bool isSpecial = false;
    public List<PowerUp> powerUps = new List<PowerUp>();
    private Vector2 knockbackForce = new Vector2(5f, 5f);

    public void FixedUpdate()
    {
        HasTarget = GUnitGunScript.HasTarget;
        if (rb.linearVelocity.magnitude <= moveSpeed && HasTarget == false)
        {
            rb.AddForce(Vector2.left, ForceMode2D.Impulse);
        }
        else rb.linearVelocity = Vector2.zero;
    }

    private void Awake()
    {
        GUnitGunScript = GetComponent<GUnitGunScript>();
        HasTarget = GUnitGunScript.HasTarget;
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void Start()
    {
        health = MaxHealth;
        
        // GameObject canvasInstance = Instantiate(healthBarPrefab, transform);
        // canvasInstance.transform.localPosition = new Vector3(0, 0.5f, 0); // or whatever offset works
        // healthUI = canvasInstance.GetComponentInChildren<HealthBarUI>();
        // healthUI.Initialize(transform, (int)MaxHealth);
        
        GameObject healthBarInstance = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        healthBarInstance.transform.SetParent(transform); // Make enemy the parent
    
        healthUI = healthBarInstance.GetComponentInChildren<HealthBarUI>();
        if (healthUI == null)
        {
            Debug.LogError("HealthBarUI component not found on health bar prefab!");
            return;
        }
    
        healthUI.Initialize(transform, MaxHealth);
        healthUI.SetHealth(health, MaxHealth); // Explicitly set initial health
    }

    public void TakeDamage(float damageAmount)
    {
        if (Random.Range(0,100) == 0 || Random.Range(0, 100) == 1) // 2% critical
        {
            damageAmount *= 2;
        }
        health -= damageAmount;
        popUpText.text = damageAmount.ToString();
        health = Mathf.Clamp(health, 0, MaxHealth); 
        if (healthUI != null)
        {
            healthUI.SetHealth(health, MaxHealth);
        }
        else
        {
            Debug.LogWarning("HealthUI reference is null!");
        }

        Vector2 hitDirection = transform.position.normalized;
        rb.AddForce(hitDirection * 2, ForceMode2D.Impulse);
        
        GameObject popup = Instantiate(popUpDamagePrefab, transform.position, Quaternion.identity);
        DamagePopUp popupScript = popup.AddComponent<DamagePopUp>();
        popupScript.target = transform;
        popupScript.offset = new Vector3(0, 1f, 0);
        
        if (health <= 0)
        {
            Die();
        }
    }



    void Die()
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


            Destroy(gameObject);
        ExperienceManager.Instance.Add(expAmount, currencyAmount);
        GameManager.Instance.AddEnemyDefeated(); // Notify stats manager
    }

    void ShowPowerUp(GameObject powerUp)
    {
        if(powerUp)
        {
            GameObject droppedLoot = Instantiate(powerUp, transform.position, Quaternion.identity);

        }
    }

    public void ManualInitForTesting()
    {
        Awake();  // kviečia lokaliai (nes klasėje)
        Start();
    }
}