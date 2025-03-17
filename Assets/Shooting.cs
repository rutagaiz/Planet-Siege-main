using UnityEngine;
using TMPro; // For TextMeshPro UI

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;

    public bool canFire;
    private float timer;
    public float timeBetweenFiring;

    [SerializeField] private int maxAmmo = 10;
    private int currentAmmo;

    [SerializeField] private TextMeshProUGUI ammoText; // Assign this in the Inspector

    void Start()
    {
        mainCam = Camera.main;
        currentAmmo = maxAmmo; // Set full ammo on start
        UpdateAmmoUI(); // Update UI at the start
    }

    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        // Fire rate control
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        // Fire bullet only if ammo is available
        if (Input.GetMouseButton(0) && canFire && currentAmmo > 0)
        {
            canFire = false;
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            currentAmmo--; // Reduce ammo count
            UpdateAmmoUI(); // Update UI
        }
    }

    // Update the UI text in "ammoLeft / maxAmmo" format
    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + maxAmmo;
        }
    }
}