using UnityEngine;

public class ScoutScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = Vector2.right * speed;
    }
}
 