using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    private Vector2 respawnPoint;
    public Slider slider;
    public Slider slider1;

    [SerializeField]
    public int Currency = 100;

    [SerializeField]
    int currentHealth, maxHealth, currentExperience, maxExperience, currentLevel, attackDamage, Speed, skillPoints;

    public GameOverScreen GameOverScreen;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        respawnPoint = transform.position;
        UpdateHealthUI();
        Medkit.OnMedkitCollect += TakeDamage;
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
            GameOverScreen.Setup();
        }
    }

    private void Die()
    {
        Respawn();
        Debug.LogError("Player has died");
    }

    private void Respawn()
    {
        transform.position = respawnPoint;
        currentHealth = maxHealth;
        Debug.LogError("Player has respawned");
    }

    private void UpdateHealthUI()
    {
        if (slider != null)
        {
            slider.value = currentHealth;
        }
    }

    private void UpdateXpUI()
    {
        if (slider1 != null)
        {
            slider1.value = currentExperience;
        }
    }


    public bool SpendCurrency(int amount)
    {
        if (Currency >= amount)
        {
            Currency -= amount;
            return true;
        }
        return false;
    }

    public void AddCurrency(int amount)
    {
        Currency += amount;
    }
}