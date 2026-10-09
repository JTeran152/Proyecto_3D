
using UnityEngine;

public class PortalInteraction : MonoBehaviour
{
    [Header("Portal Light")]
    [SerializeField] private Light portalLight;

    [Header("Portal Colors")]
    [SerializeField] private Color lockedColor = Color.red;
    [SerializeField] private Color unlockedColor = Color.blue;

    [Header("Portal Proximity Sound")]
    [SerializeField] private AudioSource proximityAudioSource;

    private void Start()
    {
        UpdatePortalColor();

        if (proximityAudioSource != null)
        {
            proximityAudioSource.playOnAwake = false;
            proximityAudioSource.loop = true;
            proximityAudioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        UpdatePortalColor();

        if (portalLight != null)
        {
            portalLight.enabled = true;
        }

        if (proximityAudioSource != null &&
            !proximityAudioSource.isPlaying)
        {
            proximityAudioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (portalLight != null)
        {
            portalLight.enabled = false;
        }

        if (proximityAudioSource != null)
        {
            proximityAudioSource.Stop();
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