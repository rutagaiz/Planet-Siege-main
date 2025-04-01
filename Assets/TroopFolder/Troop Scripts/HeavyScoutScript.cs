using UnityEngine;

public class ScoutScript : MonoBehaviour
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
        rb.linearVelocity = Vector2.right * speed;
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