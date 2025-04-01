using UnityEngine;

public class AllyScript : MonoBehaviour
{
    [SerializeField]
    int currHealth, maxHealth, atk, speed;
    private GUnitGunScript GUnitGunScript;
    private bool HasTarget;
    public Rigidbody2D rb;

    private void Awake()
    {
        rb.freezeRotation = true;
        GUnitGunScript = GetComponent<GUnitGunScript>();
        HasTarget = GUnitGunScript.HasTarget;
    }

    public void FixedUpdate()
    {
        HasTarget = GUnitGunScript.HasTarget;
        if (rb.linearVelocity.magnitude <= speed && HasTarget == false)
        {
            rb.AddForce(Vector2.right, ForceMode2D.Impulse);
        }
        else rb.linearVelocity = Vector2.zero;
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
