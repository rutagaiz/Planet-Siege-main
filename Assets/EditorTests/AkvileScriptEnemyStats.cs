using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

public class EnemyStats_EditModeTests
{
    private GameObject enemyObj;
    private Enemy_Stats enemy;
    private Rigidbody2D rb;

    [SetUp]
    public void Setup()
    {
        enemyObj = new GameObject("Enemy");
        enemy = enemyObj.AddComponent<Enemy_Stats>();
        enemyObj.AddComponent<GUnitGunScript>();
        rb = enemyObj.AddComponent<Rigidbody2D>();
        enemy.ManualInitForTesting();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(enemyObj);
    }

    // ✅ Parametrizuotas testas: tikrina ar žala sumažina sveikatą
    [TestCase(3f, 1f, 2f)]
    [TestCase(5f, 2f, 3f)]
    [TestCase(4.5f, 1.5f, 3f)]
    public void TakeDamage_ReducesHealth_Param(float startHealth, float damage, float expectedHealth)
    {
        typeof(Enemy_Stats)
            .GetField("health", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(enemy, startHealth);

        enemy.TakeDamage(damage);

        float newHealth = (float)typeof(Enemy_Stats)
            .GetField("health", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(enemy);

        Assert.AreEqual(expectedHealth, newHealth, 0.01f);
    }

    // ✅ Testas: sveikata niekada nenukrenta žemiau 0
    [Test]
    public void TakeDamage_DoesNotGoBelowZero()
    {
        typeof(Enemy_Stats)
            .GetField("health", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(enemy, 2f);

        enemy.TakeDamage(999); // labai daug žalos

        float newHealth = (float)typeof(Enemy_Stats)
            .GetField("health", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(enemy);

        Assert.GreaterOrEqual(newHealth, 0f);
    }

    // ✅ Testas: kai neturi taikinio – juda
    [Test]
    public void FixedUpdate_Moves_WhenNoTarget()
    {
        rb.linearVelocity = Vector2.zero;

        enemy.FixedUpdate();

        Assert.Greater(rb.linearVelocity.magnitude, 0f);
    }

    // ✅ Testas: kai turi taikinį – sustoja
    [Test]
    public void FixedUpdate_Stops_WhenHasTarget()
    {
        rb.linearVelocity = Vector2.right * 2f;

        var gun = enemy.GetComponent<GUnitGunScript>();
        gun.HasTarget = true;

        enemy.FixedUpdate();

        Assert.AreEqual(Vector2.zero, rb.linearVelocity);
    }
}
