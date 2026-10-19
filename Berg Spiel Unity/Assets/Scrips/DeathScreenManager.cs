
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
    }

    public void StartDeathSequence(GameObject player, Animator animator)
    {
        if (isDead) return;

        isDead = true;
        StartCoroutine(DeathSequence(player, animator));
    }

    private IEnumerator DeathSequence(GameObject player, Animator animator)
    {
        // Spielerbewegung und Kollisionen stoppen
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = false;

        Collider2D[] colliders = player.GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
            col.enabled = false;

        // Death-Animation abspielen
        if (animator != null)
            animator.Play("Player_Death", 0, 0f);

        // Auf das Ende der Animation warten
        if (animator != null)
        {
            yield return null;
            yield return new WaitUntil(
                () => animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Death")
            );

            yield return new WaitForSeconds(
                animator.GetCurrentAnimatorStateInfo(0).length
            );
        }

        // Spieler verschwinden lassen
        player.SetActive(false);

        // Death Screen anzeigen
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
