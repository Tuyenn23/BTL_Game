using Game_Fly;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMainLobby : UICanvas
{

    [Title("Button")] [SerializeField] private Button btnHide;
    [SerializeField] private Button btnPlayGame;

    public Button BtnPlay => btnHide;
    private bool isFistOpen;

    // Start is called before the first frame update
    void Start()
    {
        btnPlayGame.onClick.AddListener(OnClickBtnPlay);
        Init();
    }

    private void Init()
    {
    }

    public override void Show(bool _isShown, bool isHideMain = true)
    {
        base.Show(_isShown, isHideMain);
        if (IsShow)
        {
            Init();
        }
    }


    private void OnClickBtnPlay()
    {
        gameObject.SetActive(false);
        GameManager.Instance.loadLevelManager.LoadLevelStartGame();
        GameManager.Instance.isStartGame = true;
    }

    public void ShowAniHide()
    {
        Show(false);
    }
}
