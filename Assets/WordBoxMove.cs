using UnityEngine;

public class WordBoxMove : MonoBehaviour
{
    public float moveSpeed = 5f;           // 初期移動速度
    public float speedIncreaseRate = 0.1f; // 1秒あたりの速度増加量
    public float maxSpeed = 15f;           // 最大速度

    void Update()
    {
        // 時間経過で速度アップ（maxSpeedまで）
        moveSpeed = Mathf.Min(maxSpeed, moveSpeed + speedIncreaseRate * Time.deltaTime);

        // Z軸負方向 + 下方向に移動
        Vector3 moveDirection = (Vector3.back + Vector3.down).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // 画面外に出たら削除
        if (transform.position.y < -10f || transform.position.z < -20f)
        {
            Destroy(gameObject);
        }
    }
}
