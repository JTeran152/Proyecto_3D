using UnityEngine;

public class RuneInteraction : MonoBehaviour
{
    [SerializeField] private Light runeLight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            runeLight.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            runeLight.enabled = false;
        }
    }
}
