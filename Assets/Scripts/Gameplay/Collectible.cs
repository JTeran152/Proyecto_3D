
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Sonido de recogida")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField, Range(0f, 1f)] private float pickupVolume = 1f;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            // Reproducir el sonido antes de destruir el orbe.
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(
                    pickupSound,
                    transform.position,
                    pickupVolume
                );
            }

            GameManager.Instance.CollectOrb();

            Destroy(gameObject);
        }
    }
}