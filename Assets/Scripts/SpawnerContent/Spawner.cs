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

        private void Start()
        {
            SpawnMonsters();
        }

        private void SpawnMonsters()
        {
            for (int i = 0; i < _numberOfMonsters; i++)
            {
                Vector2 randomPosition = new Vector2(
                    Random.Range(_spawnAreaMin.x, _spawnAreaMax.x),
                    Random.Range(_spawnAreaMin.y, _spawnAreaMax.y));

                _enemy = Instantiate(_monsterPrefab, randomPosition, Quaternion.identity);
                _enemy.SetItem(_items[Random.Range(0, _items.Length)]);
            }
        }
    }
}