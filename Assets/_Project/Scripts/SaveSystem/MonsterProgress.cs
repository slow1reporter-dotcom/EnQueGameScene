using System;

[Serializable]
public class MonsterProgress
{
    public string characterName; // CharacterData と対応（各キャラ1体）
    public int level;
    public int exp;
    public int currentHP; // 再開時のHPを持ちたい場合に使用
}

