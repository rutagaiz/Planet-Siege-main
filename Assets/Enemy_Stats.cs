using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Stats : MonoBehaviour
{
    public static event Action<Enemy_Stats> OnEnemyKilled;
    private GUnitGunScript GUnitGunScript;
    private bool HasTarget; 
    [SerializeField] float health, MaxHealth = 3f;

    [SerializeField] float moveSpeed = 5f;

    public Rigidbody2D rb;

    [SerializeField]
    int expAmount = 10;
    int currencyAmount = 10;

    [Header("PowerUps")]
    public List<PowerUp> powerUps = new List<PowerUp>();

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
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

   
    
    void Die()
    {
        foreach(PowerUp powerUp in powerUps)
        {
            if (Random.Range(0f, 100f) <= powerUp.dropChance)
            {
                ShowPowerUp(powerUp.itemPrefab);
                break;
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
}