using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataWaveSO", menuName = "ScriptableObjects/Data Wave", order = 1)]
public class DataWaveSO : SerializedScriptableObject
{

    [SerializeField]
    [TableList(ShowIndexLabels = true, DrawScrollView = true, MaxScrollViewHeight = 400, MinScrollViewHeight = 200)]
    private Dictionary<int, WaveData> dataWaves = new Dictionary<int, WaveData>();

    public List<GameObject> GetListWaveByIdLevel(int indexLevel)
    {
        if(dataWaves.ContainsKey(indexLevel))
        {
            return dataWaves[indexLevel].wave;
        }
        return null;
    }
}

public class WaveData
{
    public List<GameObject> wave;
}
