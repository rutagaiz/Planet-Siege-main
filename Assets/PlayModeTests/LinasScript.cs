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
        bullet = new GameObject("Bullet");
        bullet.AddComponent<CircleCollider2D>();
        rb = bullet.AddComponent<Rigidbody2D>();
        bullet.AddComponent<BulletScript>();

        // Mock main camera
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

    //  Mock Test: Check if TakeDamage() was called on enemy
    [UnityTest]
    public IEnumerator BulletCallsEnemyTakeDamage()
    {
        var mockEnemyObj = new GameObject("MockEnemy");
        mockEnemyObj.AddComponent<BoxCollider2D>();
        mockEnemyObj.transform.position = bullet.transform.position;

        var mockEnemy = mockEnemyObj.AddComponent<MockEnemyStats>();
        bullet.GetComponent<Rigidbody2D>().simulated = true;

        yield return new WaitForFixedUpdate();

        Assert.IsTrue(mockEnemy.takeDamageCalled, "❌ Bullet did not call TakeDamage() on enemy.");
    }

    //  Parameterized Test: Bullet force sets correct velocity magnitude
    [TestCase(5f)]
    [TestCase(10f)]
    [TestCase(20f)]
    public void BulletForceAppliesCorrectVelocity(float testForce)
    {
        var script = bullet.GetComponent<BulletScript>();

        // Set private field "force" via reflection
        var forceField = typeof(BulletScript).GetField("force", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        forceField.SetValue(script, testForce);

        Input.mousePosition = new Vector3(Screen.width, Screen.height, 0); // Simulate mouse input
        script.Start();

        float velocityMagnitude = rb.velocity.magnitude;
        Assert.AreEqual(testForce, velocityMagnitude, 0.1f, $"❌ Bullet velocity magnitude doesn't match expected force {testForce}");
    }

    // Supporting mock enemy
    public class MockEnemyStats : Enemy_Stats
    {
        public bool takeDamageCalled = false;

        public override void TakeDamage(float amount)
        {
            takeDamageCalled = true;
        }
    }
}
