using System.Linq.Expressions;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private Vector2 respawnPoint; // Stores the respawn position


    [SerializeField]
    int currentHealth, maxHealth, currentExperience, maxExperience, currentLevel, attackDamage, Speed, Currency, skillPoints;

    private void Start()
    {
        respawnPoint = transform.position; // Set initial spawn point
    }

    private void OnEnable()
    {
        ExperienceManager.Instance.OnChange += HandleChange;
    }
    private void OnDisable()
    {
        ExperienceManager.Instance.OnChange -= HandleChange;
    }

    private void HandleChange(int newExperience, int newCurrency)
    {
        currentExperience += newExperience;
        Currency += newCurrency;
        if (currentExperience >= maxExperience)
        {
            LevelUp();
        }
    }
    private void LevelUp()
    {
        skillPoints += 1;
        currentHealth = maxHealth;
        currentExperience = 0;
        maxExperience += 100;
        currentLevel += 1;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // DEATH LOGIC <...>

        Respawn();
    }

    private void Respawn()
    {
        transform.position = respawnPoint; // Move player to respawn point
        currentHealth = maxHealth; // Restore health
        Debug.Log("Player respawned!");

    }
}
