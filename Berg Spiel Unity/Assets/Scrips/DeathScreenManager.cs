
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathScreenManager : MonoBehaviour
{
    public static DeathScreenManager Instance;

    [Header("Death Screen")]
    [SerializeField] private GameObject deathScreen;

    private bool isDead = false;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;

        if (deathScreen != null)
            deathScreen.SetActive(false);
    }

    public void StartDeathSequence(GameObject player, Animator animator)
    {
        if (isDead) return;

        isDead = true;
        StartCoroutine(DeathSequence(player, animator));
    }

    private IEnumerator DeathSequence(GameObject player, Animator animator)
    {
        // Normale Bewegung stoppen
        PlayerMovement movement = player.GetComponent<PlayerMovement>();

        if (movement != null)
            movement.enabled = false;

        // Bewegung stoppen, normale Physik beibehalten
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 3f;
        }

        // Todesanimation abspielen
        if (animator != null)
        {
            animator.Play("Player_Death", 0, 0f);
            animator.Update(0f);

            // Warten, bis der Death-Animationszustand erreicht ist
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0)
                .IsName("Player_Death")
            );

            // Tatsächliche Animationsdauer abwarten
            float duration =
                animator.GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(duration);
        }


        // Spieler-Sprites ausblenden, GameObject bleibt aktiv
        SpriteRenderer[] sprites =
            player.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.enabled = false;
        }

        // Death Screen anzeigen
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
        }
        else
        {
            Debug.LogError("Death Screen ist nicht zugewiesen!");
        }

        Time.timeScale = 0f;

    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
