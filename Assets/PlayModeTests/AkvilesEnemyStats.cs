using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyStats_PlayModeTests
{
    private GameObject enemyObj;
    private Enemy_Stats enemy;

    private GameObject gameManagerObj;
    private TestGameManager testGM;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        // Sukuriamas priešas su būtinais komponentais
        enemyObj = new GameObject("Enemy");
        enemy = enemyObj.AddComponent<Enemy_Stats>();
        enemyObj.AddComponent<GUnitGunScript>();
        enemyObj.AddComponent<Rigidbody2D>();
        enemy.ManualInitForTesting();

        // Sukuriamas tik GameManager mock'as
        gameManagerObj = new GameObject("GM");
        testGM = gameManagerObj.AddComponent<TestGameManager>();
        GameManager.Instance = testGM;

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        Object.Destroy(enemyObj);
        Object.Destroy(gameManagerObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Die_TriggersGameManagerOnly()
    {
        // Palaukiam, kol priešas bus aktyvus
        yield return new WaitUntil(() => enemy != null && enemy.gameObject.activeInHierarchy);

        // Sukeliam mirtį
        enemy.TakeDamage(999);

        // Duodam laiko Die() metodui
        yield return new WaitForSeconds(0.2f);

        // Tikrinam ar GameManager metodas buvo iškviestas
        Assert.IsTrue(testGM.defeatedCalled, "❌ GameManager.AddEnemyDefeated() nebuvo iškviestas");
    }

    // Testinis GameManager
    public class TestGameManager : GameManager
    {
        public bool defeatedCalled = false;

        public override void AddEnemyDefeated()
        {
            defeatedCalled = true;
        }
    }
}
