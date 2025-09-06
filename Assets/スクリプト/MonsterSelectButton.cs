using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterSelectButton : MonoBehaviour
{
    [Header("このボタンがどのモンスターを表すか")]
    public CharacterData characterData; // ScriptableObject をセット

    public void OnClick()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager がシーンに存在しません！");
            return;
        }

        if (characterData == null)
        {
            Debug.LogError("MonsterSelectButton に CharacterData がセットされていません！");
            return;
        }

        // Runtimeキャラクターを取得（必須ではないが安全策として）
        var runtime = SaveManager.Instance.GetOrCreateRuntimeCharacter(characterData);

        // GameManager で選択キャラを共有
        GameManager1.SelectedMonsterId = characterData.characterName;
        GameManager1.SelectedCharacterData = characterData;

        // シーン遷移
        SceneManager.LoadScene("Training Scene");
    }
}
