using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using DG.Tweening;
using Game_Fly;

public class ItemDrop : ItemDropBase
{


    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagConst.PLAYER))
        {
            if (itemType == TypeItem.Hp)
            {

                EventManager.SetData(EventConstants.ADD_HP_PLAYER, 1);
                EventManager.EmitEvent(EventConstants.ADD_HP_PLAYER);
                gameObject.SetActive(false);
            }
            else if (itemType == TypeItem.Shield)
            {

                shield.SetActive(true);
                gameObject.SetActive(false);

            }
            else if (itemType == TypeItem.Upgrade)
            {
                int count = GameManager.Instance.gamePlayManager.increaseCount();
                if (count == 3)
                {
                    pos_2.SetActive(true);
                    pos_3.SetActive(true);
                }
                if (count == 5)
                {
                    pos_4.SetActive(true);
                    pos_5.SetActive(true);

                }


                gameObject.SetActive(false);

            }
        }
    }



    private void OnEnable()
    {
        Destroy(gameObject, timeDestroy);
    }*/
}
