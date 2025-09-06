using UnityEngine;
using System.IO;

public class PlayerResourceManager : MonoBehaviour
{
    public PlayerResources resources = new PlayerResources(); // データ格納用
    private string savePath;

    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "player_resources.json");
        LoadResources();
    }

    // 保存
    public void SaveResources()
    {
        string json = JsonUtility.ToJson(resources, true); // trueで見やすい整形
        File.WriteAllText(savePath, json);
        Debug.Log("PlayerResources saved to " + savePath);
    }

    // 読み込み
    public void LoadResources()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            resources = JsonUtility.FromJson<PlayerResources>(json);
            Debug.Log("PlayerResources loaded from " + savePath);
        }
        else
        {
            SaveResources(); // 初回用に作成
            Debug.Log("No save file found. Created new one.");
        }
    }
}
