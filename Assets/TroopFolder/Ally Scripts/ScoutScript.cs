using UnityEngine;

public class HeavyScoutScript: MonoBehaviour
{
    [SerializeField]
    int currHealth, maxHealth, atk, speed;

     public Rigidbody2D rb ;

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

    public void TakeDamage(int damage)
    {
        currHealth -= damage;
        currHealth = Mathf.Max(0, currHealth); 

        if (currHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {     
        Destroy(gameObject);
    }
}