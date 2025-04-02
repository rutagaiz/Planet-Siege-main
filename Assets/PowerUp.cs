using UnityEngine;
[System.Serializable]
public class PowerUp
{
    public GameObject itemPrefab;
    [Range(0, 100)] public float dropChance;
}
