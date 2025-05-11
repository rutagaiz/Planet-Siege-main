using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ShootingTests
{
    private GameObject shootingObject;
    private Shooting shooting;
    private GameObject mockBullet;
    private Transform bulletSpawnPoint;

    [SetUp]
    public void Setup()
    {
        shootingObject = new GameObject("Shooter");
        shooting = shootingObject.AddComponent<Shooting>();

        // Setup mock bullet
        mockBullet = new GameObject("MockBullet");
        shooting.bullet = mockBullet;

        // Setup bullet spawn point
        var bulletSpawn = new GameObject("BulletSpawnPoint");
        bulletSpawnPoint = bulletSpawn.transform;
        shooting.bulletTransform = bulletSpawnPoint;

        // Avoid null pointer on camera
        new GameObject("MainCamera").AddComponent<Camera>().tag = "MainCamera";

        // Inject SoundManager singleton
        var soundManagerObj = new GameObject("SoundManager");
        var soundManager = soundManagerObj.AddComponent<SoundManager>();
        soundManager.Gun = soundManagerObj.AddComponent<AudioSource>();
        SoundManager.Instance = soundManager;

        // Initialize shooting state
        shooting.timeBetweenFiring = 0.5f;
        shooting.canFire = true;

        shootingObject.transform.position = Vector3.zero;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(shootingObject);
        Object.DestroyImmediate(mockBullet);
        Object.DestroyImmediate(bulletSpawnPoint.gameObject);
        Object.DestroyImmediate(Camera.main?.gameObject);
        Object.DestroyImmediate(SoundManager.Instance?.gameObject);
    }

    [UnityTest]
    public IEnumerator TestBulletIsFired_WhenCanFireAndAmmoAvailable()
    {
        int initialAmmo = 5;
        var maxAmmoField = shooting.GetType().GetField("maxAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        maxAmmoField.SetValue(shooting, 10);

        var currentAmmoField = shooting.GetType().GetField("currentAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        currentAmmoField.SetValue(shooting, initialAmmo);

        shooting.TryFireManually();

        yield return null; // Wait a frame for Instantiate

        Assert.AreEqual(initialAmmo - 1, currentAmmoField.GetValue(shooting));
    }

    [UnityTest]
    public IEnumerator TestCannotFire_WhenNoAmmo()
    {
        var currentAmmoField = shooting.GetType().GetField("currentAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        currentAmmoField.SetValue(shooting, 0);

        bool bulletFired = false;
        Shooting.OnBulletInstantiated = (obj, pos, rot) => bulletFired = true;

        shooting.TryFireManually();
        yield return null;

        Assert.IsFalse(bulletFired, "Bullet should not be fired when ammo is 0.");

        Shooting.OnBulletInstantiated = null; // Clean up
    }


    [UnityTest]
    public IEnumerator TestCooldownPreventsFiring()
    {
        var currentAmmoField = shooting.GetType().GetField("currentAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        currentAmmoField.SetValue(shooting, 5);

        shooting.canFire = false;
        shooting.timer = 0;
        shooting.timeBetweenFiring = 2f;

        shooting.TryFireManually();
        yield return null;

        Assert.AreEqual(5, currentAmmoField.GetValue(shooting)); // Ammo unchanged
    }

    [UnityTest]
    public IEnumerator TestAddAmmoRespectsMaxLimit()
    {
        var maxAmmoField = shooting.GetType().GetField("maxAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        maxAmmoField.SetValue(shooting, 10);

        var currentAmmoField = shooting.GetType().GetField("currentAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        currentAmmoField.SetValue(shooting, 9);

        shooting.AddAmmo(5);
        yield return null;

        Assert.AreEqual(10, currentAmmoField.GetValue(shooting));
    }

    [TestCase(5, 3, 8)]     // Add 3 to 5 -> 8
    [TestCase(9, 5, 10)]    // Add 5 to 9 -> cap at 10
    [TestCase(0, 10, 10)]   // Fill from empty
    [TestCase(10, 1, 10)]   // Already full
    public void AddAmmo_ShouldClampToMax(int startingAmmo, int toAdd, int expected)
    {
        var maxAmmoField = shooting.GetType().GetField("maxAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        maxAmmoField.SetValue(shooting, 10);

        var currentAmmoField = shooting.GetType().GetField("currentAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        currentAmmoField.SetValue(shooting, startingAmmo);

        shooting.AddAmmo(toAdd);

        int resultAmmo = (int)currentAmmoField.GetValue(shooting);
        Assert.AreEqual(expected, resultAmmo);
    }
    [TestCase(true, 5, true)]     // Can fire, has ammo -> should fire
    [TestCase(false, 5, false)]   // Can't fire yet
    [TestCase(true, 0, false)]    // No ammo
    [TestCase(false, 0, false)]   // Neither condition met
    public void TryFireManually_FiresOnlyWhenReady(bool canFireFlag, int ammo, bool expectedToFire)
    {
        var currentAmmoField = shooting.GetType().GetField("currentAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        currentAmmoField.SetValue(shooting, ammo);

        shooting.canFire = canFireFlag;

        bool fired = false;
        Shooting.OnBulletInstantiated = (_, __, ___) => fired = true;

        shooting.TryFireManually();

        Assert.AreEqual(expectedToFire, fired);
        Shooting.OnBulletInstantiated = null;
    }


}
