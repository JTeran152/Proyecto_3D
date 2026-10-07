using UnityEngine;
using TMPro;

public class OrbCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI orbCounterText;

    private void Start()
    {
        if (GameManager.Instance == null)
            return;

        // Nos suscribimos al evento del GameManager
        GameManager.Instance.OnOrbsChanged += UpdateOrbCounter;

        // Mostramos el estado inicial
        UpdateOrbCounter();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            // Nos desuscribimos del evento
            GameManager.Instance.OnOrbsChanged -= UpdateOrbCounter;
        }
    }

    private void UpdateOrbCounter()
    {
        orbCounterText.text = "ORBES: "
            + GameManager.Instance.CollectedOrbs
            + "/"
            + GameManager.Instance.TotalOrbs;
    }
}