using UnityEngine;
using GameData;  // GameData.Element を参照

[CreateAssetMenu(fileName = "NewWordEffect", menuName = "Game/WordEffect")]
public class WordEffect : ScriptableObject
{
    [Header("属性")]
    public Element element;  // GameData.Element 型を使用

    [Header("正の効果")]
    public int attackChange = 0;
    public int defenseChange = 0;
    public int healAmount = 0;
    public int specialCharge = 0;

    [Header("同属性ボーナス")]
    public float sameElementMultiplier = 1.5f;

    [Header("負の効果")]
    public int attackChangeNegative = 0;
    public int defenseChangeNegative = 0;
}
