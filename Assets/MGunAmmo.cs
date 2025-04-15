using UnityEngine;

public class MGunAmmo : AmmoBase
{
    public override void Collect()
    {
        GunScript gunScript = Object.FindFirstObjectByType<GunScript>();
        if (gunScript != null)
        {
            var shooters = gunScript.weaponHolder.GetComponentsInChildren<Shooting>(true);
            if (shooters.Length > 2)
            {
                shooters[2].AddAmmo(ammoCount);
            }
        }

        Destroy(gameObject);
    }
}
