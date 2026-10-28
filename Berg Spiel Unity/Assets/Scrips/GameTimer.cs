
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private float elapsedTime;
    private float finalTime;
    private bool isRunning;

    public float ElapsedTime => elapsedTime;
    public float FinalTime => finalTime;

    private void Start()
    {
        elapsedTime = 0f;
        finalTime = 0f;
        isRunning = true;

        UpdateTimerText();
    }

    private void Update()
    {
        if (!isRunning)
            return;

        elapsedTime += Time.deltaTime;
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        int centiseconds = Mathf.FloorToInt(
            (elapsedTime * 100f) % 100f
        );

        if (timerText != null)
        {
            timerText.text =
                $"{minutes:00}:{seconds:00}.{centiseconds:00}";
        }
    }

    public void StopTimer()
    {
        if (!isRunning)
            return;

        isRunning = false;
        finalTime = elapsedTime;

        Debug.Log(
            "Finale Spielzeit: " + finalTime.ToString("F2") + " Sekunden"
        );
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        finalTime = 0f;

        UpdateTimerText();
    }
}
