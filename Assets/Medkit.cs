using System;
using UnityEngine;

public class Medkit : MonoBehaviour, Item
{
    public static event Action<int> OnMedkitCollect;
    public int hpValue = 2;
 
    public void Collect()
    {
        hpValue = 0 - hpValue; // invert healing number so TakeDamage() heals instead of damaging
        OnMedkitCollect.Invoke(hpValue);
        Destroy(gameObject);
    }
}
