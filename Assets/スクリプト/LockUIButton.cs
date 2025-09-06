   using UnityEngine;

public class LockUIButton : MonoBehaviour
{
    private Vector3 fixedPosition;

    void Start()
    {
        // 初期位置を保存
        fixedPosition = transform.position;
    }

    void LateUpdate()
    {
        // 常に固定位置に戻す
        transform.position = fixedPosition;
    }
}
