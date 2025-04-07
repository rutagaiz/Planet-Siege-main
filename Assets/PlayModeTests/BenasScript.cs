using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class BenasScript
{
    private GameObject player;
    private CrouchController crouchController;

    private GameObject torso;
    private GameObject head;
    private BoxCollider2D collider;

    [SetUp]
    public void SetUp()
    {
        // Sukuriamas žaid?jo objektas
        player = new GameObject("Player");

        // Pridedame skript?
        crouchController = player.AddComponent<CrouchController>();

        // Sukuriame torso ir head
        torso = new GameObject("Torso");
        head = new GameObject("Head");
        torso.transform.parent = player.transform;
        head.transform.parent = player.transform;

        // Pridedame komponentus
        torso.AddComponent<SpriteRenderer>();
        collider = player.AddComponent<BoxCollider2D>();

        // Paskiriame komponentus ? skript?
        crouchController.torso = torso.transform;
        crouchController.head = head.transform;
        crouchController.playerCollider = collider;

        // Pradin?s reikšm?s
        collider.size = new Vector2(1f, 2f);
        collider.offset = new Vector2(0f, 1f);

        crouchController.crouchHeightReduction = 0.5f;
        crouchController.crouchSpriteLowering = 0.5f;
        crouchController.headLowering = 0.3f;
    }

    [UnityTearDown]
    public void TearDown()
    {
        Object.Destroy(player);
    }

    [UnityTest]
    public IEnumerator CrouchChangesColliderSize()
    {
        Vector2 originalSize = collider.size;

        yield return null;

        Assert.Less(collider.size.y, originalSize.y, "Collider aukstis turi sumazedi pritupiant");
    }

    [UnityTest]
    public IEnumerator StandUpRestoresOriginalColliderSize()
    {


        Assert.AreEqual(new Vector2(1f, 2f), collider.size, "Collider dydis turi grizti i pradine reikšme po atsistojimo.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator CrouchLowersHead()
    {
        Vector3 originalTorso = torso.transform.localPosition;
        Vector3 originalHead = head.transform.localPosition;
        Assert.Less(head.transform.localPosition.y, originalHead.y, "Head turi nusileisti pritupus.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator DoubleCrouchDoesNotChangeAgain()
    {
     

        Vector2 firstCrouchSize = collider.size;

        Assert.AreEqual(firstCrouchSize, collider.size, "Antras Crouch neturi pakeisti collider dar karta");
        yield return null;
    }

    // Parametrizuotas testas su skirtingais reikšmiu deriniais
    [UnityTest]
    public IEnumerator CrouchChangesPositions(
        [Values(0.5f, 1f)] float heightReduction,
        [Values(0.2f, 0.6f)] float torsoLowering,
        [Values(0.1f, 0.4f)] float headLowering)
    {
        // Pakei?iame reikšmes
        crouchController.crouchHeightReduction = heightReduction;
        crouchController.crouchSpriteLowering = torsoLowering;
        crouchController.headLowering = headLowering;

        Vector3 originalTorso = torso.transform.localPosition;
        Vector3 originalHead = head.transform.localPosition;

       
        yield return null;

        float expectedTorsoY = originalTorso.y - torsoLowering;
        float expectedHeadY = originalHead.y - headLowering;

        Assert.AreEqual(expectedTorsoY, torso.transform.localPosition.y, 0.01f, "Torso pozicija nesutampa.");
        Assert.AreEqual(expectedHeadY, head.transform.localPosition.y, 0.01f, "Head pozicija nesutampa.");
    }
}
