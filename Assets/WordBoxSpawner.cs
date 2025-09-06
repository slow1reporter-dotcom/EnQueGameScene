using UnityEngine;
using GameData;

public class WordBoxSpawner : MonoBehaviour
{
    [Header("スポーン設定")]
    public GameObject wordBoxPrefab;
    public Transform spawnParent;
    public float spawnInterval = 2f;

    [Header("スポーンポイント（複数可）")]
    public Transform[] spawnPoints;

    [Header("単語データベース")]
    public WordDatabase database;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnWordBox();
            timer = 0f;
        }
    }

    private void SpawnWordBox()
    {
        if (wordBoxPrefab == null || database == null || spawnPoints.Length == 0) return;

        // ランダムでスポーンポイントを選択
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject obj = Instantiate(wordBoxPrefab, spawnPoint.position, spawnPoint.rotation, spawnParent);

        WordBox wordBox = obj.GetComponent<WordBox>();
        if (wordBox != null)
        {
            wordBox.ResetBox();      // ★ Spawn時に必ずリセット
            wordBox.Initialize(database); // ★ 単語データを初期化
        }
    }
}
