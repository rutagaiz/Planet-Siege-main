using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private Vector2 respawnPoint; // Stores the respawn position
    public Slider slider;
    public Slider slider1;


    [SerializeField]
    int currentHealth, maxHealth, currentExperience, maxExperience, currentLevel, attackDamage, Speed, Currency, skillPoints;

    private void Start()
    {
        respawnPoint = transform.position; // Set initial spawn point
        UpdateHealthUI();
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
        UpdateXpUI();
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
        UpdateHealthUI();
        UpdateXpUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
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

    private void UpdateHealthUI()
    {
        if (slider != null)
        {
            slider.value = (float)currentHealth;
        }
    }

    private void UpdateXpUI()
    {
        if (slider1 != null)
        {
            slider1.value = (float)currentExperience;
        }
    }
}
