
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Collectibles")]
    [SerializeField] private int totalOrbs = 3;

    [Header("Ritual Music")]
    [SerializeField] private AudioSource forestAmbienceSource;
    [SerializeField] private AudioSource ritualMusicSource;

    [Header("Ritual Message")]
    [SerializeField] private GameObject ritualMessage;

    private int collectedOrbs = 0;
    private bool ritualStarted = false;

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

        // Preparar la música del ritual.
        if (ritualMusicSource != null)
        {
            ritualMusicSource.playOnAwake = false;
            ritualMusicSource.loop = true;
            ritualMusicSource.Stop();
        }

        if (ritualMessage != null)
        {
            ritualMessage.SetActive(false);
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
            StartRitualMusic();
        }
    }
    
    private void StartEnemiesRitualChase()
    {
        EnemyAI[] enemies =
            FindObjectsByType<EnemyAI>();

        foreach (EnemyAI enemy in enemies)
        {
            enemy.StartRitualChase();
        }

        Debug.Log(
            "¡El ritual ha comenzado! Todos los enemigos persiguen al jugador."
        );
    }

    private void StartRitualMusic()
    {
        if (ritualStarted)
            return;

        ritualStarted = true;

        if (ritualMessage != null)
        {
            ritualMessage.SetActive(true);
        }

        // Detener el ambiente normal del bosque.
        if (forestAmbienceSource != null)
        {
            forestAmbienceSource.Stop();
        }

        // Iniciar la música del ritual.
        if (ritualMusicSource != null)
        {
            ritualMusicSource.Play();
        }

        // Activar la persecución de todos los enemigos.
        StartEnemiesRitualChase();

        Debug.Log("¡CORRE!");
    }

    public bool HasCollectedAllOrbs()
    {
        return collectedOrbs >= totalOrbs;
    }
}