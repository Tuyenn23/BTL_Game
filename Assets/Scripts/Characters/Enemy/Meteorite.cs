using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Meteorite : MonoBehaviour
{

    public float rotationAmount = 360f;
    public float rotationDuration = 1f;

    private void Start()
    {
        RotateZ();
    }

    void RotateZ()
    {
         transform.DORotate(new Vector3(0f, 0f, rotationAmount), rotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .OnComplete(() => RotateZ());
        transform.DOMoveY(-20f, 8f);/*.SetEase(Ease.Linear);*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagConst.LIMIT))
        {
            WaveSpawn.S_instance.OnEnemyDead?.Invoke();
            Destroy(gameObject);
        }
    }
}
