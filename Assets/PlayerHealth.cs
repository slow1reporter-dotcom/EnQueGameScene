using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("基本ステータス")]
    public int maxHP = 3;
    private int currentHP;

    public int attack = 10;
    public int defense = 5;

    public int CurrentHP => currentHP; // 読み取り専用
    public int MaxHP => maxHP;

    [Header("HPバー UI")]
    public Image hpBarFillImage;

    [Header("Game Over UI")]
    public GameObject gameOverText;

    void Start()
    {
        currentHP = maxHP;
        UpdateHPBar();

        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - defense, 0); // 防御力反映
        currentHP -= actualDamage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPBar();

        if (currentHP <= 0)
        {
            Debug.Log("ゲームオーバー！");
            GameOver();
        }
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPBar();
    }

    private void UpdateHPBar()
    {
        if (hpBarFillImage != null)
        {
            float fill = (float)currentHP / maxHP;
            hpBarFillImage.fillAmount = fill;
        }
    }

    private void GameOver()
    {
        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }
        Time.timeScale = 0f;
    }
}
