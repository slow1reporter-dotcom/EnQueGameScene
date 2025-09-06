using UnityEngine; 

public class Player : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;

    private Animator animator;
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private float swipeThreshold = 50f; // スワイプのしきい値（ピクセル）

    void Start()
    {
        // プレイヤーのAnimatorを自動取得
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleKeyboardInput();
        HandleSwipeInput();
    }

    // ⌨️ 十字キーの左右入力（反転なし）
    private void HandleKeyboardInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            // 移動（反転なし）
            transform.Translate(Vector3.right * horizontal * moveSpeed * Time.deltaTime);

            // アニメーション設定（方向一致）
            if (horizontal < 0) // 左キー
            {
                animator.SetBool("Left", true);
                animator.SetBool("Right", false);
            }
            else if (horizontal > 0) // 右キー
            {
                animator.SetBool("Right", true);
                animator.SetBool("Left", false);
            }
        }
        else
        {
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }
    }

    // 📱 スワイプ入力（反転なし）
    private void HandleSwipeInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                touchStartPos = touch.position;
                break;

            case TouchPhase.Ended:
                touchEndPos = touch.position;
                Vector2 swipeDelta = touchEndPos - touchStartPos;

                if (Mathf.Abs(swipeDelta.x) > swipeThreshold && Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    // 左右スワイプ認識（反転なし）
                    float direction = swipeDelta.x > 0 ? 1f : -1f;

                    transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime * 10);

                    // スワイプ方向に応じてアニメーション設定
                    if (direction < 0) // 左
                    {
                        animator.SetBool("Left", true);
                        animator.SetBool("Right", false);
                    }
                    else if (direction > 0) // 右
                    {
                        animator.SetBool("Right", true);
                        animator.SetBool("Left", false);
                    }
                }
                break;
        }
    }
}
