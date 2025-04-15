using UnityEngine;

public abstract class AmmoBase : MonoBehaviour, Item
{
    public int ammoCount = 20;

    public abstract void Collect();
}