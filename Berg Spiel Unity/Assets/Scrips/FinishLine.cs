
using UnityEngine;
using TMPro;
using System.Collections;

public class FinishLine : MonoBehaviour
{
    [Header("Finish UI")]
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private GameObject finishScreen;
    [SerializeField] private TMP_Text finalTimeText;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject timerText;

    [Header("Gondel")]
    [SerializeField] private Animator gondelAnimator;
    [SerializeField] private Transform gondelEntryPoint;

    [Header("Konfetti")]
    [SerializeField] private ParticleSystem gondelConfetti;

    [Header("Timing")]
    [SerializeField] private float finishDelay = 3f;
    [SerializeField] private float doorOpenDelay = 0.8f;
    [SerializeField] private float entryDuration = 1.9f;

    private bool finishReached = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (finishReached || !other.CompareTag("Player"))
            return;

        if (gameTimer == null ||
            finishScreen == null ||
            finalTimeText == null ||
            gondelAnimator == null ||
            gondelEntryPoint == null)
        {
            Debug.LogError(
                "FinishLine: Referenzen fehlen!"
            );
            return;
        }

        PlayerMovement player =
            other.GetComponentInParent<PlayerMovement>();

        if (player == null)
        {
            Debug.LogError(
                "PlayerMovement nicht gefunden!"
            );
            return;
        }

        finishReached = true;

        // Timer sofort stoppen
        gameTimer.StopTimer();

        // Spielersteuerung deaktivieren
        player.DisableControls();

        // Ziel-Sequenz starten
        StartCoroutine(FinishSequence(player));
    }

    private IEnumerator FinishSequence(PlayerMovement player)
    {
        // 1. Gondeltür öffnen
        gondelAnimator.Play("GondelOpen", 0, 0f);

        yield return new WaitForSeconds(doorOpenDelay);

        // 2. Spieler-Einstiegsanimation starten
        player.StartGondelEntry();

        Rigidbody2D playerRb =
            player.GetComponent<Rigidbody2D>();

        // Physik während des Einstiegs deaktivieren
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.simulated = false;
        }

        Vector3 startPosition = player.transform.position;

        // Nur horizontal bewegen,
        // damit der Spieler nicht absinkt
        Vector3 targetPosition = new Vector3(
            gondelEntryPoint.position.x,
            player.transform.position.y,
            player.transform.position.z
        );

        float elapsed = 0f;

        // 3. Spieler in die Gondel bewegen
        while (elapsed < entryDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsed / entryDuration
            );

            player.transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                t
            );

            yield return null;
        }

        player.transform.position = targetPosition;

        // Einstiegsbewegung beenden
        player.StopGondelEntry();

        // 4. KONFETTI ABSCHIESSEN!
        if (gondelConfetti != null)
        {
            // Partikel laufen auch bei pausiertem Spiel
            var main = gondelConfetti.main;
            main.simulationSpeed = 1f;
            main.useUnscaledTime = true;

            gondelConfetti.Play();

            Debug.Log("Konfetti gestartet!");
        }
        else
        {
            Debug.LogWarning(
                "Gondel Confetti wurde nicht zugewiesen!"
            );
        }

        // 5. Restliche Zeit bis zum Ziel-Screen
        float remainingTime = Mathf.Max(
            0f,
            finishDelay - doorOpenDelay - entryDuration
        );

        yield return new WaitForSeconds(remainingTime);

        // 6. Finale Zeit berechnen
        int minutes = Mathf.FloorToInt(
            gameTimer.FinalTime / 60f
        );

        int seconds = Mathf.FloorToInt(
            gameTimer.FinalTime % 60f
        );

        int centiseconds = Mathf.FloorToInt(
            (gameTimer.FinalTime * 100f) % 100f
        );

        finalTimeText.text =
            $"{minutes:00}:{seconds:00}.{centiseconds:00}";

        // 7. UI ausblenden
        if (healthBar != null)
        {
            healthBar.SetActive(false);
        }

        if (timerText != null)
        {
            timerText.SetActive(false);
        }

        // 8. Ziel-Screen anzeigen
        finishScreen.SetActive(true);

        // 9. Spiel pausieren
        Time.timeScale = 0f;
    }
}
