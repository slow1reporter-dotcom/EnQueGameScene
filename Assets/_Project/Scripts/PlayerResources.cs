using System;

[Serializable] // JSON に変換するために必要
public class PlayerResources
{
    public int trainingCrystals = 1000000;   // キャラ育成用
    public int emeralds = 100000;           // 才能値強化用
    public int summonCrystals = 1000000;     // ガチャ用
    public int gold = 1000000;               // トレードやショップ用
    public int stamina = 100000;          // 行動力
    public int playerLevel = 100000;        // プレイヤー自身のレベル
}
