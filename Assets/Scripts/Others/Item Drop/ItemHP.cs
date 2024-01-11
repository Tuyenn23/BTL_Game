using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;


public class ItemHP : ItemDropBase
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagConst.PLAYER))
        {
            EventManager.SetData(EventConstants.ADD_HP_PLAYER, 1);
            EventManager.EmitEvent(EventConstants.ADD_HP_PLAYER);
            AudioController.Instance.PlaySound(AudioController.Instance.getItem);
            gameObject.SetActive(false);
        }
        
    }
}
