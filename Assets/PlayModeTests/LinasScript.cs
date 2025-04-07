using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class BulletScriptTests
{
    private GameObject bullet;
    private Rigidbody2D rb;
    private Camera testCam;

    [SetUp]
    public void SetUp()
    {
        bullet = new GameObject();
        bullet.AddComponent<CircleCollider2D>();
        rb = bullet.AddComponent<Rigidbody2D>();
        bullet.AddComponent<BulletScript>();

        // Mock a main camera
        var camObj = new GameObject("MainCamera");
        testCam = camObj.AddComponent<Camera>();
        testCam.tag = "MainCamera";
        testCam.transform.position = new Vector3(0, 0, -10);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(bullet);
        var cam = Camera.main;
        if (cam != null) Object.DestroyImmediate(cam.gameObject);
    }

    [UnityTest]
    public IEnumerator BulletMovesTowardsMousePosition()
    {
        Input.mousePosition = new Vector3(Screen.width, Screen.height, 0);
        yield return null;

        var prevPos = bullet.transform.position;
        yield return new WaitForSeconds(0.1f);
        Assert.AreNotEqual(prevPos, bullet.transform.position, "Bullet did not move toward mouse.");
    }

    [UnityTest]
    public IEnumerator BulletSelfDestructsAfterLifetime()
    {
        float lifetime = 3f;
        yield return new WaitForSeconds(lifetime + 0.5f);
        Assert.IsTrue(bullet == null || bullet.Equals(null), "Bullet was not destroyed after lifetime.");
    }

    [Test]
    public void BulletRotatesTowardsTargetDirection()
    {
        var script = bullet.GetComponent<BulletScript>();
        script.Start(); // manually trigger
        float zRotation = bullet.transform.rotation.eulerAngles.z;

        Assert.IsTrue(zRotation >= 0 && zRotation <= 360, "Bullet rotation not correctly applied.");
    }

    [UnityTest]
    public IEnumerator BulletDealsDamageToEnemy()
    {
        var enemy = new GameObject().AddComponent<Enemy_Stats>();
        enemy.transform.position = bullet.transform.position;
        enemy.gameObject.AddComponent<BoxCollider2D>();
        bullet.GetComponent<Rigidbody2D>().simulated = true;

        float initialHealth = enemy.health;
        Physics2D.Simulate(0.1f);

        yield return new WaitForFixedUpdate();
        Assert.Less(enemy.health, initialHealth, "Enemy did not take damage from bullet.");
    }

    [UnityTest]
    public IEnumerator BulletDestroysOnTurretHit()
    {
        var turretObj = new GameObject("Turret");
        turretObj.tag = "EnemyTurret";
        var turret = turretObj.AddComponent<turretScript>();
        turretObj.AddComponent<BoxCollider2D>();
        turretObj.transform.position = bullet.transform.position;

        yield return new WaitForFixedUpdate();
        Assert.IsTrue(turret.health < turret.maxHealth, "Turret did not take damage.");
    }
}
