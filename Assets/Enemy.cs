using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootInterval = 2f;
    public Transform firePoint;

    void Start()
    {
        InvokeRepeating(nameof(Shoot), 1f, shootInterval);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // プレイヤーの方向を向かせる（必要なら追加）
        Transform player = GameObject.FindWithTag("Players")?.transform;
        if (player != null)
        {
            Vector3 dir = (player.position - firePoint.position).normalized;
            bullet.transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
