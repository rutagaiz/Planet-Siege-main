using UnityEngine;

public class RLauncherAmmo : AmmoBase
{
    public override void Collect()
    {
        GunScript gunScript = Object.FindFirstObjectByType<GunScript>();
        if (gunScript != null)
        {
            var shooters = gunScript.weaponHolder.GetComponentsInChildren<Shooting>(true);
            if (shooters.Length > 4)
            {
                shooters[4].AddAmmo(ammoCount);
            }
        }

        Destroy(gameObject);
    }
}
