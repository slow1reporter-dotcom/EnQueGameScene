using System;

[Serializable]
public class CharacterSaveData
{
    // -----------------------------
    // 基本情報
    // -----------------------------
    public string characterName;

    // レベル・経験値
    public int level;
    public int exp;

    // -----------------------------
    // ステータス（変動するので保存）
    // -----------------------------
    public int currentHP;
    public int currentAttack;
    public int currentDefense;

    // -----------------------------
    // 才能値（個体差）
    // -----------------------------
    public int talentHP;
    public int talentAttack;
    public int talentDefense;

    // -----------------------------
    // 装備データ（IDで保存: int 配列）
    // -----------------------------
    public int[] equippedItemIDs;

    // -----------------------------
    // 図鑑解放済みかどうか
    // -----------------------------
    public bool unlocked;

    // -----------------------------
    // コンストラクタ
    // -----------------------------
    public CharacterSaveData(
        string name,
        int level,
        int hp,
        int attack,
        int defense,
        bool unlocked = false,
        int exp = 0,
        int talentHP = 0,
        int talentAttack = 0,
        int talentDefense = 0,
        int[] equippedIDs = null
    )
    {
        characterName = name;
        this.level = level;
        currentHP = hp;
        currentAttack = attack;
        currentDefense = defense;
        this.unlocked = unlocked;
        this.exp = exp;

        this.talentHP = talentHP;
        this.talentAttack = talentAttack;
        this.talentDefense = talentDefense;

        this.equippedItemIDs = equippedIDs ?? new int[0];
    }
}
