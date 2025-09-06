using UnityEngine;

public class EnemySpawner1 : MonoBehaviour
{
    [Header("生成する敵のプレハブ")]
    public GameObject enemyPrefab;

    [Header("スポーン設定")]
    public Transform spawnCenter;       // 中心位置（空オブジェクトをアタッチ）
    public float spawnInterval = 3f;    // 生成間隔（秒）
    public float spawnXRange = 5f;      // 横方向のランダム幅
    public float spawnY = 0.5f;         // Y位置
    public float spawnZOffset = 0f;     // 中心位置からZ方向のオフセット

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnCenter == null) return;

        float x = Random.Range(-spawnXRange, spawnXRange);
        float z = spawnCenter.position.z + spawnZOffset;

        Vector3 spawnPos = new Vector3(spawnCenter.position.x + x, spawnY, z);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
