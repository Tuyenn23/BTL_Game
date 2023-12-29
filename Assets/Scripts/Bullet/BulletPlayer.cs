using Game_Fly;
using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;

public class BulletPlayer : BaseBullet
{
    [SerializeField]
    protected GameObject effect;
    [SerializeField]
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagConst.ENEMY))
        {
            this.Send(collision.transform);
            /*this.updateHealthBar();*/
            this.DestroyBullet();
        }
        if (collision.CompareTag(TagConst.LIMIT_2))
        {
            this.Limit();
        }
    }

    

    protected virtual void DestroyBullet()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
        //Destroy(gameObject);
        SimplePool.Despawn(gameObject);
    }

    protected virtual void Limit()
    {
        SimplePool.Despawn(gameObject);

    }

    /*protected virtual void updateHealthBar()
    {
        if (enemyType == EnemyType.Boss)
        {
            
            healthBar.SetHealth(ays.Hp);
        }
    }*/




    private void Update()
    {
        MoveBullet();
    }

    protected override void MoveBullet()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagConst.ENEMY))
        {
            Debug.Log("va cham");
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            *//*Destroy(collision.gameObject);*//*
            Destroy(gameObject);

        }
    }*/
}
