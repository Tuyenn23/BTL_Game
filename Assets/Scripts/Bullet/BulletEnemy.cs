using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;

public class BulletEnemy : BaseBullet
{
    [SerializeField]
    protected GameObject effect, effect_2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagConst.PLAYER))
        {
            this.Send(collision.transform);
            AudioController.Instance.PlaySound(AudioController.Instance.playerHit);
            this.DestroyBullet();
        }
        if (collision.CompareTag(TagConst.LIMIT))
        {
            this.UnTouch();
        }
        if (collision.CompareTag(TagConst.SHIELD))
        {
            Instantiate(effect_2, transform.position, Quaternion.identity);
            AudioController.Instance.PlaySound(AudioController.Instance.shieldHit);
            this.UnTouch();
        }
    }

    protected virtual void DestroyBullet()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
        SimplePool.Despawn(gameObject);
        
    }

    protected virtual void UnTouch()
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

    
}
