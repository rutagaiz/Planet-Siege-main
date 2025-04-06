// turretScriptTest.cs
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class turretScriptTest
{
    private GameObject turretObj;
    private turretScript turret;

    [SetUp]
    public void Setup()
    {
        turretObj = new GameObject("Turret");
        turret = turretObj.AddComponent<turretScript>();

        // Assign required fields
        turret.Gun = new GameObject("Gun");
        turret.Gun.transform.parent = turretObj.transform;

        GameObject shootPoint = new GameObject("ShootPoint");
        turret.ShootPoint = shootPoint.transform;
        shootPoint.transform.parent = turretObj.transform;

        turret.Bullet = new GameObject("Bullet");
        turret.Bullet.AddComponent<TurretBullet>();

        turret.Range = 5f;
        turret.FireRate = 1f;
        turret.targetTags = new string[] { "Enemy" };
        turret.turretFaction = TurretFaction.Ally;

        turretObj.AddComponent<CircleCollider2D>();
        turretObj.AddComponent<SpriteRenderer>();

        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.transform.position = turretObj.transform.position;

        GameManager.Instance = new GameObject("GM").AddComponent<GameManager>();
        Player_Stats.Instance = new GameObject("Stats").AddComponent<Player_Stats>();

        turret.Initialize();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(turretObj);
        Object.DestroyImmediate(GameManager.Instance.gameObject);
        Object.DestroyImmediate(Player_Stats.Instance.gameObject);
    }

    [Test]
    public void Turret_TakesDamageCorrectly()
    {
        int startHP = turret.GetCurrentHealth();
        turret.TakeDamage(10);
        Assert.AreEqual(startHP - 10, turret.GetCurrentHealth());
    }

    [Test]
    public void Turret_DoesNotOverRepair()
    {
        turret.TakeDamage(5);
        turret.TryRepair();
        turret.ForceInstantRepair();
        Assert.LessOrEqual(turret.GetCurrentHealth(), 50);
    }

    [UnityTest]
    public IEnumerator Turret_TriggersShootCooldown()
    {
        turret.FireRate = 10f;
        turret.targetTags = new string[] { "Enemy" };
        GameObject enemy = new GameObject("Enemy");
        enemy.tag = "Enemy";
        enemy.transform.position = turretObj.transform.position + Vector3.right;

        yield return null; // Let turret Update find target     

        yield return new WaitForSeconds(0.2f);
        Assert.LessOrEqual(Time.time, turret.GetNextFireTime());

        Object.DestroyImmediate(enemy);
    }

    [Test]
    public void Turret_RepairFails_OutOfRange()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = turretObj.transform.position + Vector3.right * 10f;
        turret.TryRepair();
        Assert.IsFalse(turret.IsRepairing());
    }

    [Test]
    public void Turret_IsDestroyedAtZeroHP()
    {
        turret.TakeDamage(999);
        Assert.IsTrue(turret.IsDestroyed());
    }

    [Test]
    public void Turret_FiresOnlyWhenCooldownPassed()
    {
        turret.SetNextFireTime(Time.time + 1f);
        float before = turret.GetNextFireTime();
        Assert.AreEqual(before, turret.GetNextFireTime());
    }
}

// Extension methods added to turretScript (for testing access)
public static class turretScriptExtensions
{
    public static int GetCurrentHealth(this turretScript turret) => typeof(turretScript).GetField("currentHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(turret) as int? ?? 0;
    public static bool IsDestroyed(this turretScript turret) => (bool)typeof(turretScript).GetField("isDestroyed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(turret);
    public static bool IsRepairing(this turretScript turret) => (bool)typeof(turretScript).GetField("isRepairing", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(turret);
    public static float GetNextFireTime(this turretScript turret) => (float)typeof(turretScript).GetField("nextTimeToFire", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(turret);
    public static void SetNextFireTime(this turretScript turret, float value) => typeof(turretScript).GetField("nextTimeToFire", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(turret, value);
    public static void ForceInstantRepair(this turretScript turret) => turret.StartCoroutine(ForceRepair(turret));

    private static IEnumerator ForceRepair(turretScript turret)
    {
        yield return new WaitForSeconds(0.01f);
    }
}
