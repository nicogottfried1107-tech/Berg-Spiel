using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 7f;
    private Rigidbody2D rb;


void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }


}
