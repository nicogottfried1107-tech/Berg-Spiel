
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;

    private bool finishReached = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (finishReached)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (gameTimer == null)
        {
            Debug.LogError(
                "GameTimer wurde beim FinishLine-Objekt nicht zugewiesen!"
            );
            return;
        }

        finishReached = true;

        gameTimer.StopTimer();

        Debug.Log(
            "Level geschafft! Deine Zeit: "
            + gameTimer.FinalTime.ToString("F2")
            + " Sekunden"
        );
    }
}
