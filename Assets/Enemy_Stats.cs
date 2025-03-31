using System;
using UnityEngine;

public class Enemy_Stats : MonoBehaviour
{
    public static event Action<Enemy_Stats> OnEnemyKilled;
    [SerializeField] float health, MaxHealth = 3f;

    [SerializeField] float moveSpeed = 5f;

    public Rigidbody2D rb;

    int expAmount = 10;
    int currencyAmount = 10;

    public void FixedUpdate()
    {
        rb.linearVelocity = Vector2.left * moveSpeed;
    }

    private void Awake()
    {
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
        Destroy(gameObject);
        ExperienceManager.Instance.Add(expAmount, currencyAmount);
        GameManager.Instance.AddEnemyDefeated(); // Notify stats manager
    }
}