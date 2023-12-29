using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSender : MonoBehaviour
{
    [SerializeField]
    protected float damage = 1;
    [SerializeField]
    /*private EnemyType enemyType;
    public HealthBarBoss healthBar;*/
    public virtual void Send(Transform obj)
    {
        DamageReceiver damageReceiver = obj.GetComponentInChildren<DamageReceiver>();
        if (damageReceiver == null) return;
        this.Send(damageReceiver);
    }

    public virtual void Send(DamageReceiver damageReceiver)
    {
        damageReceiver.Deduct(damage);
        this.DestroyObject();
    }

    protected virtual void DestroyObject()
    {

    }
}
