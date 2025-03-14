using System.Linq.Expressions;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField]
    int currentHealth, maxHealth, currentExperience, maxExperience, currentLevel, attackDamage, Speed, Currency, skillPoints;



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
}
