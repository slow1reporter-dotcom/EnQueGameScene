using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("攻撃設定")]
    public float attackRadius = 3f; // 攻撃範囲（半径）
    public int damageAmount = 10;   // 与えるダメージ

    [Header("攻撃キー設定")]
    public KeyCode attackKey = KeyCode.Space; // 攻撃キー

    private int enemyLayer; // Enemyレイヤー番号

    void Start()
    {
        // Unityのレイヤー番号を取得
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            AttackEnemiesInRadius();
        }
    }

    private void AttackEnemiesInRadius()
    {
        // Enemyレイヤーだけを対象にするマスクを作成
        int layerMask = 1 << enemyLayer;

        // 半径内のコライダーを取得
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius, layerMask);

        foreach (Collider hit in hitColliders)
        {
            // まず EnemyHealth を探す
            EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
                continue; // 他のチェックは飛ばす
            }

            // 次に EnemyHealthBoss を探す
            EnemyHealthBoss bossHealth = hit.GetComponent<EnemyHealthBoss>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damageAmount);
            }
        }

        Debug.Log($"半径 {attackRadius}m 内の Enemy レイヤーの敵に {damageAmount} ダメージを与えました。");
    }

    // Sceneビューで攻撃範囲を表示
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
