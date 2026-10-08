using UnityEngine;

public class PortalInteraction : MonoBehaviour
{
    [Header("Portal Light")]
    [SerializeField] private Light portalLight;

    [Header("Portal Colors")]
    [SerializeField] private Color lockedColor = Color.red;
    [SerializeField] private Color unlockedColor = Color.blue;

    private void Start()
    {
        UpdatePortalColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UpdatePortalColor();
            portalLight.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            portalLight.enabled = false;
        }
    }

    private void UpdatePortalColor()
    {
        if (portalLight == null || GameManager.Instance == null)
            return;

        if (GameManager.Instance.HasCollectedAllOrbs())
        {
            portalLight.color = unlockedColor;
        }
        else
        {
            portalLight.color = lockedColor;
        }
    }
}