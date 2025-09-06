using UnityEngine;

public class PlayerAutoAttack1 : MonoBehaviour
{
    [Header("攻撃設定")]
    public float attackRadius = 3f;     // 攻撃範囲（半径）
    public float attackInterval = 2f;   // 攻撃間隔（秒）
    public float attackDelay = 0.5f;    // アニメ開始から攻撃発動までの遅延
    public float effectDuration = 0.5f; // エフェクトの再生時間

    [Header("アニメーション設定")]
    public Animator animator;           // 攻撃アニメーション用Animator
    public string magicBoolName = "Magic"; // AnimatorのBool名

    [Header("エフェクト設定")]
    public GameObject attackEffectPrefab; // Particle SystemのPrefab
    public Transform effectSpawnPoint;    // エフェクトを出す位置（手の位置）

    private int enemyLayer;
    private float attackTimer;

    private CharacterStats playerStats; // Playerのステータス参照用

    void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        attackTimer = attackInterval;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        playerStats = GetComponent<CharacterStats>();
        if (playerStats == null)
        {
            Debug.LogError("CharacterStats がアタッチされていません！");
        }
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            // 攻撃中としてMagicをtrueにする
            if (animator != null)
            {
                animator.SetBool(magicBoolName, true);
            }

            // 遅延して攻撃＆エフェクト
            Invoke(nameof(DelayedAttack), attackDelay);

            attackTimer = attackInterval;
        }
    }

    private void DelayedAttack()
    {
        AttackEnemiesInRadius();
        PlayAttackEffect();

        if (animator != null)
        {
            animator.SetBool(magicBoolName, false);
        }
    }

    private void AttackEnemiesInRadius()
    {
        if (playerStats == null) return;

        int layerMask = 1 << enemyLayer;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius, layerMask);

        foreach (Collider hit in hitColliders)
        {
            EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // damageAmount の代わりに CurrentAttack を使用
                enemyHealth.TakeDamage(playerStats.CurrentAttack);
            }
        }

        Debug.Log($"半径 {attackRadius}m 内の Enemy レイヤーの敵に {playerStats.CurrentAttack} ダメージを与えました。");
    }

    private void PlayAttackEffect()
    {
        if (attackEffectPrefab != null && effectSpawnPoint != null)
        {
            GameObject effect = Instantiate(attackEffectPrefab, effectSpawnPoint.position, effectSpawnPoint.rotation);

            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var main = ps.main;
                main.duration = effectDuration;
                ps.Play();
            }

            Destroy(effect, effectDuration);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
