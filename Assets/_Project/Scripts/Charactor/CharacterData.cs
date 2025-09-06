using UnityEngine;
using System.Collections.Generic;
using GameData;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Game/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("基本情報")]
    public string characterName;
    [Range(1,6)] public int starLevel = 1;
    public int level = 1;
    public Element element;

    [Header("基礎ステータス")]
    public int baseHP = 1000;
    public int baseAttack = 100;
    public int baseDefense = 50;

    [Header("才能値（個体差）")]
    [Range(0,75)] public int talentHP;
    [Range(0,75)] public int talentAttack;
    [Range(0,50)] public int talentDefense;

    [Header("必殺技関連")]
    public SpecialType specialType;
    [TextArea] public string specialDescription;
    public float specialMultiplier = 2f;
    public float buffDuration = 0f;
    public int specialFlatValue = 0;
    public float specialRange = 5f;

    [Header("攻撃関連")]
    public float attackInterval = 1.0f;
    public NormalAttackType normalAttackType = NormalAttackType.Single;
    public float attackRange = 3f;

    [Header("外見")]
    public GameObject characterPrefab;
    public Sprite characterSprite2D;
    public Sprite chibiSprite2D;
    public Sprite elementOrbSprite2D;

    [Header("装備")]
    public EquipmentData[] equippedItems;

    [Header("条件付きスキル")]
    public List<ConditionalSkill> conditionalSkills;

    [Header("育成補正")]
    [Range(0f, 2f)] public float hpGrowthPerLevel = 0.05f;
    [Range(0f, 2f)] public float attackGrowthPerLevel = 0.05f;
    [Range(0f, 2f)] public float defenseGrowthPerLevel = 0.05f;

    // -----------------------------
    // レアリティと図鑑解放フラグ
    // -----------------------------
    public Rarity rarity = Rarity.Common;   // ガチャ排出時の希少度
    [HideInInspector] public bool unlocked = false; // 図鑑に登録済みか

    // -----------------------------
    // 重複によるレベル上限拡張
    // -----------------------------
    [Header("レベル上限拡張")]
    public int extraMaxLevel = 0; // ガチャ重複ごとに +2 される
    // 既存の BaseMaxLevel に対応するエイリアス
    public int baseMaxLevel => BaseMaxLevel;

    // -----------------------------
    // 星レベルごとの最大レベル + 拡張分
    // -----------------------------
    public int MaxLevel
    {
        get
        {
            int baseMax = starLevel switch
            {
                1 => 60,
                2 => 70,
                3 => 80,
                4 => 90,
                5 => 100,
                6 => 110,
                _ => 60
            };
            return baseMax + extraMaxLevel;
        }
    }

    // GachaManager 用にエイリアス
    public int maxLevel
    {
        get => MaxLevel;
        set
        {
            extraMaxLevel = Mathf.Max(0, value - BaseMaxLevel);
        }
    }

    public int BaseMaxLevel => starLevel switch
    {
        1 => 60,
        2 => 70,
        3 => 80,
        4 => 90,
        5 => 100,
        6 => 110,
        _ => 60
    };

    // -----------------------------
    // レベルアップ処理
    // -----------------------------
    public void LevelUp()
    {
        if (level >= MaxLevel) return;

        level++;
        baseHP = Mathf.RoundToInt(baseHP * (1 + hpGrowthPerLevel));
        baseAttack = Mathf.RoundToInt(baseAttack * (1 + attackGrowthPerLevel));
        baseDefense = Mathf.RoundToInt(baseDefense * (1 + defenseGrowthPerLevel));

        Debug.Log($"{characterName} がレベル {level} に上がりました！");
        Debug.Log($"総合力: {TotalPower}, 才能値合計: {TotalTalent}");
    }

    // -----------------------------
    // 才能値込みの最終ステータス
    // -----------------------------
    public int GetFinalHP() => Mathf.RoundToInt(baseHP * (1 + talentHP / 100f));
    public int GetFinalAttack() => Mathf.RoundToInt(baseAttack * (1 + talentAttack / 100f));
    public int GetFinalDefense() => Mathf.RoundToInt(baseDefense * (1 + talentDefense / 100f));

    // -----------------------------
    // 総合力・才能値合計
    // -----------------------------
    public int TotalPower => GetFinalHP() + GetFinalAttack() + GetFinalDefense();
    public int TotalTalent => talentHP + talentAttack + talentDefense;

    // -----------------------------
    // ステータスバー用プロパティ
    // -----------------------------
    public int CurrentHP => GetFinalHP();
    public int CurrentAttack => GetFinalAttack();
    public int CurrentDefense => GetFinalDefense();

    // -----------------------------
    // 星表示用
    // -----------------------------
    public int StarLevel => starLevel;  // TrainingButton で星数参照用
}

[System.Serializable]
public class ConditionalSkill
{
    public SpecialType specialType;
    [TextArea] public string description;
    public SkillTarget target = SkillTarget.Enemy;
    public float multiplier = 1f;
    public int flatValue = 0;
    public float range = 5f;
    public float buffDuration = 0f;
    public SkillTriggerCondition triggerCondition;
    public float interval = 0f;
}

public enum Rarity
{
    Common,
    Rare,
    SuperRare,
    UltraRare,
    Legendary
}
