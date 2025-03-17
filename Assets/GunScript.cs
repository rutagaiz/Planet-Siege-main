using UnityEngine;

public class GunScript : MonoBehaviour
{
    int totalWeapons; // Number of weapons in the weapon holder
    public int currentWeaponIndex;
    public GameObject[] guns;
    public GameObject weaponHolder;
    public GameObject currentGun;

    void Start()
    {
        totalWeapons = weaponHolder.transform.childCount;
        guns = new GameObject[totalWeapons];

        for (int i = 0; i < totalWeapons; i++)
        {
            guns[i] = weaponHolder.transform.GetChild(i).gameObject;
            guns[i].SetActive(false);
        }

        guns[0].SetActive(true);
        currentGun = guns[0];
        currentWeaponIndex = 0;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f) // Scroll up to switch to the next gun
        {
            SwitchWeapon(1);
        }
        else if (scroll < 0f) // Scroll down to switch to the previous gun
        {
            SwitchWeapon(-1);
        }
    }

    void SwitchWeapon(int direction)
    {
        guns[currentWeaponIndex].SetActive(false);
        currentWeaponIndex += direction;

        if (currentWeaponIndex >= totalWeapons)
        {
            currentWeaponIndex = 0; // Wrap around to the first weapon
        }
        else if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = totalWeapons - 1; // Wrap around to the last weapon
        }

        guns[currentWeaponIndex].SetActive(true);
        currentGun = guns[currentWeaponIndex];
    }
}
