using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;

public class BulletEnemy : BaseBullet
{
    [SerializeField]
    protected GameObject effect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagConst.PLAYER))
        {
            this.Send(collision.transform);
            this.DestroyBullet();
        }
        if (collision.CompareTag(TagConst.LIMIT))
        {
            this.Limit();
        }
    }

    protected virtual void DestroyBullet()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
        SimplePool.Despawn(gameObject);
        //Destroy(gameObject);
    }

    protected virtual void Limit()
    {
        SimplePool.Despawn(gameObject);

    }
    private void Update()
    {
        MoveBullet();
    }

    protected override void MoveBullet()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagConst.PLAYER))
        {
            Debug.Log("player chet");
            Destroy(collision.gameObject);
            Destroy(gameObject);

        }
    }*/
}
