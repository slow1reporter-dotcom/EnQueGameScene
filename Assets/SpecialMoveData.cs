using UnityEngine;

[CreateAssetMenu(fileName = "NewSpecialMove", menuName = "Special Move Data")]
public class SpecialMoveData : ScriptableObject
{
    [Header("基本情報")]
    public string moveName;
    public GameData.SpecialType specialType;

    [Header("効果")]
    public float range = 5f;
    public float multiplier = 1f;

    [Header("バフ/デバフ")]
    public int buffAttackPercent;
    public int buffDefensePercent;
}
