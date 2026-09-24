using UnityEngine;

public class PortalInteraction : MonoBehaviour
{
    [SerializeField] private Light portalLight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
}