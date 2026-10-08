
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    [Header("Current Difficulty")]
    [SerializeField] private Difficulty currentDifficulty = Difficulty.Easy;

    public Difficulty CurrentDifficulty => currentDifficulty;

    public int TotalOrbs
    {
        get
        {
            switch (currentDifficulty)
            {
                case Difficulty.Easy:
                    return 3;

                case Difficulty.Normal:
                    return 5;

                case Difficulty.Hard:
                    return 7;

                default:
                    return 3;
            }
        }
    }

    public float PatrolSpeed
    {
        get
        {
            switch (currentDifficulty)
            {
                case Difficulty.Easy:
                    return 2f;

                case Difficulty.Normal:
                    return 3f;

                case Difficulty.Hard:
                    return 4f;

                default:
                    return 2f;
            }
        }
    }

    public float DetectionRange
    {
        get
        {
            switch (currentDifficulty)
            {
                case Difficulty.Easy:
                    return 8f;

                case Difficulty.Normal:
                    return 12f;

                case Difficulty.Hard:
                    return 16f;

                default:
                    return 8f;
            }
        }
    }

    public float LoseRange
    {
        get
        {
            switch (currentDifficulty)
            {
                case Difficulty.Easy:
                    return 12f;

                case Difficulty.Normal:
                    return 16f;

                case Difficulty.Hard:
                    return 22f;

                default:
                    return 12f;
            }
        }
    }

    public float ChaseSpeedBonus
    {
        get
        {
            switch (currentDifficulty)
            {
                case Difficulty.Easy:
                    return 1f;

                case Difficulty.Normal:
                    return 1.5f;

                case Difficulty.Hard:
                    return 2f;

                default:
                    return 1f;
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        currentDifficulty = difficulty;
        Debug.Log(
            "Dificultad seleccionada: " + currentDifficulty +
            " | Orbes: " + TotalOrbs
        );
    }

    public void SetEasy()
    {
        SetDifficulty(Difficulty.Easy);
    }

    public void SetNormal()
    {
        SetDifficulty(Difficulty.Normal);
    }

    public void SetHard()
    {
        SetDifficulty(Difficulty.Hard);
    }
}