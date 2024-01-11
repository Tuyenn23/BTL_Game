using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game_Fly;
public class ItemSheild : ItemDropBase
{
    [SerializeField] GameObject shield;
    
    protected override void Start()
    {
        base.Start();
        initSheild();
    }
    private void initSheild()
    {
        shield = GameManager.Instance.gamePlayManager.Air.Sheild;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagConst.PLAYER))
        {
            shield.SetActive(true);
            AudioController.Instance.PlaySound(AudioController.Instance.getItem);
            gameObject.SetActive(false);
        }
    }
}
