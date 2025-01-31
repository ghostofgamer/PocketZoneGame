using System.Collections.Generic;
using PlayerContent;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyContent
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private int _numberOfPatrolPoints = 5; 
        [SerializeField] private float _patrolPointGenerationRadius = 3f;
        [SerializeField] private float _viewDistance = 4f;
        [SerializeField] private float _viewWidth = 6f; 
        [SerializeField] private float _patrolSpeed = 1.65f; 
        [SerializeField] private float _chaseSpeed = 3f;
        
        private int _damage = 15;
        private float _patrolWaitTime = 1.5f;
        private float _attackRadius = 1.5f; 
        private float _attackCooldown = 1f;
        private PlayerHealth _playerHealth;
        private Transform _playerTransform;
        private int _currentPatrolIndex = 0;
        private NavMeshAgent _agent;
        private bool _isChasing = false;
        private float _lastAttackTime = 0f;
        private float _lastPatrolTime = 0f;
        private List<Vector3> _patrolPoints = new List<Vector3>();
    
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateUpAxis = false;
            _agent.updateRotation = false;
        
            GeneratePatrolPoints();
            SetNextPatrolPoint();
        }

        private void Update()
        {
            if (_isChasing)
            {
                _agent.speed = _chaseSpeed;
                _agent.SetDestination(_playerTransform.position);
                _animator.SetBool("Walking", true);
            
                if (!CanSeePlayer())
                {
                    _animator.SetBool("Attack", false);
                    _isChasing = false;
                    _agent.speed = _patrolSpeed;
                    SetNextPatrolPoint();
                }
            
                if (Vector2.Distance(transform.position, _playerTransform.position) <= _attackRadius)
                {
                    AttackPlayer();
                }
            }
            else
            {
                _agent.speed = _patrolSpeed;

                if (!_agent.pathPending && _agent.remainingDistance < 0.3f)
                {
                    
                    Debug.Log("Patrolling");
                    if (_lastPatrolTime == 0f)
                        _lastPatrolTime = Time.time;
                
                    if (Time.time - _lastPatrolTime >= _patrolWaitTime)
                        SetNextPatrolPoint();
                    else
                        _animator.SetBool("Walking", false);
                }
                else
                {
                    Debug.Log("Walking");
                    _animator.SetBool("Walking", true);
                }

                if (CanSeePlayer())
                    _isChasing = true;
            }

            CheckDirection();
        }

        private void AttackPlayer()
        {
            if (Time.time - _lastAttackTime >= _attackCooldown)
            {
                _animator.SetBool("Attack", true);
        
                if (_playerHealth != null)
                    _playerHealth.TakeDamage(_damage);
                
                _lastAttackTime = Time.time;
            }
        }
    
        private void GeneratePatrolPoints()
        {
            _patrolPoints.Clear();

            float angleIncrement = 360f / _numberOfPatrolPoints;

            for (int i = 0; i < _numberOfPatrolPoints; i++)
            {
                float angle = i * angleIncrement * Mathf.Deg2Rad;
                Vector3 pointOnCircle = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * _patrolPointGenerationRadius;
                Vector3 randomPoint = transform.position + pointOnCircle;
                NavMeshHit hit;

                if (NavMesh.SamplePosition(randomPoint, out hit, _patrolPointGenerationRadius, NavMesh.AllAreas))
                    _patrolPoints.Add(hit.position);
            }
            
            ShufflePatrolPoints();
        }
    
        private void ShufflePatrolPoints()
        {
            for (int i = 0; i < _patrolPoints.Count; i++)
            {
                Vector3 temp = _patrolPoints[i];
                int randomIndex = Random.Range(i, _patrolPoints.Count);
                _patrolPoints[i] = _patrolPoints[randomIndex];
                _patrolPoints[randomIndex] = temp;
            }
        }

        private bool CanSeePlayer()
        {
            Vector2 origin = (Vector2)transform.position + (Vector2)transform.right * _viewDistance * 0.5f;
            Collider2D[] hits = Physics2D.OverlapBoxAll(origin, new Vector2(_viewWidth, _viewDistance), 0);

            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject.TryGetComponent(out PlayerHealth player))
                {
                    _playerTransform = player.transform;
                    _playerHealth = player;
                    return true;
                }
            }

            _playerHealth = null;
            return false;
        }

        private void SetNextPatrolPoint()
        {
            if (_patrolPoints.Count == 0)
                return;
        
            _agent.SetDestination(_patrolPoints[_currentPatrolIndex]);
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
            _lastPatrolTime = 0f;
        }

        private void CheckDirection()
        {
            if (_agent.velocity.x > 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (_agent.velocity.x < 0)
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}