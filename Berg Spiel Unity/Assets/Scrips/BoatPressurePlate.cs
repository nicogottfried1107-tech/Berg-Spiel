
using UnityEngine;

public class BoatPressurePlate : MonoBehaviour
{
    [Header("Boote, die aktiviert werden sollen")]
    public BoatMovement[] boats;

    [Header("Spieler Tag")]
    public string playerTag = "Player";

    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nur der Spieler darf aktivieren
        if (activated)
            return;

        if (!other.CompareTag(playerTag))
            return;

        activated = true;

        // Alle zugewiesenen Boote aktivieren
        foreach (BoatMovement boat in boats)
        {
            if (boat != null)
            {
                boat.ActivateBoat();
            }
        }
    }
}
