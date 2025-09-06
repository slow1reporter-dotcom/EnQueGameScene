using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GameData;

public class TrainingButton : MonoBehaviour
{
    private CharacterRuntime runtimeCharacter;                  
    public PlayerResourceManager playerResourcesManager; 

    public int crystalCost = 1;                          

    [Header("UI 参照")]
    public TextMeshProUGUI levelText;       
    public TextMeshProUGUI crystalsText;    
    public TextMeshProUGUI totalPowerText;  
    public TextMeshProUGUI totalTalentText; 
    public Image chibiImage;                
    public Image elementOrbImage;           
    public TextMeshProUGUI specialText;     
    public TextMeshProUGUI skillsText;      

    [Header("ステータスバー")]
    public Slider hpSlider;
    public Slider attackSlider;
    public Slider defenseSlider;

    [Header("星画像")]
    public Image[] starImages;              

    void Start()
    {
        var selectedCharacterData = GameManager1.SelectedCharacterData;

        if (selectedCharacterData == null)
        {
            Debug.LogError("育成対象キャラクターが設定されていません！");
            return;
        }

        // Runtimeキャラクターを取得または新規作成
        runtimeCharacter = SaveManager.Instance.GetOrCreateRuntimeCharacter(selectedCharacterData);

        UpdateUI();
    }

    public void OnClickLevelUp()
    {
        if (runtimeCharacter == null || playerResourcesManager == null) return;

        if (playerResourcesManager.resources.trainingCrystals < crystalCost)
        {
            Debug.Log("育成クリスタルが足りません！");
            return;
        }

        // クリスタル消費
        playerResourcesManager.resources.trainingCrystals -= crystalCost;
        playerResourcesManager.SaveResources();

        // レベルアップ
        runtimeCharacter.LevelUp();

        // 永続化
        SaveManager.Instance.UpdateCharacterRuntime(runtimeCharacter);

        UpdateUI();

        Debug.Log($"{runtimeCharacter.template.characterName} がレベルアップ！ 現在Lv {runtimeCharacter.level}");
    }

    private void UpdateUI()
    {
        if (runtimeCharacter == null) return;

        var template = runtimeCharacter.template;

        if (levelText != null) levelText.text = $"Lv. {runtimeCharacter.level}";
        if (crystalsText != null) crystalsText.text = $" {playerResourcesManager.resources.trainingCrystals}";
        if (totalPowerText != null) totalPowerText.text = $" {runtimeCharacter.TotalPower}";
        if (totalTalentText != null) totalTalentText.text = $" {runtimeCharacter.TotalTalent}";

        if (chibiImage != null && template.chibiSprite2D != null)
            chibiImage.sprite = template.chibiSprite2D;

        if (elementOrbImage != null && template.elementOrbSprite2D != null)
            elementOrbImage.sprite = template.elementOrbSprite2D;

        if (specialText != null)
            specialText.text = $" {template.specialDescription}";

        if (skillsText != null)
        {
            skillsText.text = "\n";
            foreach (var skill in template.conditionalSkills)
                skillsText.text += $" {skill.description}\n";
        }

        if (hpSlider != null) hpSlider.value = runtimeCharacter.currentHP;
        if (attackSlider != null) attackSlider.value = runtimeCharacter.currentAttack;
        if (defenseSlider != null) defenseSlider.value = runtimeCharacter.currentDefense;

        UpdateStars();
    }

    private void UpdateStars()
    {
        if (starImages == null) return;

        foreach (var img in starImages)
            img.gameObject.SetActive(false);

        int level = runtimeCharacter.StarLevel;
        for (int i = 0; i < level && i < starImages.Length; i++)
            starImages[i].gameObject.SetActive(true);
    }
}
