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

        if (rb.linearVelocity.magnitude <= speed)
        {
            rb.AddForce(Vector2.right, ForceMode2D.Impulse);
        }
    }
}