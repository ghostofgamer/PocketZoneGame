using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private ItemPickUp[] _items; 
    
    public Enemy monsterPrefab;
    public int numberOfMonsters = 3; 
    public float spawnRadius = 5f; 
    public Vector2 spawnAreaMin = new Vector2(-5, -5); 
    public Vector2 spawnAreaMax = new Vector2(5, 5);

    private Enemy _enemy;
    
    private void Start()
    {
        SpawnMonsters();
    }

    private void SpawnMonsters()
    {
        for (int i = 0; i < numberOfMonsters; i++)
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );
            _enemy =  Instantiate(monsterPrefab, randomPosition, Quaternion.identity);
            
            
            _enemy.SetItem(_items[Random.Range(0, _items.Length)]);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3((spawnAreaMin.x + spawnAreaMax.x) / 2, 0, (spawnAreaMin.y + spawnAreaMax.y) / 2),
            new Vector3(spawnAreaMax.x - spawnAreaMin.x, 0, spawnAreaMax.y - spawnAreaMin.y));
    }
}