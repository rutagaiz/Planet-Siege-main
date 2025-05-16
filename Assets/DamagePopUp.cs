using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public Transform target; // The enemy to follow
    public Vector3 offset; // Offset from the enemy
    
    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
