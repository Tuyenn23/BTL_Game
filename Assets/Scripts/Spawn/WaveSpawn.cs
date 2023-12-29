using Game_Fly;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawn : MonoBehaviour
{

    private static WaveSpawn s_instance;

    [SerializeField]
    private GameObject itemDrag;
    [SerializeField]
    private List<Transform> pointPos;
    private int count = 1;

    public BoxFormation boxFormation;
    public RadialFormation radialFormation;
    public AsteroidSpawner asteroidSpawner;
    public SpawnMiniBoss miniBoss;
    public UnityAction OnEnemyDead;
    private int countInvoke;

    private void Awake()
    {
        s_instance = this;
    }

    private void Start()
    {
        OnEnemyDead += CheckCompleteWave;
    }
    
    private void CheckCompleteWave()
    {
        countInvoke++;
        if(boxFormation != null)
        {
            if(countInvoke >= boxFormation.countEnemy)
            {
                GameManager.Instance.gamePlayManager.ChangeStateEndGame(LevelResult.Win);
            }
        } else if (radialFormation != null)
        {
            if (countInvoke >= radialFormation.countEnemy)
            {
                GameManager.Instance.gamePlayManager.ChangeStateEndGame(LevelResult.Win);
            }
        }
        else if (asteroidSpawner != null)
        {
            if (countInvoke >= asteroidSpawner.countEnemy)
            {
                GameManager.Instance.gamePlayManager.ChangeStateEndGame(LevelResult.Win);
            }
        }
        else if (miniBoss != null)
        {
            if (countInvoke >= miniBoss.countEnemy)
            {
                GameManager.Instance.gamePlayManager.ChangeStateEndGame(LevelResult.Win);
            }
        }
    }

    public static WaveSpawn S_instance { get => s_instance; set => s_instance = value; }

    public void DelayedDestroyLevel(float delayTime = 0.2f)
    {
        StartCoroutine(DelayedEndgameCoroutine(delayTime));
    }

    private IEnumerator DelayedEndgameCoroutine(float delayTime)
    {
        //yield return Yielders.Get(delayTime);
        yield return new WaitForSeconds(delayTime);
        if (gameObject.activeInHierarchy == true)
        {
            Destroy(gameObject);
        }
    }

    /*    private void DelayDropItem()
        {
            int indexPos = Random.Range(0, pointPos.Count);
            if (GameManager.Instance.Count == 6)
            {
                if (count == 1)
                {
                    Instantiate(itemDrag, pointPos[indexPos].position, Quaternion.identity);
                    count--;
                }
            }
        }*/

    private void OnDisable()
    {
        countInvoke = 0;
        OnEnemyDead -= CheckCompleteWave;
    }
}
