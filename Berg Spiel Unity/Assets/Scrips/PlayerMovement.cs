using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 1f;
    public float movementForce =0.1f;
    public float bounceFactor = 10f;

    public float fallMultiplier = 3.0f;
    
    //Ground Check
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded;

    private Rigidbody2D rb;
    private Transform bootParent;
    private Transform aktuellePlattform;

    private bool isKnockedBack = false;

    //Animator
    private Animator anim;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }

        Debug.Log("Animator gefunden: " + (anim != null));
    }

    void Update()
    {

      
            if (groundCheck == null)
            {
                Debug.LogWarning("Ground check transform is not assigned!");
                return;
            }

            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );

        

            // Bewegungsrichtung einmal ermitteln
            float RichtungX = Input.GetAxis("Horizontal");

            if (!isKnockedBack)
            {
                rb.linearVelocity = new Vector2(
                    RichtungX * movementForce,
                    rb.linearVelocity.y
                );
            }



        // Springen
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            anim.SetBool("IsJumping", true);
        }

        // Sprunganimation beim Landen beenden
        if (isGrounded && rb.linearVelocity.y <= 0)
        {
            anim.SetBool("IsJumping", false);
        }





        // Schnelleres Fallen
        if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector2.up
                    * Physics2D.gravity.y
                    * (fallMultiplier - 1)
                    * Time.deltaTime;
            }

            // Spieler drehen
            if (rb.linearVelocity.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (rb.linearVelocity.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

        // Laufanimation
        if (anim != null)
        {
            bool laeuft = Mathf.Abs(RichtungX) > 0.1f;

            anim.SetBool("IsRunning", laeuft);

            
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boat"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Nur koppeln, wenn der Spieler von oben landet
                if (contact.normal.y > 0.5f)
                {
                    aktuellePlattform = collision.transform;
                    transform.SetParent(aktuellePlattform);
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (aktuellePlattform != null &&
            collision.transform == aktuellePlattform)
        {
            transform.SetParent(null);
            aktuellePlattform = null;
        }
    }
}
