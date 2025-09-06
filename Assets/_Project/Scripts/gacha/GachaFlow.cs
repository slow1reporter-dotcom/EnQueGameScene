using UnityEngine;
using System.Collections.Generic;
using GameData;

public class GachaFlow : MonoBehaviour
{
    public PageSwitcher switcher;
    public GachaUIManager gachaUIManager;

    private List<CharacterRuntime> lastResults;

    // 単発ガチャ実行
    public void PlaySingle()
    {
        // ① ガチャ実行 → 結果データ生成
        CharacterRuntime runtime = gachaUIManager.gachaManager.RollGacha();
        if (runtime != null)
        {
            lastResults = new List<CharacterRuntime> { runtime };
            gachaUIManager.SendMessage("ShowResultUI", lastResults); // UIの準備のみ
        }

        // ② 演出画面へ
        switcher.Show(3);

        // ③ 演出後に結果画面へ
        Invoke(nameof(ShowResult), 3f);
    }

    // 11連ガチャ実行
    public void PlayMulti()
    {
        // ① ガチャ実行 → 結果データ生成
        lastResults = gachaUIManager.gachaManager.RollMultiGacha(11);
        if (lastResults.Count > 0)
        {
            gachaUIManager.SendMessage("ShowResultUI", lastResults); // UIの準備のみ
        }

        // ② 演出画面へ
        switcher.Show(3);

        // ③ 演出後に結果画面へ
        Invoke(nameof(ShowResult), 3f);
    }

    // 演出後に呼ばれる
    void ShowResult()
    {
        switcher.Show(4); // 結果画面へ
    }
}
