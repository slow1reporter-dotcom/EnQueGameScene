using UnityEngine;

public class PageSwitcher : MonoBehaviour
{
    [Header("切り替える画面を順番に登録してください")]
    public GameObject[] pages; // 0=Main, 1=ConfirmSingle, 2=Confirm11, 3=Gacha, 4=Result

    void Start()
    {
        Show(0); // ゲーム開始時は index 0 (メイン画面) を表示
    }

    public void Show(int index)
    {
        if (pages == null || pages.Length == 0) return;

        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] != null)
                pages[i].SetActive(i == index);
        }
    }
}
