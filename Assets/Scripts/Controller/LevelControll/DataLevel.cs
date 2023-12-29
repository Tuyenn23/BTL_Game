using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Game_DrawCar;

public static class DataLevel
{
    private static string ALL_DATA_LEVEL = "ALL_DATA_LEVEL";

    private static DataLevelModel dataLevelModel;

    static DataLevel()
    {
        dataLevelModel = JsonConvert.DeserializeObject<DataLevelModel>(PlayerPrefs.GetString(ALL_DATA_LEVEL));

        if (dataLevelModel == null)
        {
            dataLevelModel = new DataLevelModel();
            dataLevelModel.CurrentLevel = 1;
        }

        SaveDataLevel();
    }

    private static void SaveDataLevel()
    {
        string json = JsonConvert.SerializeObject(dataLevelModel);
        PlayerPrefs.SetString(ALL_DATA_LEVEL, json);
    }

    public static void SetLevel(int level)
    {
        dataLevelModel.SetLevel(level);
        SaveDataLevel();

    }

    public static int GetLevel()
    {
        return dataLevelModel.GetLevel();
    }
}

public class DataLevelModel
{

    public int CurrentLevel;

    public void SetLevel(int level)
    {
        CurrentLevel = level;
    }

    public int GetLevel()
    {
        return CurrentLevel;
    }
}
