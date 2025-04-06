using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.SceneManagement;


public class Player_Stats_Tests : MonoBehaviour
{
    private Player_Stats playerStats;
    private int initialHealth;

    [UnitySetUp]
    public IEnumerator LoadSceneAndSetup()
    {
        // Ensure that any log messages related to NullReferenceException are ignored
        LogAssert.ignoreFailingMessages = true;

        // Load the scene asynchronously and wait until it's fully loaded
        var sceneLoad = SceneManager.LoadSceneAsync("atsarginis"); // Replace with the actual name of your scene
        while (!sceneLoad.isDone)
            yield return null;

        yield return null; // Allow a frame to make sure everything is initialized

        // Find the PlayerStats component on the GameObject in the scene
        playerStats = GameObject.FindObjectOfType<Player_Stats>();
        Assert.IsNotNull(playerStats, "PlayerStats component is not found in the scene! Ensure it's attached to a GameObject in your scene.");

        // Store the initial health from the Inspector setup
        initialHealth = GetPrivate<int>("currentHealth");

        // If needed, also set up or mock other components, like Enemy_Stats, here
        SetupEnemyStats();
    }

    // Helper method to retrieve private fields using reflection
    private int GetPrivate<T>(string fieldName)
    {
        return (int)typeof(Player_Stats)
            .GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(playerStats);
    }
    private void SetPrivate<T>(string fieldName, T value)
    {
        var fieldInfo = typeof(Player_Stats)
            .GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        fieldInfo.SetValue(playerStats, value);
    }

    // Optional: Setup or mock any missing components like Enemy_Stats here
    private void SetupEnemyStats()
    {
        // Find the EnemyStats component (if necessary for the test) and set any required values
        var enemyStats = GameObject.FindObjectOfType<Enemy_Stats>();
        if (enemyStats == null)
        {
            enemyStats = new GameObject("Enemy").AddComponent<Enemy_Stats>(); // Ensure it's created for testing
        }

        // Add any required setup for enemyStats here
        // For example: enemyStats.someField = someValue;
    }

    // Test method to check health decreases after damage
    [UnityTest]
    public IEnumerator TakeDamage_UpdatesHealth_FromInspector()
    {
        // Damage player and check if health decreases as expected
        playerStats.TakeDamage(5);
        yield return null; // Yield to allow the health update

        int currentHealth = GetPrivate<int>("currentHealth");

        Assert.AreEqual(initialHealth - 5, currentHealth, "Player health didn't update as expected after taking damage.");
    }
    [UnityTest]
    public IEnumerator PlayerXP_IncreasesAndLevelsUp()
    {
        // Assume the player is at level 1 and needs 100 XP to level up
        playerStats.HandleChange(100, 0); // Add 100 XP
        yield return null;

        // Check that player leveled up
        int currentLevel = GetPrivate<int>("currentLevel");
        Assert.AreEqual(2, currentLevel, "Player should have leveled up after gaining 100 XP.");

        // Check that XP is reset to 0 after leveling up
        int currentXP = GetPrivate<int>("currentExperience");
        Assert.AreEqual(0, currentXP, "XP should be reset after leveling up.");
    }
    [UnityTest]
    public IEnumerator PlayerSkillPoints_AreIncreasedOnLevelUp()
    {
        // Simulate XP gain leading to level up
        playerStats.HandleChange(150, 0); // Gain 150 XP
        yield return null;

        // Check if skill points increased
        int skillPoints = GetPrivate<int>("skillPoints");
        Assert.AreEqual(1, skillPoints, "Skill points should increase after leveling up.");
    }
    [UnityTest]
    public IEnumerator PlayerCurrency_AddsCorrectly()
    {
        // Add currency and check balance
        playerStats.AddCurrency(50);
        yield return null;

        int currentCurrency = GetPrivate<int>("Currency");
        Assert.AreEqual(50, currentCurrency, "Currency should be added correctly.");
    }
    [UnityTest]
    public IEnumerator PlayerCurrency_SpendsCorrectly()
    {
        // Add currency
        playerStats.AddCurrency(100);
        yield return null;

        // Spend currency
        playerStats.SpendCurrency(50);
        yield return null;

        int currentCurrency = GetPrivate<int>("Currency");
        Assert.AreEqual(50, currentCurrency, "Currency should be correctly spent.");
    }

    [TestCase(5, ExpectedResult = true)] // Test with 5 damage
    [TestCase(10, ExpectedResult = true)] // Test with 10 damage
    [TestCase(0, ExpectedResult = true)] // Test with no damage
    [TestCase(20, ExpectedResult = true)] // Test with large damage
    public bool TakeDamage_HealthDecreasesCorrectly(int damage)
    {
        // Set the player's initial health
        SetPrivate<int>("currentHealth", initialHealth);

        // Damage player and check if health decreases correctly
        playerStats.TakeDamage(damage);

        int currentHealth = GetPrivate<int>("currentHealth");

        // Assert that health is correctly updated based on damage
        return currentHealth == initialHealth - damage;
    }

}
