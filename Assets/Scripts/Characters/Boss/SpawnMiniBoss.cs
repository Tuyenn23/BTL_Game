using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMiniBoss : MonoBehaviour
{
    [SerializeField]
    protected GameObject bossPrefab;

    [SerializeField]
    protected GameObject effect;

    [SerializeField]
    protected Transform pos1, pos2;

    public int countEnemy = 0;

    [SerializeField]
    float timeDelay;
    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        Instantiate(effect, pos1.position, Quaternion.identity);
        Instantiate(effect, pos2.position, Quaternion.identity);

        yield return new WaitForSeconds(timeDelay);
        Instantiate(bossPrefab, pos1.position, Quaternion.identity);
        Instantiate(bossPrefab, pos2.position, Quaternion.identity);


    }

    private void OnDisable()
    {
        StopCoroutine(Spawn());
    }
}
