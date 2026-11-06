
using UnityEngine;
using TMPro;
using System.Collections;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private GameObject finishScreen;
    [SerializeField] private TMP_Text finalTimeText;

    // UI-Elemente, die verschwinden sollen
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject timerText;

    [SerializeField] private float finishDelay = 3f;

    private bool finishReached = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (finishReached)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (gameTimer == null)
        {
            Debug.LogError("GameTimer wurde nicht zugewiesen!");
            return;
        }

        if (finishScreen == null || finalTimeText == null)
        {
            Debug.LogError("FinishScreen oder FinalTimeText fehlt!");
            return;
        }

        PlayerMovement playerMovement =
            other.GetComponentInParent<PlayerMovement>();

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement wurde nicht gefunden!");
            return;
        }

        finishReached = true;

        // Timer sofort stoppen
        gameTimer.StopTimer();

        // Spielersteuerung deaktivieren
        playerMovement.DisableControls();

        // 3 Sekunden warten
        StartCoroutine(FinishSequence());
    }

    private IEnumerator FinishSequence()
    {
        yield return new WaitForSeconds(finishDelay);

        // Finale Zeit berechnen
        int minutes = Mathf.FloorToInt(gameTimer.FinalTime / 60f);
        int seconds = Mathf.FloorToInt(gameTimer.FinalTime % 60f);
        int centiseconds = Mathf.FloorToInt(
            (gameTimer.FinalTime * 100f) % 100f
        );

        finalTimeText.text =
            $"{minutes:00}:{seconds:00}.{centiseconds:00}";

        // HealthBar ausblenden
        if (healthBar != null)
        {
            healthBar.SetActive(false);
        }

        // NEU: TimerText ausblenden
        if (timerText != null)
        {
            timerText.SetActive(false);
        }

        // FinishScreen anzeigen
        finishScreen.SetActive(true);

        // Spiel einfrieren
        Time.timeScale = 0f;
    }
}
