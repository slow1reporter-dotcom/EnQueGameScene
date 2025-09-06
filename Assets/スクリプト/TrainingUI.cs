using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrainingUI : MonoBehaviour
{
    public TMP_Text monsterName;   // モンスター名
    public Image monsterImage;     // モンスター画像

    void Start()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager が存在しません！");
            return;
        }

        // 選択中キャラを取得
        string selectedName = GameManager1.SelectedMonsterId; // ボタンでセット済み
        var character = SaveManager.Instance.CurrentSaveData.characters
                            .Find(x => x.characterName == selectedName);

        if (character == null)
        {
            monsterName.text = "？？？";
            monsterImage.sprite = null;
            return;
        }

        // 名前表示
        monsterName.text = character.characterName;

        // Sprite は CharacterData 側に持たせておくのがベスト
        CharacterData dataAsset = Resources.Load<CharacterData>("CharacterData/" + character.characterName);
        if (dataAsset != null && dataAsset.characterSprite2D != null)
        {
            monsterImage.sprite = dataAsset.characterSprite2D;
        }
        else
        {
            Debug.LogWarning($"CharacterData/ {character.characterName} が見つかりません or Spriteなし");
            monsterImage.sprite = null;
        }
    }
}
