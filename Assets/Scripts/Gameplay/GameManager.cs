
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

    public event Action OnOrbsChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Obtener la cantidad de orbes según la dificultad seleccionada.
        if (DifficultyManager.Instance != null)
        {
            totalOrbs = DifficultyManager.Instance.TotalOrbs;
        }
    }

    public void CollectOrb()
    {
        // Evitar que el contador supere el total requerido.
        if (collectedOrbs >= totalOrbs)
            return;

        collectedOrbs++;

        Debug.Log(
            "Orbes recolectados: " +
            collectedOrbs + "/" + totalOrbs
        );

        OnOrbsChanged?.Invoke();

        if (HasCollectedAllOrbs())
        {
            Debug.Log("¡Todos los orbes necesarios fueron recolectados!");
        }
    }

    public bool HasCollectedAllOrbs()
    {
        return collectedOrbs >= totalOrbs;
    }
}