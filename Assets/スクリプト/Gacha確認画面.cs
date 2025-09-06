using UnityEngine;

public class Gacha確認画面 : MonoBehaviour
{
    [Header("切り替える画面（Panel のルート GameObject）")]
    public GameObject pageGachaMain;
    public GameObject pageConfirmSingle;
    public GameObject pageConfirm11;

    void Start()
    {
        // ゲーム開始時はメイン画面を表示
        ShowGachaMain();
    }

    public void ShowGachaMain()
    {
        SetActiveExclusive(pageGachaMain);
    }

    public void ShowConfirmSingle()
    {
        SetActiveExclusive(pageConfirmSingle);
    }

    public void ShowConfirm11()
    {
        SetActiveExclusive(pageConfirm11);
    }

    /// <summary>
    /// 渡した1つだけActiveにして、他は全部オフにする
    /// </summary>
    void SetActiveExclusive(GameObject toShow)
    {
        if (pageGachaMain != null)     pageGachaMain.SetActive(toShow == pageGachaMain);
        if (pageConfirmSingle != null) pageConfirmSingle.SetActive(toShow == pageConfirmSingle);
        if (pageConfirm11 != null)     pageConfirm11.SetActive(toShow == pageConfirm11);
    }
}
