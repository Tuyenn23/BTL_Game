using Game_Fly;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : UICanvas
{
    [Title("Button")] [SerializeField] private Button btnHide;
    [SerializeField] private Button btnPlayGame;

    public Button BtnPlay => btnHide;
    private bool isFistOpen;
    public Text txtWaveComplete;

    // Start is called before the first frame update
    void Start()
    {
        btnPlayGame.onClick.AddListener(OnClickBtnPlay);
    }

    private void OnEnable()
    {
        txtWaveComplete.text = "Wave " + PlayerDataManager.Instance.GetIndexWave() + " Complete";
    }

    private void OnClickBtnPlay()
    {
        ShowAniHide();
        GameManager.Instance.loadLevelManager.LoadLevelStartGame();
        GameManager.Instance.isStartGame = true;
    }

    public void ShowAniHide()
    {
        Show(false);
    }
}
