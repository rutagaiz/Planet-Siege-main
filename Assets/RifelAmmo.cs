using UnityEngine;

public class RifleAmmo : AmmoBase
{
    public override void Collect()
    {
        Debug.Log("RifleAmmo pickup: bandom rasti Shooting (aktyvų ar ne)");

        GunScript gunScript = Object.FindFirstObjectByType<GunScript>();
        if (gunScript != null)
        {
            // IŠKOJIMAS NET IŠ NEAKTYVIŲ
            Shooting[] allShooters = gunScript.weaponHolder.GetComponentsInChildren<Shooting>(true); // true – įskaitant neaktyvius

            if (allShooters.Length > 1)
            {
                Shooting rifle = allShooters[1]; // rifles antras, t.y. indeksas 1
                rifle.AddAmmo(ammoCount);
                Debug.Log($"RifleAmmo pickup: pridėta {ammoCount} ammo!");
            }
            else
            {
                Debug.LogWarning("RifleAmmo pickup: Nerasta pakankamai Shooting komponentų!");
            }
        }
        else
        {
            Debug.LogWarning("RifleAmmo pickup: GunScript nerastas!");
        }

        Destroy(gameObject);
    }
}
