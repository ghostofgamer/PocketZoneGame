using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject monsterPrefab; // Префаб монстра
    public int numberOfMonsters = 3; // Количество монстров для спавна
    public float spawnRadius = 5f; // Радиус спавна
    public Vector2 spawnAreaMin = new Vector2(-5, -5); // Минимальные координаты области спавна
    public Vector2 spawnAreaMax = new Vector2(5, 5);
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
            Instantiate(monsterPrefab, randomPosition, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3((spawnAreaMin.x + spawnAreaMax.x) / 2, 0, (spawnAreaMin.y + spawnAreaMax.y) / 2),
            new Vector3(spawnAreaMax.x - spawnAreaMin.x, 0, spawnAreaMax.y - spawnAreaMin.y));
    }
}