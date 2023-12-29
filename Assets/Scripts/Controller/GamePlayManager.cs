using Game_DrawCar;
using Game_Fly;
using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;
using UnityEngine.Events;

public class GamePlayManager : MonoBehaviour
{
    public void ChangeStateEndGame(LevelResult levelResult)
    {
        switch (levelResult)
        {
            case LevelResult.Win:
                ActionWin();
                break;
            case LevelResult.Lose:
                ActionLose();
                break;
            default:
                break;
        }
    }

    private void ActionWin()
    {
        GameManager.Instance.UiController.OpenUiWin();
        GameManager.Instance.IncreaseLevel(GameManager.Instance.levelPlaying);
        GameManager.Instance.isStartGame = true;

    }

    private void ActionLose()
    {
        GameManager.Instance.isStartGame = true;

    }


}

