using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class EnemyMove : MonoBehaviour
{
    public float moveSpeed = 5f;  // 移動速度

    public EnemyMove(float moveSpeed) => this.moveSpeed = moveSpeed;

    void Update() =>
        // Z軸方向に前進する（Zが大きくなる）
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
