using Game_DrawCar;
using Game_Fly;
using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;
using UnityEngine.Events;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] Air air;
    public Air Air => air;
    int count;
    public int increaseCount()
    {
        return count++;
    }
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
        AddressablesUtils.Instance.listWaves.Clear();
    }

    public void ChangeStateEndWave()
    {
        int index = PlayerDataManager.Instance.GetIndexWave() + 1;
        PlayerDataManager.Instance.SetIndexWave(index);
        GameManager.Instance.UiController.OpenUiWin();
        GameManager.Instance.isStartGame = true;
    }

    private void ActionWin()
    {
        //int index = PlayerDataManager.Instance.GetIndexWave() + 1;
        //PlayerDataManager.Instance.SetIndexWave(index);
        GameManager.Instance.UiController.OpenUiWin();
        GameManager.Instance.IncreaseLevel(GameManager.Instance.levelPlaying);
        GameManager.Instance.isStartGame = true;

    }

    private void ActionLose()
    {
        GameManager.Instance.UiController.OpenUiLose();
        GameManager.Instance.isStartGame = true;


    }

    private void OnApplicationQuit()
    {
        PlayerDataManager.Instance.SetIndexWave(0);
    }
}

