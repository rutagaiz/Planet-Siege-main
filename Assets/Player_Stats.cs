using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class Player_Stats : MonoBehaviour
{

    public event Action<int> OnDamageChanged;

    public static Player_Stats Instance;

    private Vector2 respawnPoint;
    public Slider slider;
    public Slider slider1;
    public Text currencyText;
    public Text skillText;

    //[SerializeField]
    //public int Currency = 100;

    [SerializeField]
    int currentHealth, maxHealth, maxExperience = 100, currentLevel, attackDamage, Speed, skillPoints;

    public GameOverScreen GameOverScreen;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        respawnPoint = transform.position;
        UpdateHealthUI();
        UpdateXpUI();
        UpdateCurrencyUI();
        // Skill points UI
        UpdateSkillUI();
        Medkit.OnMedkitCollect += TakeDamage;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Keypad1) && skillPoints >= 1)
        {
            skillPoints--;
            maxHealth += 10;
            currentHealth = maxHealth;
            UpdateSkillUI();
        }
        if (Input.GetKey(KeyCode.Keypad2) && skillPoints >= 1)
        {
            skillPoints--;
            attackDamage += 1;
            OnDamageChanged?.Invoke(attackDamage);
            UpdateSkillUI();
        }
        if (Input.GetKey(KeyCode.Keypad3) && skillPoints >= 1)
        {
            skillPoints--;
            Speed += 1;
            UpdateSkillUI();
        }
    }

    private void OnEnable()
    {
        ExperienceManager.Instance.OnChange += HandleChange;
    }

    private void OnDisable()
    {
        ExperienceManager.Instance.OnChange -= HandleChange;
    }

    public void HandleChange(int newCurrency, int newExperience)
    {
        GameManager.Instance.AddXP(newExperience);
        GameManager.Instance.AddCoin(newCurrency);
        UpdateXpUI();
        UpdateCurrencyUI();

        if (GameManager.Instance.XP >= maxExperience)
        {
            LevelUp();
            UpdateSkillUI();
        }
    }

    private void LevelUp()
    {
        currentHealth = maxHealth;
        GameManager.Instance.ResetXP();
        maxExperience += 20;
        currentLevel += 1;
        skillPoints += 1;
        UpdateHealthUI();
        UpdateXpUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
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
            slider1.value = GameManager.Instance.XP;
        }
    }


    public bool SpendCurrency(int amount)
    {
        if (GameManager.Instance.coinCount >= amount)
        {
            GameManager.Instance.coinCount -= amount;
            UpdateCurrencyUI();
            return true;
        }
        return false;
    }

    public void AddCurrency(int amount)
    {
        GameManager.Instance.AddCoin(amount);
        UpdateCurrencyUI();
    }

    // Skill pointsam Add()
    public void AddSkillPoint(int amount)
    {
        GameManager.Instance.AddSkill(amount);
    }

    private void UpdateCurrencyUI()
    {
        if (currencyText != null)
        {
            currencyText.text = "Coin count: " + GameManager.Instance.coinCount;
        }
    }

    // Skill pointsam UI tekstas
    private void UpdateSkillUI()
    {
        if (skillText != null)
        {
            skillText.text = "Skill points: " + skillPoints;
        }
    }
 
    public int GetCurrentDamage()
    {
        return attackDamage;
    }   
}