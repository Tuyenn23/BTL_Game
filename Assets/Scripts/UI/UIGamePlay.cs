using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : UICanvas
{

    [Title("Button")] [SerializeField] private Button btnHide;
    [SerializeField] private Button btnPlayGame;
    [SerializeField] private Button btnExit;

    private bool isFistOpen;
    [SerializeField]
    private Text txtHp;
    

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventConstants.UPDATE_HP_PLAYER, UpdateHPPLayer);
    }

    private void UpdateHPPLayer()
    {
        txtHp.text = EventManager.GetFloat(EventConstants.UPDATE_HP_PLAYER).ToString();
    }

    private void Exit()
    {
        OnBackPressed();
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
        ShowAniHide();

    }

    public void ShowAniHide()
    {
        Show(false);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventConstants.UPDATE_HP_PLAYER, UpdateHPPLayer);
        PlayerDataManager.Instance.SetIndexWave(0);
    }
}
