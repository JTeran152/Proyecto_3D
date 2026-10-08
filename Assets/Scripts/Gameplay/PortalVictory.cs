using UnityEngine;

public class PortalVictory : MonoBehaviour
{
    [SerializeField] private PortalMessageUI portalMessageUI;
    [SerializeField] private VictoryUI victoryUI;

    private bool victoryTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (victoryTriggered)
            return;

        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.HasCollectedAllOrbs())
        {
            victoryTriggered = true;

            portalMessageUI.ClearMessage();

            Debug.Log("¡VICTORIA! Has recolectado todos los orbes y activaste el portal.");

            victoryUI.ShowVictory();
        }
        else
        {
            portalMessageUI.ShowMessage(
                "El portal está bloqueado.\nNecesitas encontrar los 3 orbes."
            );

            Debug.Log("El portal está bloqueado. Necesitas recolectar los 3 orbes.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (!victoryTriggered)
        {
            portalMessageUI.ClearMessage();
        }
    }
}