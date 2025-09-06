using UnityEngine;

public enum EquipmentRarity
{
    N,
    R,
    SR,
    SSR
}

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Game/Equipment")]
public class EquipmentData : ScriptableObject
{
    [Header("識別用ID (一意に設定してください)")]
    public int itemID;   // ← ★ 追加：装備データを一意に識別するID

    [Header("基本情報")]
    public string equipmentName;
    public Sprite icon;
    public EquipmentRarity rarity;   // ★ レアリティを追加

    [Header("ステータス補正値")]
    public int hpBonus;
    public int attackBonus;
    public int defenseBonus;
    [Range(-1f, 1f)] public float attackSpeedModifier;
    [Range(-1f, 1f)] public float moveSpeedModifier;

    [Header("属性耐性")]
    public bool fireResist;
    public bool waterResist;
    public bool grassResist;
    public bool lightResist;
    public bool darkResist;
}
