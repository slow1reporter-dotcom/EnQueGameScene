using UnityEngine;

public class GachaSystem : MonoBehaviour
{
    [Header("ガチャ対象キャラクター")]
    public CharacterData[] allCharacters; // ScriptableObjectリスト
    public Transform spawnPoint;          // 生成位置

    // ガチャを1回引く
    public void PullGacha()
    {
        if (allCharacters.Length == 0 || spawnPoint == null)
        {
            Debug.LogWarning("ガチャの設定が不完全です。");
            return;
        }

        // ランダムでキャラを選択
        int idx = Random.Range(0, allCharacters.Length);
        CharacterData selectedData = allCharacters[idx];

        if (selectedData.characterPrefab == null)
        {
            Debug.LogError($"CharacterData: {selectedData.characterName} にPrefabが設定されていません。");
            return;
        }

        // キャラクターをシーンに生成
        GameObject charObj = Instantiate(selectedData.characterPrefab, spawnPoint.position, Quaternion.identity);
        CharacterStats charStats = charObj.GetComponent<CharacterStats>();

        if (charStats == null)
        {
            Debug.LogError("キャラPrefabにCharacterStatsがアタッチされていません。");
            Destroy(charObj);
            return;
        }

        // ScriptableObjectをセット
        charStats.data = selectedData;

        // 才能値をランダム化
        RandomizeTalent(charStats.data);

        // 戦闘用ステータス初期化
        charStats.InitializeStats();

        Debug.Log($"ガチャ排出: {selectedData.characterName} (星{selectedData.starLevel}) " +
                  $"才能値: HP{selectedData.talentHP}%, 攻撃{selectedData.talentAttack}%, 防御{selectedData.talentDefense}%");
    }

    // 才能値ランダム化
    private void RandomizeTalent(CharacterData data)
    {
        data.talentHP = Random.Range(0, 76);      // 0～75%
        data.talentAttack = Random.Range(0, 76);  // 0～75%
        data.talentDefense = Random.Range(0, 51); // 0～50%
    }
}
