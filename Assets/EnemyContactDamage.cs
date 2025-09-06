using UnityEngine;
using System.Collections;

public class EnemyContactDamage : MonoBehaviour
{
    public int damageAmount = 1;         // 与えるダメージ量
    public float damageInterval = 2f;    // 継続ダメージ間隔（秒）

    private Coroutine damageCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Players"))
        {
            // 接触した瞬間に1ダメージ
            CharacterStats playerStats = other.GetComponent<CharacterStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);
            }

            // 継続ダメージ開始
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DealDamageOverTime(other));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Players"))
        {
            // 継続ダメージ停止
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DealDamageOverTime(Collider target)
    {
        CharacterStats playerStats = target.GetComponent<CharacterStats>();
        while (playerStats != null)
        {
            yield return new WaitForSeconds(damageInterval);
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);
            }
            else
            {
                yield break; // CharacterStatsがなくなったら停止
            }
        }
    }
}
