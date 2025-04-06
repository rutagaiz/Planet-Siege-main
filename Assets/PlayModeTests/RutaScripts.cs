using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;


public class RutaScripts
{
    private GameObject playerObj;
    private Player_movement player;
    private Rigidbody2D rb;
    private GameManager mockGameManager;

    [SetUp]
    public void Setup()
    {
        // Create player with all required components
        playerObj = new GameObject("Player");
        player = playerObj.AddComponent<Player_movement>();
        rb = playerObj.AddComponent<Rigidbody2D>();
        player.rb = rb;

        var playerCollider = playerObj.GetComponent<BoxCollider2D>();
        if (playerCollider == null)
        {
            playerCollider = playerObj.AddComponent<BoxCollider2D>();
        }
        playerCollider.isTrigger = true;

        // Create mock child objects (helmet and torso)
        var helmet = new GameObject("helmet_0");
        helmet.transform.parent = playerObj.transform;
        helmet.AddComponent<SpriteRenderer>();

        var torso = new GameObject("torso");
        torso.transform.parent = playerObj.transform;
        torso.AddComponent<SpriteRenderer>();

        // Mock GameManager
        mockGameManager = new GameObject("GameManager").AddComponent<GameManager>();
        GameManager.Instance = mockGameManager;
        //player.cm = mockGameManager; // Assign to player's reference

        // Initialize components
        player.Invoke("Start", 0); // Manually call Start since we're in test mode
    }

    [TearDown]
    public void Teardown()
    {
        // Cleanup
        Object.DestroyImmediate(playerObj);
        Object.DestroyImmediate(mockGameManager.gameObject);
    }

    // 1. Basic movement test
    [UnityTest]
    public IEnumerator Player_MovesHorizontally_WhenInputReceived()
    {
        // Reset all forces
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0; // Disable gravity for test

        // Test movement
        player.TestMove(1f); // Move right

        // No need to wait for frames since we're calling FixedUpdate directly
        Assert.AreEqual(
            new Vector2(player.moveSpeed, 0f),
            rb.linearVelocity,
            "Player should move horizontally without vertical movement"
        );

        // Restore gravity (important for other tests)
        rb.gravityScale = 1;
        yield return null; // Required for UnityTest
    }

    // 2. Flip test (parameterized)
    [TestCase(true, ExpectedResult = false)] // Face right -> should flip left
    [TestCase(false, ExpectedResult = true)] // Face left -> should flip right
    public bool Player_FlipsSprite_WhenChangingDirection(bool startFacingRight)
    {
        player.facingRight = startFacingRight;
        player.Flip(!startFacingRight);
        return player.facingRight;
    }

    // 3. Vertical movement tests
    [UnityTest]
    public IEnumerator Player_FliesUp_WhenWPressed()
    {
        // Simulate W pressed
        player.simulateWPressed = true;

        // Reset player state
        player.transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;

        // Wait for physics update
        yield return new WaitForFixedUpdate();

        // Verify upward movement
        Assert.Greater(rb.linearVelocity.y, 0f);

        // Clean up
        player.simulateWPressed = false;
    }

    [UnityTest]
    public IEnumerator Player_DoesNotExceedMaxHeight()
    {
        player.transform.position = new Vector3(0, player.maxY, 0);
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(player.maxY, player.transform.position.y);
        Assert.AreEqual(0, rb.linearVelocity.y);
    }

    //// 4. Coin collection test with mock
    //[UnityTest]
    //public IEnumerator Player_CollectsCoin_OnTriggerEnter()
    //{
    //    // Verify GameManager is set up
    //    Assert.IsNotNull(GameManager.Instance, "GameManager instance is null");
    //    Assert.IsNotNull(player.cm, "Player's GameManager reference is null");

    //    int initialCoins = mockGameManager.GetCoins();

    //    // Create test coin
    //    var coin = new GameObject("Coin");
    //    var coinCollider = coin.AddComponent<BoxCollider2D>();
    //    coinCollider.isTrigger = true;
    //    coin.tag = "Coin";

    //    // Manually trigger the collision
    //    player.OnTriggerEnter2D(coinCollider);

    //    // Verify
    //    Assert.AreEqual(initialCoins + 1, mockGameManager.GetCoins(),
    //        "GameManager should have received 1 coin");

    //    // Cleanup
    //    Object.DestroyImmediate(coin);
    //    yield return null;
    //}

    // 5. Mock Input System
    private class MockInput : IInput
    {
        private bool wPressed;

        public void SetKey(KeyCode key, bool pressed)
        {
            if (key == KeyCode.W) wPressed = pressed;
        }

        public bool GetKey(KeyCode key) => key == KeyCode.W ? wPressed : false;
        public float GetAxis(string axis) => 0f;
    }

    // 6. Edge case test for sprite renderers
    [Test]
    public void Player_HandlesMissingSpriteRenderers_Gracefully()
    {
        // Destroy renderers to test error handling
        Object.DestroyImmediate(player.transform.Find("helmet_0").GetComponent<SpriteRenderer>());
        Object.DestroyImmediate(player.transform.Find("torso").GetComponent<SpriteRenderer>());

        Assert.DoesNotThrow(() => player.Flip(true));
    }

    // 7. Parameterized speed test
    [TestCase(5f, 2f, 3f)]
    [TestCase(10f, 4f, 6f)]
    public void Player_MoveSpeed_AffectsVelocity(float speed, float input, float expectedVelocityX)
    {
        player.moveSpeed = speed;
        player.movement = input;
        player.FixedUpdate(); // Manually call FixedUpdate

        Assert.AreEqual(expectedVelocityX, rb.linearVelocity.x);
    }
}

// Mock interface for input testing
public interface IInput
{
    bool GetKey(KeyCode key);
    float GetAxis(string axis);
}

// Testable version of GameManager
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int coins;

    public void AddCoin(int amount) => coins += amount;
    public int GetCoins() => coins;
}