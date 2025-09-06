using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentDatabase", menuName = "Game/EquipmentDatabase")]
public class EquipmentDatabase : ScriptableObject
{
    private static EquipmentDatabase _instance;

    public static EquipmentDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                // Resources フォルダ内からロードする
                _instance = Resources.Load<EquipmentDatabase>("EquipmentDatabase");

                if (_instance == null)
                {
                    Debug.LogError("EquipmentDatabase が Resources フォルダに見つかりません。");
                }
            }
            return _instance;
        }
    }

    [Header("登録されている全装備")]
    public EquipmentData[] allEquipment;

    // 名前で装備を検索
    public EquipmentData GetEquipmentByName(string name)
    {
        foreach (var eq in allEquipment)
        {
            if (eq.equipmentName == name)
                return eq;
        }
        return null;
    }

    // ID で装備を検索
    public EquipmentData GetEquipmentByID(int id)
    {
        foreach (var eq in allEquipment)
        {
            if (eq.itemID == id)
                return eq;
        }
        return null;
    }
}
