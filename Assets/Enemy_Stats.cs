using System;
using UnityEngine;

public class Enemy_Stats : MonoBehaviour
{
    [SerializeField] float health, MaxHealth = 3f;

    [SerializeField] float moveSpeed = 5f;

    Rigidbody2D rb;

    int expAmount = 10;
    int currencyAmount = 10;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        ExperienceManager.Instance.Add(currencyAmount,expAmount);
    }
}
