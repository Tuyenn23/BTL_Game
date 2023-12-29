using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public UiMainLobby UiMainLobby;
    public UIWin UiWin;
    public UIGamePlay UiGamePlay;

    public void Init()
    {
        OpenUiMainLobby();
    }

    public void OpenUiMainLobby()
    {
        UiMainLobby.Show(true);
    }
    public void OpenUiGamePlay()
    {
        UiGamePlay.Show(true);
    }

    public void OpenUiWin()
    {
        UiWin.Show(true);
    }
}
