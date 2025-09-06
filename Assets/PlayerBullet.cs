using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Vector3 velocity;

    // 弾の速度セット
    public void SetVelocity(Vector3 vel)
    {
        velocity = vel;
    }

    void Start()
    {
        // 3秒後に自動的に弾を消す
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }
        }

        Destroy(gameObject);
    }
}
