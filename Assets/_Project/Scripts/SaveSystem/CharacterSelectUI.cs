using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterSelectUI : MonoBehaviour
{
    [Header("キャラクターリスト")]
    public List<CharacterData> allCharacters; // Inspectorで登録

    [Header("UIプレハブ")]
    public GameObject characterButtonPrefab; // ボタンプレハブ（Button + Image + Text）
    public Transform buttonParent;           // ボタンを置く親Transform

    private void Start()
    {
        // 全キャラクターのボタンを生成
        foreach (var data in allCharacters)
        {
            // 修正：SaveManagerのメソッド名に合わせる
            var runtime = SaveManager.Instance.GetOrCreateRuntimeCharacter(data);

            Debug.Log($"{data.characterName} 表示Lv:{runtime.level} HP:{runtime.currentHP}");

            // ボタン生成
            GameObject buttonObj = Instantiate(characterButtonPrefab, buttonParent);
            Button button = buttonObj.GetComponent<Button>();
            Image icon = buttonObj.GetComponentInChildren<Image>();
            Text nameText = buttonObj.GetComponentInChildren<Text>();

            if (icon != null) icon.sprite = data.characterSprite2D;
            if (nameText != null) nameText.text = $"{data.characterName} Lv:{runtime.level}";

            // ボタンクリックでキャラクター選択
            button.onClick.AddListener(() => OnSelect(data));
        }
    }

    // キャラクター選択処理
    public void OnSelect(CharacterData data)
    {
        // 修正：SaveManagerのメソッド名に合わせる
        var runtime = SaveManager.Instance.GetOrCreateRuntimeCharacter(data);

        // SaveDataに反映
        SaveManager.Instance.UpdateCharacterRuntime(runtime);

        Debug.Log($"選択: {data.characterName} Lv:{runtime.level} HP:{runtime.currentHP}");

        // シーン遷移（必要なら）
        // SceneManager.LoadScene("TrainingScene");
    }
}
