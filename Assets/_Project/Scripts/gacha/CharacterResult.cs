using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameData;

public class CharacterResult : MonoBehaviour
{
    [Header("UI Elements")]
    public Image characterImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI levelText;

    public void SetData(CharacterRuntime runtime)
    {
        if (runtime == null) return;

        if (characterImage != null)
            characterImage.sprite = runtime.template.characterSprite2D;

        if (nameText != null)
            nameText.text = runtime.template.characterName;

        if (rarityText != null)
            rarityText.text = runtime.template.rarity.ToString();

        if (levelText != null)
            levelText.text = $": {runtime.level}/{runtime.CurrentMaxLevel}";
    }
}
