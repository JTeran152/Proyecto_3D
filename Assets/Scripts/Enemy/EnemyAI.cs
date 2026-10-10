
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float stoppingDistance = 0.5f;

    [Header("Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float loseRange = 15f;

    [Header("Detection Optimization")]
    [SerializeField, Min(0.02f)]
    private float detectionInterval = 0.1f;

    [Header("Chase Music")]
    [SerializeField] private AudioClip detectionSound;
    [SerializeField, Range(0f, 1f)]
    private float detectionVolume = 0.8f;

    [Header("Audio Transitions")]
    [SerializeField] private float chaseFadeDuration = 0.8f;

    [Header("Defeat")]
    [SerializeField] private float defeatDistance = 1.2f;
    [SerializeField] private DefeatUI defeatUI;

    private float chaseSpeedBonus = 1f;
    private int currentPoint = 0;
    private float nextDetectionTime;

    private bool isChasing = false;
    private bool ritualChaseActive = false;
    private bool defeatTriggered = false;
    private bool registeredAsChasing = false;

    // Música compartida entre todos los enemigos.
    private static AudioSource chaseMusicSource;
    private static int chasingEnemyCount;
    private static bool ritualMusicActive;

    // Estado del fundido de audio.
    private static float fadeStartTime;
    private static float fadeDuration;
    private static float fadeStartVolume;
    private static float fadeTargetVolume;
    private static bool stopSourceAfterFade;

    [RuntimeInitializeOnLoadMethod(
        RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticAudioState()
    {
        chaseMusicSource = null;
        chasingEnemyCount = 0;
        ritualMusicActive = false;

        fadeStartTime = 0f;
        fadeDuration = 0f;
        fadeStartVolume = 0f;
        fadeTargetVolume = 0f;
        stopSourceAfterFade = false;
    }

    private void Start()
    {
        ApplyDifficulty();
        nextDetectionTime = Time.time;
    }

    private void ApplyDifficulty()
    {
        if (DifficultyManager.Instance == null)
        {
            Debug.LogWarning(
                "No se encontró DifficultyManager. " +
                "Se usarán los valores del Inspector.",
                this
            );
            return;
        }

        DifficultyManager manager = DifficultyManager.Instance;

        patrolSpeed = manager.PatrolSpeed;
        detectionRange = manager.DetectionRange;
        loseRange = manager.LoseRange;
        chaseSpeedBonus = manager.ChaseSpeedBonus;
    }

    private void Update()
    {
        UpdateChaseMusicFade();

        if (defeatTriggered)
            return;

        // El movimiento y la comprobación de derrota
        // continúan ejecutándose cada fotograma.
        if (isChasing)
            ChasePlayer();
        else
            Patrol();

        CheckDefeat();

        // La detección se comprueba con un intervalo.
        if (Time.time >= nextDetectionTime)
        {
            nextDetectionTime = Time.time + detectionInterval;
            DetectPlayer();
        }
    }

    private void DetectPlayer()
    {
        if (player == null)
            return;

        if (ritualChaseActive)
        {
            isChasing = true;
            SetChasingState(true);
            return;
        }

        // Comparar distancias al cuadrado evita calcular
        // la raíz cuadrada en cada comprobación.
        float sqrDistance =
            (transform.position - player.position).sqrMagnitude;

        float sqrDetectionRange = detectionRange * detectionRange;
        float sqrLoseRange = loseRange * loseRange;

        if (!isChasing && sqrDistance <= sqrDetectionRange)
        {
            isChasing = true;
            SetChasingState(true);

            Debug.Log(
                "¡Jugador detectado! El enemigo comienza la persecución."
            );
        }
        else if (isChasing && sqrDistance >= sqrLoseRange)
        {
            isChasing = false;
            SetChasingState(false);

            Debug.Log(
                "Jugador perdido. El enemigo vuelve a patrullar."
            );
        }
    }

    private void SetChasingState(bool chasing)
    {
        if (registeredAsChasing == chasing)
            return;

        registeredAsChasing = chasing;

        if (chasing)
            chasingEnemyCount++;
        else
            chasingEnemyCount = Mathf.Max(0, chasingEnemyCount - 1);

        UpdateChaseMusic();
    }

    private void UpdateChaseMusic()
    {
        if (ritualMusicActive)
            return;

        if (chasingEnemyCount > 0)
        {
            EnsureChaseMusicSource();

            FadeChaseMusicTo(
                detectionVolume,
                chaseFadeDuration,
                false
            );
        }
        else
        {
            FadeChaseMusicTo(
                0f,
                chaseFadeDuration,
                true
            );
        }
    }

    private void EnsureChaseMusicSource()
    {
        if (chaseMusicSource != null)
            return;

        if (player == null || detectionSound == null)
            return;

        chaseMusicSource =
            player.gameObject.AddComponent<AudioSource>();

        chaseMusicSource.clip = detectionSound;
        chaseMusicSource.volume = 0f;
        chaseMusicSource.spatialBlend = 0f;
        chaseMusicSource.loop = true;
        chaseMusicSource.playOnAwake = false;
        chaseMusicSource.Stop();
    }

    private static void FadeChaseMusicTo(
        float targetVolume,
        float duration,
        bool stopAfterFade)
    {
        if (chaseMusicSource == null)
            return;

        if (targetVolume > 0f && !chaseMusicSource.isPlaying)
        {
            chaseMusicSource.volume = 0f;
            chaseMusicSource.Play();
        }

        fadeStartVolume = chaseMusicSource.volume;
        fadeTargetVolume = targetVolume;
        fadeStartTime = Time.unscaledTime;
        fadeDuration = Mathf.Max(0.01f, duration);
        stopSourceAfterFade = stopAfterFade;
    }

    private static void UpdateChaseMusicFade()
    {
        if (chaseMusicSource == null)
            return;

        if (!chaseMusicSource.isPlaying &&
            fadeTargetVolume <= 0f)
            return;

        float progress = Mathf.Clamp01(
            (Time.unscaledTime - fadeStartTime) / fadeDuration
        );

        chaseMusicSource.volume = Mathf.Lerp(
            fadeStartVolume,
            fadeTargetVolume,
            progress
        );

        if (progress >= 1f)
        {
            chaseMusicSource.volume = fadeTargetVolume;

            if (stopSourceAfterFade)
            {
                chaseMusicSource.Stop();
                chaseMusicSource.volume = 0f;
            }

            stopSourceAfterFade = false;
        }
    }

    public void StartRitualChase()
    {
        if (defeatTriggered)
            return;

        ritualChaseActive = true;
        isChasing = true;
        SetChasingState(true);
    }

    public static void StopChaseMusicForRitual()
    {
        if (ritualMusicActive)
            return;

        ritualMusicActive = true;

        FadeChaseMusicTo(0f, 1.5f, true);
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        Transform targetPoint = patrolPoints[currentPoint];

        if (targetPoint == null)
            return;

        Vector3 direction = targetPoint.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude <= stoppingDistance)
        {
            currentPoint++;

            if (currentPoint >= patrolPoints.Length)
                currentPoint = 0;

            return;
        }

        MoveTowards(direction, patrolSpeed);
    }

    private void ChasePlayer()
    {
        if (player == null)
            return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        MoveTowards(direction, patrolSpeed + chaseSpeedBonus);
    }

    private void CheckDefeat()
    {
        if (!isChasing || player == null)
            return;

        float sqrDistance =
            (transform.position - player.position).sqrMagnitude;

        if (sqrDistance <= defeatDistance * defeatDistance)
        {
            defeatTriggered = true;
            SetChasingState(false);

            Debug.Log("¡DERROTA! El enemigo alcanzó al jugador.");

            if (defeatUI != null)
                defeatUI.ShowDefeat();
            else
                Debug.LogWarning(
                    "No hay DefeatUI asignado al enemigo.",
                    this
                );
        }
    }

    private void MoveTowards(Vector3 direction, float speed)
    {
        if (direction == Vector3.zero)
            return;

        Vector3 movement =
            direction.normalized * speed * Time.deltaTime;

        transform.position += movement;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            5f * Time.deltaTime
        );
    }

    private void OnDisable()
    {
        SetChasingState(false);
    }
}
