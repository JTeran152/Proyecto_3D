using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Collectibles")]
    [SerializeField] private int totalOrbs = 3;

    private int collectedOrbs = 0;

    public int CollectedOrbs => collectedOrbs;
    public int TotalOrbs => totalOrbs;

    // Evento que avisa cuando cambia la cantidad de orbes
    public event Action OnOrbsChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectOrb()
    {
        collectedOrbs++;

        Debug.Log("Orbes recolectados: " + collectedOrbs + "/" + totalOrbs);

        // Avisamos a la UI que el contador cambió
        OnOrbsChanged?.Invoke();

        if (collectedOrbs >= totalOrbs)
        {
            Debug.Log("¡Todos los orbes fueron recolectados!");
        }
    }

    public bool HasCollectedAllOrbs()
    {
        return collectedOrbs >= totalOrbs;
    }
}