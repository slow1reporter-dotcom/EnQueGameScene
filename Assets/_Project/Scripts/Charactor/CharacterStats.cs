using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameData;

public class CharacterStats : MonoBehaviour
{
    [Header("参照するキャラクターデータ")]
    public CharacterData data;

    // ※CharacterStats側の装備スロットは削除
    // public EquipmentData[] equippedItems;

    [Header("戦闘中の現在値")]
    public int CurrentHP { get; private set; }
    public int CurrentAttack { get; private set; }
    public int CurrentDefense { get; private set; }
    public int CurrentSpecialGauge { get; private set; }
    public float CurrentAttackSpeed { get; private set; }
    public float CurrentMoveSpeed { get; private set; }

    // 属性耐性（装備補正込み）
    public bool FireResist { get; private set; }
    public bool WaterResist { get; private set; }
    public bool GrassResist { get; private set; }
    public bool LightResist { get; private set; }
    public bool DarkResist { get; private set; }

    void Start()
    {
        InitializeStats();
        ApplyEquipment();       // 装備補正を反映
        StartPeriodicSkills();
    }

    public void InitializeStats()
    {
        CurrentHP = data.GetFinalHP();
        CurrentAttack = data.GetFinalAttack();
        CurrentDefense = data.GetFinalDefense();
        CurrentSpecialGauge = 0;
        CurrentAttackSpeed = 1f;
        CurrentMoveSpeed = 5f;

        FireResist = false;
        WaterResist = false;
        GrassResist = false;
        LightResist = false;
        DarkResist = false;
    }

    // -----------------------------
    // 装備補正を反映
    // -----------------------------
    public void ApplyEquipment()
    {
        if (data.equippedItems == null) return;

        foreach (var eq in data.equippedItems)
        {
            if (eq == null) continue;

            CurrentHP += eq.hpBonus;
            CurrentAttack += eq.attackBonus;
            CurrentDefense += eq.defenseBonus;
            CurrentAttackSpeed += eq.attackSpeedModifier;
            CurrentMoveSpeed += eq.moveSpeedModifier;

            FireResist |= eq.fireResist;
            WaterResist |= eq.waterResist;
            GrassResist |= eq.grassResist;
            LightResist |= eq.lightResist;
            DarkResist |= eq.darkResist;
        }
    }
    // -----------------------------
    // HP・ステータス操作（属性耐性考慮）
    // -----------------------------
    public void TakeDamage(int damage, Element element = Element.Fire)
    {
        bool isResist = element switch
        {
            Element.Fire => FireResist,
            Element.Water => WaterResist,
            Element.Grass => GrassResist,
            Element.Light => LightResist,
            Element.Dark => DarkResist,
            _ => false
        };

        if (isResist)
        {
            damage /= 2; // 耐性ありなら半減
        }

        CurrentHP -= Mathf.Max(damage, 1);
        if (CurrentHP <= 0) Die();
    }

    private void Die()
    {
        Debug.Log($"{data.characterName} は倒れた！");
        Destroy(gameObject);
    }

    public void Heal(int amount) => CurrentHP += amount;
    public void AddAttack(int value) => CurrentAttack += value;
    public void AddDefense(int value) => CurrentDefense += value;

    // -----------------------------
    // ChargeSpecial & 属性判定
    // -----------------------------
    public void ChargeSpecial(int amount)
    {
        CurrentSpecialGauge = Mathf.Min(CurrentSpecialGauge + amount, 3);
        if (CurrentSpecialGauge >= 3)
        {
            ActivateSpecial();
            CurrentSpecialGauge = 0;
        }
    }

    public bool IsSameAttribute(CharacterStats other)
    {
        if (other == null) return false;
        return this.data.element == other.data.element;
    }

    public bool IsSameAttribute(GameData.Element elem)
    {
        return this.data.element == elem;
    }

    // -----------------------------
    // 条件付きスキル自動処理
    // -----------------------------
    private void StartPeriodicSkills()
    {
        foreach (var skill in data.conditionalSkills)
        {
            if (skill.triggerCondition == SkillTriggerCondition.EveryInterval && skill.interval > 0f)
            {
                StartCoroutine(PeriodicSkill(skill));
            }
        }
    }

    private IEnumerator PeriodicSkill(ConditionalSkill skill)
    {
        while (true)
        {
            yield return new WaitForSeconds(skill.interval);
            ActivateConditionalSkill(skill);
        }
    }

    public void OnCorrect() => TriggerConditionalSkills(SkillTriggerCondition.OnCorrect);
    public void OnWrong() => TriggerConditionalSkills(SkillTriggerCondition.OnWrong);
    public void OnEnemyDead() => TriggerConditionalSkills(SkillTriggerCondition.OnEnemyDead);
    public void OnAllyDead() => TriggerConditionalSkills(SkillTriggerCondition.OnAllyDead);

    private void TriggerConditionalSkills(SkillTriggerCondition condition)
    {
        foreach (var skill in data.conditionalSkills)
        {
            if (skill.triggerCondition == condition)
            {
                ActivateConditionalSkill(skill);
            }
        }
    }

    private void ActivateConditionalSkill(ConditionalSkill skill)
    {
        Debug.Log($"{data.characterName} が {skill.specialType} を発動！");

        switch (skill.target)
        {
            case SkillTarget.Self:
                ApplySkill(skill, this);
                break;
            case SkillTarget.Ally:
                ApplySkill(skill, FindNearestAlly());
                break;
            case SkillTarget.Enemy:
                ApplySkill(skill, FindNearestEnemy());
                break;
            case SkillTarget.AllAllies:
                foreach (var ally in FindAllAllies(skill.range))
                    ApplySkill(skill, ally);
                break;
            case SkillTarget.AllEnemies:
                foreach (var enemy in FindAllEnemies(skill.range))
                    ApplySkill(skill, enemy);
                break;
        }
    }

    private void ApplySkill(ConditionalSkill skill, CharacterStats target)
    {
        if (target == null) return;

        switch (skill.specialType)
        {
            case SpecialType.SingleAttack:
            case SpecialType.MultiAttack:
                int damage = Mathf.RoundToInt(CurrentAttack * skill.multiplier + skill.flatValue);
                target.TakeDamage(damage);
                break;
            case SpecialType.Heal:
                int healAmount = Mathf.RoundToInt(CurrentAttack * skill.multiplier + skill.flatValue);
                target.Heal(healAmount);
                break;
            case SpecialType.BuffAttack:
                target.AddAttack(Mathf.RoundToInt(skill.buffDuration));
                break;
            case SpecialType.BuffDefense:
                target.AddDefense(Mathf.RoundToInt(skill.buffDuration));
                break;
        }
    }

    // -----------------------------
    // 通常攻撃処理
    // -----------------------------
    public void NormalAttack(CharacterStats target)
    {
        if (target == null) return;
        int damage = Mathf.Max(CurrentAttack - target.CurrentDefense, 1);
        target.TakeDamage(damage);
        Debug.Log($"{data.characterName} の通常攻撃！ {target.data.characterName} に {damage} ダメージ");
    }

    private void ActivateSpecial()
    {
        Debug.Log($"{data.characterName} の必殺技発動！（{data.specialType}）");
        // 既存の必殺技処理をここに残す
    }

    // -----------------------------
    // ターゲット検索
    // -----------------------------
    private CharacterStats FindNearestEnemy() { /* 既存処理 */ return null; }
    private CharacterStats FindNearestAlly() { /* 既存処理 */ return null; }
    private List<CharacterStats> FindAllEnemies(float range) { return new List<CharacterStats>(); }
    private List<CharacterStats> FindAllAllies(float range) { return new List<CharacterStats>(); }
    // 以下、TakeDamage～ActivateSpecial、ターゲット検索などは変更なし
    // （元のコードのまま利用）
}

    

