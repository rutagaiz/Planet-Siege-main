using UnityEngine;

public class TankScript : MonoBehaviour
{
    [SerializeField]
    int currHealth, maxHealth, atk, speed;

     public Rigidbody2D rb;

    private void Start()
    {
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = Vector2.right * speed;
    }
}