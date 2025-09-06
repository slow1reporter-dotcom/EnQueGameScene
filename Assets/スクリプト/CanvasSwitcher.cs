using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject canvasMain;      // ガチャ画面
    public GameObject canvasConfirm;   // 確認画面

    // ガチャボタンを押したとき
    public void ShowConfirm()
    {
        canvasMain.SetActive(false);      // ガチャ画面を隠す
        canvasConfirm.SetActive(true);    // 確認画面を表示
    }

    // OKボタンを押したとき
    public void OnConfirmOK()
    {
        Debug.Log("ガチャを実行！"); 
        canvasConfirm.SetActive(false);
        canvasMain.SetActive(true);       // 必要なら戻す
    }

    // キャンセルボタンを押したとき
    public void OnConfirmCancel()
    {
        canvasConfirm.SetActive(false);
        canvasMain.SetActive(true);       // 戻す
    }
}
