using UnityEngine;

public class PistolAmmo : AmmoBase
{
    public override void Collect()
    {
        GunScript gunScript = Object.FindFirstObjectByType<GunScript>();
        if (gunScript != null)
        {
            var shooters = gunScript.weaponHolder.GetComponentsInChildren<Shooting>(true);
            if (shooters.Length > 0)
            {
                shooters[0].AddAmmo(ammoCount);
            }
        }

        Destroy(gameObject);
    }
}
