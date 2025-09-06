using UnityEngine;
using System.Collections.Generic;
using GameData;

public class GachaUIManager : MonoBehaviour
{
    public GachaManager gachaManager;

    [Header("Result表示用 Container (ScrollViewなし)")]
    public Transform multiResultContent;       // ScrollViewのContent または 普通のPanelを指定
    public GameObject characterResultPrefab;   // プレハブを指定

    private List<CharacterRuntime> lastResults = new List<CharacterRuntime>();

    // 単発ガチャ
    public void OnSingleGachaButton()
    {
        CharacterRuntime runtime = gachaManager.RollGacha();
        if (runtime != null)
        {
            lastResults = new List<CharacterRuntime> { runtime };
            ShowResultUI(lastResults);
        }
    }

    // 11連ガチャ
    public void OnMultiGachaButton()
    {
        lastResults = gachaManager.RollMultiGacha(11);
        if (lastResults.Count > 0)
        {
            ShowResultUI(lastResults);
        }
    }

    // 共通UI表示
    private void ShowResultUI(List<CharacterRuntime> results)
    {
        if (results == null || results.Count == 0) return;

        // コンテナ内をクリア
        foreach (Transform child in multiResultContent)
            Destroy(child.gameObject);

        // 結果を生成して配置
        foreach (var runtime in results)
        {
            GameObject go = Instantiate(characterResultPrefab, multiResultContent);
            CharacterResult cr = go.GetComponent<CharacterResult>();
            if (cr != null)
            {
                cr.SetData(runtime);
            }
            else
            {
                Debug.LogError("CharacterResult スクリプトが Prefab にアタッチされていません！");
            }
        }
    }
}
