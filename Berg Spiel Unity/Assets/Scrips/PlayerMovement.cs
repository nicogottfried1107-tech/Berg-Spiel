using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 12f;
    public float movementForce =9f;
    public float bounceFactor = 10f;

    public float fallMultiplier = 3.0f;
    
    //Ground Check
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded;

    private Rigidbody2D rb;

    private bool isKnockedBack = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (groundCheck == null)
        {
            Debug.LogWarning("Ground check transform is not assigned!");
            return;
        }

        // Ground Check
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (!isKnockedBack)
        {
            float RichtungX = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(RichtungX * movementForce, rb.linearVelocity.y);
        }

        // Springen
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // schnelleres Fallen
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up
                * Physics2D.gravity.y
                * (fallMultiplier - 1)
                * Time.deltaTime;
        }

        // Flip the player sprite based on movement direction
        if (rb.linearVelocity.x < 0)
        {

            transform.localScale = new Vector3(-1, 1, 1);


        }
        else if (rb.linearVelocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void BackBounceBush(Transform bush)
    {
        isKnockedBack = true;

        Vector2 direction = (transform.position - bush.position).normalized;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * bounceFactor, ForceMode2D.Impulse);

        Invoke(nameof(EndKnockback), 0.2f);
    }

    private void EndKnockback()
    {
        isKnockedBack = false;
    }
}
