using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;       // 生成する敵のプレハブ
    public float spawnInterval = 3f;     // 生成間隔（秒）
    public float spawnXRange = 5f;       // 横のランダム範囲
    public float spawnZ = 20f;           // 生成するZ位置

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        float x = Random.Range(-spawnXRange, spawnXRange);
        Vector3 spawnPos = new Vector3(x, 0.5f, spawnZ);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
