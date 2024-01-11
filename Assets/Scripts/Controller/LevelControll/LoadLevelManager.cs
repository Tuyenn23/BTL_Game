using Game_DrawCar;
using Game_Fly;
using System.Collections;
using System.Collections.Generic;
using TigerForge;
using Unicorn.Utilities;
using UnityEngine;

public class LoadLevelManager : MonoBehaviour
{
    public int indexLevelRandom;

    public void LoadLevelStartGame()
    {
        /*GameManager.Instance.addressableGame*/
        AddressablesUtils.Instance.SpawnObject(GameManager.Instance.levelPlaying, PlayerDataManager.Instance.GetIndexWave());
    }
}
