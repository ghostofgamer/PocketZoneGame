using System.Collections;
using EnemyContent;
using InventoryContent;
using UnityEngine;

namespace SpawnerContent
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private ItemPickUp[] _items;
        [SerializeField] private Enemy _monsterPrefab;
        [SerializeField] private int _numberOfMonsters = 3;
        [SerializeField] private float _spawnRadius = 5f;
        [SerializeField] private Vector2 _spawnAreaMin;
        [SerializeField] private Vector2 _spawnAreaMax;

        private Enemy _enemy;
        private float _spawnDelay = 0.3f; 
        
        private void Start()
        {
            StartCoroutine(SpawnMonstersWithDelay());
        }
        
        private IEnumerator SpawnMonstersWithDelay()
        {
            for (int i = 0; i < _numberOfMonsters; i++)
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(_spawnAreaMin.x, _spawnAreaMax.x),
                    Random.Range(_spawnAreaMin.y, _spawnAreaMax.y),
                    0);

                _enemy = Instantiate(_monsterPrefab, randomPosition, Quaternion.identity);
                _enemy.SetItem(_items[Random.Range(0, _items.Length)]);
                yield return new WaitForSeconds(_spawnDelay); 
            }
        }
        

        /*private void c()
        {
            if (patrolPoints.Count == 0)
                return;

            agent.SetDestination(patrolPoints[currentPatrolIndex]);

            int nextPatrolIndex;

            do
            {
                nextPatrolIndex = Random.Range(0, patrolPoints.Count);
            } while (nextPatrolIndex == currentPatrolIndex);

            currentPatrolIndex = nextPatrolIndex;
            // currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            lastPatrolTime = 0f;
        }*/
    }
}