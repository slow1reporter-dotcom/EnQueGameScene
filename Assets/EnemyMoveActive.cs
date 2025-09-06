using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class EnemyMoveActive : MonoBehaviour
{
    public float moveSpeed = 5f;  
    private Rigidbody rb;
    private Transform target;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // 空中でも動かす場合
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // トンネリング防止
    }

    void FixedUpdate()
    {
        if (target == null) FindNearestPlayer();

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Players");
        if (players.Length == 0) return;

        float nearestDistance = Mathf.Infinity;
        GameObject nearestPlayer = null;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPlayer = player;
            }
        }

        if (nearestPlayer != null)
            target = nearestPlayer.transform;
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
