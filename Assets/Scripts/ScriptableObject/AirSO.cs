using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DataAir", menuName = "ScriptableObjects/Data Airs", order = 1)]
public class AirSO : SerializedScriptableObject
{
    [SerializeField]
    [TableList(ShowIndexLabels = true, DrawScrollView = true, MaxScrollViewHeight = 400, MinScrollViewHeight = 200)]
    private List<DataAir> dataAir = new List<DataAir>();

    public DataAir GetDataAirById(int id)
    {
        return dataAir.FirstOrDefault((e) => e.id == id);
    }
}

public class DataAir
{
    public int id;
}
