using UnityEngine;

public class LGunAmmo1 : AmmoBase
{
    public override void Collect()
    {
        GunScript gunScript = Object.FindFirstObjectByType<GunScript>();
        if (gunScript != null)
        {
            var shooters = gunScript.weaponHolder.GetComponentsInChildren<Shooting>(true);
            if (shooters.Length > 3)
            {
                shooters[3].AddAmmo(ammoCount);
            }
        }

        Destroy(gameObject);
    }
}
