using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    int life = 3;
    public CapsuleCollider2D playerCollider;

    public PlayerMovement playerMovement;

    private bool kannSchadenErhalten = true;

    IEnumerator DamageCooldown()
    {
        kannSchadenErhalten = false;
        yield return new WaitForSeconds(1.5f); // Cooldown-Zeit in Sekunden
        kannSchadenErhalten = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // Überprüfen, ob der Spieler mit dem Wasser kollidiert und Schaden erhalten kann
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Water")) && kannSchadenErhalten)
        {
            life--;
            Debug.Log("Player Life: " + life);

            StartCoroutine(DamageCooldown());


            if (life <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            }
        }
    }


        private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bush"))
        {
            life--;
            Debug.Log("Player Life: " + life);

            playerMovement.BackBounceBush(collision.transform);

            if (life <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}

