using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using TMPro; // UI komponentui naudoti

// Testų klasė skirta testuoti Shooting komponentą
public class ShootingTests
{
    private GameObject shootingObj;        // Objektas, prie kurio bus pridėtas Shooting script
    private Shooting shootingScript;       // Nuoroda į Shooting script komponentą

    // Prieš kiekvieną testą inicializuojami būtini komponentai
    [SetUp]
    public void Setup()
    {
        // Sukuriamas naujas objektas su Shooting script
        shootingObj = new GameObject("Shooting");
        shootingScript = shootingObj.AddComponent<Shooting>();

        // Priskiriamas „bullet“ prefab (čia naudojamas tuščias GameObject)
        shootingScript.bullet = new GameObject("Bullet");

        // Priskiriamas „bulletTransform“ (kur kulka bus sukurta)
        shootingScript.bulletTransform = shootingObj.transform;

        // Sukuriamas dirbtinis UI objektas su TextMeshProUGUI komponentu (naudojamas amunicijos rodymui)
        GameObject uiObj = new GameObject("AmmoUI");
        var ammoText = uiObj.AddComponent<TextMeshProUGUI>();

        // Naudojant reflection priskiriamas privatūs `ammoText` laukas
        shootingScript.GetType()
            .GetField("ammoText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(shootingScript, ammoText);

        // Priskiriama pagrindinė kamera (reikalinga norint apskaičiuoti pelės poziciją)
        var cameraObj = new GameObject("Main Camera");
        var cam = cameraObj.AddComponent<Camera>();
        Camera.main.tag = "MainCamera"; // būtina, kad `Camera.main` veiktų
        cam.transform.position = new Vector3(0, 0, -10); // Kamera turi būti už objekto

        // Inicializuojami kiti laukai
        shootingScript.timeBetweenFiring = 0.5f;
        shootingScript.canFire = true;

        // Rankiniu būdu paleidžiamas Start metodas (nes `Start()` automatiškai Unity testuose nesuveikia)
        shootingScript.Invoke("Start", 0);
    }

    // Po kiekvieno testo viskas sunaikinama
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(shootingObj);
        Object.DestroyImmediate(Camera.main.gameObject);
        Object.DestroyImmediate(GameObject.Find("AmmoUI"));
    }

    // Testas: patikrina ar sumažėja amunicija po šūvio
    [UnityTest]
    public IEnumerator Shooting_AmmoReducesOnFire()
    {
        // Nustatoma 5 kulkos
        shootingScript.GetType()
            .GetField("currentAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(shootingScript, 5);

        shootingScript.canFire = true;

        // Simuliuojamas pelės paspaudimas (teoriškai – veiksmas atliekamas rankiniu būdu per Update)
        shootingScript.TryFireManually(); // rankiniu būdu kviečiamas Update

        yield return null;

        // Gauta amunicijos reikšmė po šūvio
        int ammoAfter = (int)shootingScript.GetType()
            .GetField("currentAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(shootingScript);

        Assert.AreEqual(4, ammoAfter); // Tikrinama ar kulkų sumažėjo 1
    }

    // Testas: tikrina, kad šūvis nevyksta, kai nėra amunicijos
    [UnityTest]
    public IEnumerator Shooting_CannotFireWithoutAmmo()
    {
        // currentAmmo = 0, šūvis negalimas
        shootingScript.GetType()
            .GetField("currentAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(shootingScript, 0);

        shootingScript.canFire = true;

        shootingScript.TryFireManually(); // simuliuojamas bandymas šauti
        yield return null;

        // Tikrinama ar nebuvo sukurta nauja kulka (Clone reikšmė būna instancijuojant)
        Assert.IsNull(GameObject.Find("Bullet(Clone)"));
    }

    // Testas: tikrina ar UI tekstas teisingai atnaujinamas
    [Test]
    public void Shooting_UIUpdatesCorrectly()
    {
        // Gauta UpdateAmmoUI() metodas per reflection
        var method = shootingScript.GetType().GetMethod("UpdateAmmoUI", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // currentAmmo = 3
        shootingScript.GetType()
            .GetField("currentAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(shootingScript, 3);

        method.Invoke(shootingScript, null); // iškviečiamas metodas

        // Pasiimamas ammoText tekstas ir tikrinamas rezultatas
        var text = shootingScript.GetType()
            .GetField("ammoText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(shootingScript) as TextMeshProUGUI;

        Assert.AreEqual("3 / 10", text.text);
    }

    // Testas: tikrina ar metodas AddAmmo() teisingai padidina amuniciją
    [Test]
    public void Shooting_AddAmmoIncreasesCurrentAmmo()
    {
        // Gaunamas AddAmmo metodas
        var method = shootingScript.GetType().GetMethod("AddAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // currentAmmo = 2
        shootingScript.GetType()
            .GetField("currentAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(shootingScript, 2);

        method.Invoke(shootingScript, new object[] { 3 }); // Pridedama 3 kulkos

        // currentAmmo turėtų būti 5
        int currentAmmo = (int)shootingScript.GetType()
            .GetField("currentAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(shootingScript);

        Assert.AreEqual(5, currentAmmo);
    }
}
