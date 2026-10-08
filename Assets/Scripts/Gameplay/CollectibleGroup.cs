
using UnityEngine;

public class CollectibleGroup : MonoBehaviour
{
    [Header("Orbes disponibles en la escena")]
    [SerializeField] private GameObject[] orbs;

    private void Start()
    {
        ConfigureOrbs();
    }

    private void ConfigureOrbs()
    {
        int orbsToActivate = 3;

        if (DifficultyManager.Instance != null)
        {
            orbsToActivate = DifficultyManager.Instance.TotalOrbs;
        }

        if (orbs == null || orbs.Length == 0)
        {
            Debug.LogError("CollectibleGroup: no se han asignado los orbes.", this);
            return;
        }

        if (orbsToActivate > orbs.Length)
        {
            Debug.LogWarning("No hay suficientes orbes para la dificultad seleccionada.", this);
            orbsToActivate = orbs.Length;
        }

        for (int i = 0; i < orbs.Length; i++)
        {
            if (orbs[i] != null)
            {
                orbs[i].SetActive(i < orbsToActivate);
            }
        }

        Debug.Log("Orbes activados: " + orbsToActivate + "/" + orbs.Length);
    }
}