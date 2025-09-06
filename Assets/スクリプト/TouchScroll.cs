using UnityEngine;

public class VerticalScroll : MonoBehaviour
{
    public float scrollSpeed = 0.5f; // スクロール速度

    void Update()
    {
        // -----------------------
        // スマホ（タッチ操作）
        // -----------------------
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                // 縦方向のみに変換
                float deltaY = touch.deltaPosition.y * scrollSpeed;
                transform.Translate(new Vector3(0, deltaY, 0));
            }
        }

        // -----------------------
        // PC（マウスホイール）
        // -----------------------
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel"); 
        if (Mathf.Abs(scrollWheel) > 0.01f) // 少しでも動いたら
        {
            float deltaY = scrollWheel * 100f * scrollSpeed; 
            transform.Translate(new Vector3(0, deltaY, 0));
        }
    }
}

