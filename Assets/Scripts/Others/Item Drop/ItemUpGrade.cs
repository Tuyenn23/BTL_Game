using Game_Fly;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUpGrade : ItemDropBase
{
    public Transform pos_2, pos_3, pos_4, pos_5;
    protected override void Start()
    {
        base.Start();
        initPos();
    }
    private void initPos()
    {
        pos_2 = GameManager.Instance.gamePlayManager.Air.firePos_2;
        pos_3 = GameManager.Instance.gamePlayManager.Air.firePos_3;
        pos_4 = GameManager.Instance.gamePlayManager.Air.firePos_4;
        pos_5 = GameManager.Instance.gamePlayManager.Air.firePos_5;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagConst.PLAYER))
        {
            AudioController.Instance.PlaySound(AudioController.Instance.getItem);
            int count = GameManager.Instance.gamePlayManager.increaseCount();
            if (count == 2)
            {
                AudioController.Instance.PlaySound(AudioController.Instance.upgradeDone);
                pos_2.gameObject.SetActive(true);
                pos_3.gameObject.SetActive(true);
            }
            if (count == 4)
            {
                AudioController.Instance.PlaySound(AudioController.Instance.upgradeDone);
                pos_4.gameObject.SetActive(true);
                pos_5.gameObject.SetActive(true);

            }
            gameObject.SetActive(false);


        }
    }
}
