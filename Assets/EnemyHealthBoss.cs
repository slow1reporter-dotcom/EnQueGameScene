using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBoss : MonoBehaviour
{
    [Header("ステータス")]
    public int maxHP = 100;
    private int currentHP;

    [Header("固定UIのHPバー")]
    public Image hpBarFull; // Canvas上の固定HPバーのImage（Fill）

    void Start()
    {
        currentHP = maxHP;
        UpdateHPBar();
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPBar();

        Debug.Log($"[Boss] HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void UpdateHPBar()
    {
        if (hpBarFull != null)
        {
            hpBarFull.fillAmount = (float)currentHP / maxHP;
        }
        else
        {
            Debug.LogWarning("Boss HPバーがInspectorで設定されていません。");
        }
    }

    private void Die()
    {
        Debug.Log("ボス撃破！");
        Destroy(gameObject);
    }
}
