using PlayerContent;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform[] _patrolPoints;
[SerializeField]private Animator _animator;

    private int _damage = 15;
    public float viewDistance = 1f; // Расстояние обзора перед врагом
    public float viewWidth = 0.5f; // Ширина области обзора
    public float patrolSpeed = 2f; // Скорость патрулирования
    public float chaseSpeed = 3.5f;
    public Transform player;
    private PlayerHealth _playerHealth;

    public float attackRadius = 3f; // Радиус атаки
    public float attackCooldown = 1f;

    private int currentPatrolIndex = 0;
    private NavMeshAgent agent;
    private bool isChasing = false;
    private float lastAttackTime = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        SetNextPatrolPoint();
    }

    private void Update()
    {
        if (isChasing)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);

            if (!CanSeePlayer())
            {
                isChasing = false;
                agent.speed = patrolSpeed;
                SetNextPatrolPoint();
            }

            if (Vector2.Distance(transform.position, player.position) <= attackRadius)
            {
                AttackPlayer();
            }
        }
        else
        {
            agent.speed = patrolSpeed;

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                SetNextPatrolPoint();

            if (CanSeePlayer())
                isChasing = true;
        }

        CheckDirection();
    }

    void AttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("Атакуем игрока!");
            if (_playerHealth != null)
                _playerHealth.TakeDamage(_damage);
            lastAttackTime = Time.time;
        }
    }

    bool CanSeePlayer()
    {
        Vector2 origin = (Vector2)transform.position + (Vector2)transform.right * viewDistance * 0.5f;
        Collider2D[] hits = Physics2D.OverlapBoxAll(origin, new Vector2(viewWidth, viewDistance), 0);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.TryGetComponent(out PlayerHealth player))
            {
                _playerHealth = player;
                return true;
            }
        }

        _playerHealth = null;
        return false;
    }

    void SetNextPatrolPoint()
    {
        if (_patrolPoints.Length == 0)
            return;

        agent.SetDestination(_patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % _patrolPoints.Length;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 origin = (Vector2)transform.position + (Vector2)transform.right * viewDistance * 0.5f;
        Gizmos.DrawWireCube(origin, new Vector3(viewWidth, viewDistance, 0));
    }

    void CheckDirection()
    {
        if (agent.velocity.x > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (agent.velocity.x < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}