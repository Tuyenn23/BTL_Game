using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    [SerializeField]
    protected GameObject bossPrefab, miniBoss;

    [SerializeField]
    protected GameObject effect;
    
    [SerializeField]
    protected Transform pos1, pos2;

    [SerializeField]
    float timeDelay;

    [SerializeField]
    private EnemyBossType enemyBossType;

    public int countEnemy = 0;
    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        if(enemyBossType == EnemyBossType.MiniBoss)
        {
            Instantiate(effect, pos1.position, Quaternion.identity);
            Instantiate(effect, pos2.position, Quaternion.identity);

            yield return new WaitForSeconds(timeDelay);
            Instantiate(miniBoss, pos1.position, Quaternion.identity);
            Instantiate(miniBoss, pos2.position, Quaternion.identity);
        }
        else if(enemyBossType == EnemyBossType.Boss)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            AudioController.Instance.PlaySound(AudioController.Instance.warnningBoss);
            yield return new WaitForSeconds(timeDelay);
            Instantiate(bossPrefab, pos1.position, Quaternion.identity);
        }
        

    }

    private void OnDisable()
    {
        StopCoroutine(Spawn());
    }
}
