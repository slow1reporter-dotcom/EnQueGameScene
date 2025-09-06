using UnityEngine;
using System.Collections.Generic;
using GameData;

public class GachaManager : MonoBehaviour
{
    [Header("参照データベース")]
    public CharacterDatabase characterDatabase;

    [Header("排出確率 (%)")]
    [Range(0, 100)] public float commonRate = 60;
    [Range(0, 100)] public float rareRate = 25;
    [Range(0, 100)] public float superRareRate = 10;
    [Range(0, 100)] public float ultraRareRate = 4;
    [Range(0, 100)] public float legendaryRate = 1;

    // -----------------------------
    // 単体ガチャ実行（Runtimeベース）
    // -----------------------------
    public CharacterRuntime RollGacha()
    {
        float roll = Random.Range(0f, 100f);
        Rarity chosenRarity;

        if (roll < legendaryRate) chosenRarity = Rarity.Legendary;
        else if (roll < legendaryRate + ultraRareRate) chosenRarity = Rarity.UltraRare;
        else if (roll < legendaryRate + ultraRareRate + superRareRate) chosenRarity = Rarity.SuperRare;
        else if (roll < legendaryRate + ultraRareRate + superRareRate + rareRate) chosenRarity = Rarity.Rare;
        else chosenRarity = Rarity.Common;

        // 候補キャラを取得
        List<CharacterData> candidates = characterDatabase.allCharacters
            .FindAll(c => c.rarity == chosenRarity);

        if (candidates.Count == 0)
        {
            Debug.LogWarning("⚠️ 指定レアリティのキャラが存在しません。");
            return null;
        }

        CharacterData selectedTemplate = candidates[Random.Range(0, candidates.Count)];

        // Runtimeを取得 or 新規作成
        CharacterRuntime runtime = SaveManager.Instance.GetOrCreateRuntimeCharacter(selectedTemplate);

        // -----------------------------
        // 重複処理（Runtimeベースで extraMaxLevel 管理）
        // -----------------------------
        if (runtime.unlocked)
        {
            runtime.extraMaxLevel = Mathf.Min(runtime.extraMaxLevel + 2, 50 - selectedTemplate.BaseMaxLevel);
            Debug.Log($"🔹 {runtime.template.characterName} が重複！CurrentMaxLevel → {runtime.CurrentMaxLevel}");
        }
        else
        {
            runtime.unlocked = true; // 初回解放
            Debug.Log($"✨ {runtime.template.characterName} を初解放！");
        }

        // SaveManagerに反映
        SaveManager.Instance.UpdateCharacterRuntime(runtime);

        Debug.Log($"🎉 ガチャ結果: {runtime.template.characterName} (レア度: {runtime.template.rarity}, Lv:{runtime.level}, CurrentMaxLevel:{runtime.CurrentMaxLevel})");

        return runtime;
    }

    // -----------------------------
    // 複数回ガチャ（例: 11連）
    // -----------------------------
    public List<CharacterRuntime> RollMultiGacha(int count)
    {
        List<CharacterRuntime> results = new List<CharacterRuntime>();
        for (int i = 0; i < count; i++)
        {
            CharacterRuntime runtime = RollGacha();
            if (runtime != null)
                results.Add(runtime);
        }
        return results;
    }
}
