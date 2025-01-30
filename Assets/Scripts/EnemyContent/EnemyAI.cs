using System.Collections.Generic;
using PlayerContent;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private Animator _animator;
    [SerializeField] private int numberOfPatrolPoints = 5; 
    [SerializeField] private float patrolPointGenerationRadius = 10f;
    
    private int _damage = 15;
    public float viewDistance = 1f; // Расстояние обзора перед врагом
    public float viewWidth = 0.5f; // Ширина области обзора
    public float patrolSpeed = 2f; // Скорость патрулирования
    public float chaseSpeed = 3.5f;
    public Transform playerTransform;
    private PlayerHealth _playerHealth;
    public float patrolWaitTime = 5f;
    public float attackRadius = 3f; // Радиус атаки
    public float attackCooldown = 1f;

    private int currentPatrolIndex = 0;
    private NavMeshAgent agent;
    private bool isChasing = false;
    private float lastAttackTime = 0f;
    private float lastPatrolTime = 0f;
    private List<Vector3> patrolPoints = new List<Vector3>();
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        
        GeneratePatrolPoints();
        SetNextPatrolPoint();
    }

    private void Update()
    {
        // Debug.Log(Time.time - lastPatrolTime>= patrolWaitTime);
        
        if (isChasing)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(playerTransform.position);
            _animator.SetBool("Walking", true);
            
            if (!CanSeePlayer())
            {
                _animator.SetBool("Attack", false);
                isChasing = false;
                agent.speed = patrolSpeed;
                SetNextPatrolPoint();
            }

            Debug.Log("Chasing "+  (Vector2.Distance(transform.position, playerTransform.position) <= attackRadius));
            
            if (Vector2.Distance(transform.position, playerTransform.position) <= attackRadius)
            {
                AttackPlayer();
            }
        }
        else
        {
            agent.speed = patrolSpeed;

            if (!agent.pathPending && agent.remainingDistance < 0.1f)
            {
                if (lastPatrolTime == 0f)
                {
                    lastPatrolTime = Time.time;
                }
                
                if (Time.time - lastPatrolTime >= patrolWaitTime)
                {
                    SetNextPatrolPoint();
                }
                else
                {
                    _animator.SetBool("Walking", false);
                }
            }
            else
            {
                _animator.SetBool("Walking", true);
            }

            if (CanSeePlayer())
            {
                isChasing = true;
                // _animator.SetBool("Attack", true);
            }
        }

        CheckDirection();
    }

    void AttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            _animator.SetBool("Attack", true);
            Debug.Log("Атакуем игрока!");
            if (_playerHealth != null)
                _playerHealth.TakeDamage(_damage);
            lastAttackTime = Time.time;
        }
    }
    
    void GeneratePatrolPoints()
    {
        for (int i = 0; i < numberOfPatrolPoints; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * patrolPointGenerationRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, patrolPointGenerationRadius, NavMesh.AllAreas))
            {
                patrolPoints.Add(hit.position);
            }
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
                playerTransform = player.transform;
                _playerHealth = player;
                return true;
            }
        }

        _playerHealth = null;
        return false;
    }

    void SetNextPatrolPoint()
    {
        if (patrolPoints.Count == 0)
            return;

        agent.SetDestination(patrolPoints[currentPatrolIndex]);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        lastPatrolTime = 0f;
        
        
        
        /*if (_patrolPoints.Length == 0)
            return;

        agent.SetDestination(_patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % _patrolPoints.Length;
        lastPatrolTime = 0f;*/
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