namespace GameData
{
    /// <summary>
    /// 属性（Element）定義
    /// </summary>
    public enum Element
    {
        Fire,
        Water,
        Grass,
        Light,
        Dark
    }

    /// <summary>
    /// 必殺技タイプ（SpecialType）定義
    /// </summary>
    public enum SpecialType
    {
        SingleAttack,
        MultiAttack,
        Heal,
        BuffAttack,
        BuffDefense
    }

    /// <summary>
    /// 通常攻撃タイプ（NormalAttackType）定義
    /// </summary>
    public enum NormalAttackType
    {
        Single, // 単体攻撃
        Area    // 範囲攻撃
    }

    /// <summary>
    /// 条件付きスキルの発動条件
    /// </summary>
    public enum SkillTriggerCondition
    {
        OnCorrect,      // 正解したとき
        OnWrong,        // 失敗したとき
        OnEnemyDead,    // 敵が倒れたとき
        OnAllyDead,     // 味方が倒れたとき
        EveryInterval   // 定期発動
    }

    /// <summary>
    /// 条件付きスキルの対象
    /// </summary>
    public enum SkillTarget
    {
        Self,        // 自分自身
        Ally,        // 最も近い味方
        Enemy,       // 最も近い敵
        AllEnemies,  // 範囲内のすべての敵
        AllAllies    // 範囲内のすべての味方
    }
}
