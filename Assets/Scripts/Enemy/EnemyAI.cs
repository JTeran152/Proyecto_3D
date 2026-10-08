
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

    [Header("Defeat")]
    [SerializeField] private float defeatDistance = 1.2f;
    [SerializeField] private DefeatUI defeatUI;

    private float chaseSpeedBonus = 1f;

    private int currentPoint = 0;
    private bool isChasing = false;
    private bool defeatTriggered = false;

    private void Start()
    {
        ApplyDifficulty();
    }

    private void ApplyDifficulty()
    {
        if (DifficultyManager.Instance == null)
        {
            Debug.LogWarning(
                "No se encontró DifficultyManager. Se usarán los valores del Inspector.",
                this
            );
            return;
        }

        DifficultyManager manager = DifficultyManager.Instance;

        patrolSpeed = manager.PatrolSpeed;
        detectionRange = manager.DetectionRange;
        loseRange = manager.LoseRange;
        chaseSpeedBonus = manager.ChaseSpeedBonus;

        Debug.Log(
            gameObject.name + " configurado: " +
            "Velocidad " + patrolSpeed +
            ", detección " + detectionRange +
            ", pérdida " + loseRange
        );
    }

    private void Update()
    {
        if (defeatTriggered)
            return;

        DetectPlayer();

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        CheckDefeat();
    }

    private void DetectPlayer()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            player.position
        );

        if (!isChasing && distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            Debug.Log("¡Jugador detectado! El enemigo comienza la persecución.");
        }

        if (isChasing && distanceToPlayer >= loseRange)
        {
            isChasing = false;
            Debug.Log("Jugador perdido. El enemigo vuelve a patrullar.");
        }
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        Transform targetPoint = patrolPoints[currentPoint];

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

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distanceToPlayer <= defeatDistance)
        {
            defeatTriggered = true;

            Debug.Log("¡DERROTA! El enemigo alcanzó al jugador.");

            if (defeatUI != null)
            {
                defeatUI.ShowDefeat();
            }
            else
            {
                Debug.LogWarning(
                    "No hay DefeatUI asignado al enemigo.",
                    this
                );
            }
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
}
