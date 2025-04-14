using System;
using UnityEngine;

public class Ammo : MonoBehaviour, Item
{
    public int AmmoCount = 20;

    private void Start()
    {
        // Pasirink atsitiktinį ginklą
        GunScript gunManager = FindObjectOfType<GunScript>();
        if (gunManager != null && gunManager.guns.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, gunManager.guns.Length);
            targetGun = gunManager.guns[randomIndex].GetComponent<Shooting>();
        }
    }

    private Shooting targetGun;

    public void Collect()
    {
        if (targetGun != null)
        {
            targetGun.AddAmmo(AmmoCount);
        }
        Destroy(gameObject);
    }
}
