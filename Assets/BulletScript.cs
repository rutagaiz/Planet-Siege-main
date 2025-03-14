using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    float bullet_Damage;

    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;

    // Akvile
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
               
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 direction = (mousePos - transform.position).normalized;
        
        
        rb.linearVelocity = direction * force;

        //So that the bullet rotates pagal kursoriaus kampa
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy_Stats>(out Enemy_Stats enemy))
        {
            enemy.TakeDamage(bullet_Damage);
        }
        Destroy(gameObject);
    }
}
