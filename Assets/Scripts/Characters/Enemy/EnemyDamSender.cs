using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamSender : DamageSender
{
    [SerializeField]
    protected float timeDelay;
    private float timer;
    [SerializeField]
    protected GameObject effect;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(TagConst.PLAYER))
        {
            this.timer -= Time.deltaTime;
            if (this.timer > 0) return;
            this.timer = this.timeDelay;
            this.Send(collision.transform);
            this.AtkPlayer();
        }
    }

    protected virtual void AtkPlayer()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
        AudioController.Instance.PlaySound(AudioController.Instance.playerHit);
    }

    
}