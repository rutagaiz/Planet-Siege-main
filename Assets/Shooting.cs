using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;
    public static System.Action<GameObject, Vector3, Quaternion> OnBulletInstantiated;
    public bool canFire;
    public float timer;
    public float timeBetweenFiring;

    [SerializeField] private int maxAmmo = 10;
    private int currentAmmo;

    [SerializeField] private TextMeshProUGUI ammoText;

    void Start()
    {
        mainCam = Camera.main;
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        
    }

    public void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        // Šaudymo sąlyga – atskirta į testuojamą metodą
        if (Input.GetMouseButton(0))
        {
            TryFireManually();
        }

        if (mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // 🔥 Šis metodas bus naudojamas testuose vietoj `Input.GetMouseButton(0)`
    public void TryFireManually()
    {
        if (canFire && currentAmmo > 0)
        {
            //SoundManager.Instance.PlayGunSound();
            canFire = false;
            GameObject spawnedBullet = Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            OnBulletInstantiated?.Invoke(spawnedBullet, bulletTransform.position, Quaternion.identity);
            currentAmmo--;
            UpdateAmmoUI();
        }
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + maxAmmo;
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        UpdateAmmoUI();
    }
}
