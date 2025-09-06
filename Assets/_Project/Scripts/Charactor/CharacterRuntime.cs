using System.Linq;
using UnityEngine;

public class CharacterRuntime
{
    // 参照するテンプレート
    public CharacterData template;

    // 変動ステータス
    public int level;
    public int exp;
    public int currentHP;
    public int currentAttack;
    public int currentDefense;

    // 才能値
    public int talentHP;
    public int talentAttack;
    public int talentDefense;

    // 装備IDで管理
    public int[] equippedItemIDs;

    // 図鑑解放フラグ
    public bool unlocked;

    // -----------------------------
    // Runtimeベースで管理する追加レベル
    // -----------------------------
    public int extraMaxLevel = 0;

    // Runtime上の最大レベル（TemplateのBaseMaxLevel + 追加分）
    public int CurrentMaxLevel => Mathf.Min(template.BaseMaxLevel + extraMaxLevel, 50);

    // -----------------------------
    // 追加プロパティ（TrainingButton用）
    // -----------------------------
    public int TotalPower => currentHP + currentAttack + currentDefense;
    public int TotalTalent => talentHP + talentAttack + talentDefense;
    public int StarLevel => Mathf.Clamp(level / 10, 1, 5); // 例: レベル10ごとに星1つ、最大5つ

    // -----------------------------
    // コンストラクタ（新規作成 or ロード時）
    // -----------------------------
    public CharacterRuntime(CharacterData template, CharacterSaveData saveData = null)
    {
        this.template = template;

        if (saveData != null)
        {
            ApplySaveData(saveData);
        }
        else
        {
            level = template.level;
            currentHP = template.GetFinalHP();
            currentAttack = template.GetFinalAttack();
            currentDefense = template.GetFinalDefense();
            talentHP = template.talentHP;
            talentAttack = template.talentAttack;
            talentDefense = template.talentDefense;
            equippedItemIDs = template.equippedItems?.Select(e => e.itemID).ToArray() ?? new int[0];
            unlocked = template.unlocked;
            exp = 0;
            extraMaxLevel = 0;
        }
    }

    // -----------------------------
    // レベルアップ
    // -----------------------------
    public void LevelUp()
    {
        if (level >= CurrentMaxLevel) return;

        level++;
        currentHP = Mathf.RoundToInt(currentHP * (1 + template.hpGrowthPerLevel));
        currentAttack = Mathf.RoundToInt(currentAttack * (1 + template.attackGrowthPerLevel));
        currentDefense = Mathf.RoundToInt(currentDefense * (1 + template.defenseGrowthPerLevel));
    }

    // -----------------------------
    // 重複ガチャによる追加レベル
    // -----------------------------
    public void AddExtraMaxLevel(int amount)
    {
        extraMaxLevel = Mathf.Min(extraMaxLevel + amount, 50 - template.BaseMaxLevel);
    }

    // -----------------------------
    // セーブデータに変換
    // -----------------------------
    public CharacterSaveData ToSaveData()
    {
        return new CharacterSaveData(
            template.characterName,
            level,
            currentHP,
            currentAttack,
            currentDefense,
            unlocked,
            exp,
            talentHP,
            talentAttack,
            talentDefense,
            equippedItemIDs
        );
    }

    // -----------------------------
    // セーブデータを適用
    // -----------------------------
    public void ApplySaveData(CharacterSaveData saveData)
    {
        level = saveData.level;
        currentHP = saveData.currentHP;
        currentAttack = saveData.currentAttack;
        currentDefense = saveData.currentDefense;
        talentHP = saveData.talentHP;
        talentAttack = saveData.talentAttack;
        talentDefense = saveData.talentDefense;
        equippedItemIDs = saveData.equippedItemIDs ?? new int[0];
        unlocked = saveData.unlocked;
        exp = saveData.exp;
        extraMaxLevel = 0; // 初期化
    }

    // -----------------------------
    // 装備参照
    // -----------------------------
    public EquipmentData[] GetEquippedItems()
    {
        return equippedItemIDs.Select(id => EquipmentDatabase.Instance.GetEquipmentByID(id))
                               .Where(e => e != null)
                               .ToArray();
    }
}
