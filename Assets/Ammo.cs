using System;
using UnityEngine;

public class Ammo : MonoBehaviour, Item
{
    public static event Action<int> OnAmmoCollect;
    public int AmmoCount = 20;
    public void Collect()
    {
        OnAmmoCollect.Invoke(AmmoCount);
        Destroy(gameObject);
    }
}
