using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemLevelElement : MonoBehaviour
{

    [SerializeField]
    private GameObject Lock;
    [SerializeField]
    private int indexLevel;

    private void OnEnable()
    {
        CheckIsUnlock();
    }

    private void CheckIsUnlock()
    {
        if(DataLevel.GetLevel() >= indexLevel)
        {
            Lock.SetActive(false);
        }
        else
        {
            Lock.SetActive(true);
        }
    }

    public void SelectLevel()
    {
        if(!Lock.activeSelf)
        {
            PlayerDataManager.Instance.SetChooseIndexLevel(indexLevel);
            SceneManager.LoadScene("SampleScene");
        }
    }
}
