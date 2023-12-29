using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class DamageReceiver : MonoBehaviour
{

    [SerializeField]
    protected CircleCollider2D circleCollider2D;
    [SerializeField]
    protected bool isDead = false;
    [SerializeField]
    private float hp = 1;
    [SerializeField]
    protected float maxHp = 1;

    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float Hp { get => hp; set => hp = value; }

    protected virtual void OnEnable()
    {
        this.Rebord();
    }

    private void Start()
    {
        LoadComponents();
    }
    protected void LoadComponents()
    {
        this.LoadCollider();
    }

    protected virtual void Rebord()
    {
        this.hp = this.maxHp;
        this.isDead = false;
    }

    public virtual void Add(float add)
    {
        if (this.isDead) return;
        this.hp += add;
        if (this.hp > this.maxHp) this.hp = this.maxHp;
    }

    public virtual void Deduct(float deduct)
    {
        if (this.isDead) return;

        this.hp -= deduct;
        EventManager.EmitEvent(EventConstants.UPDATE_HP_BOSS);

        if (this.hp < 0) this.hp = 0;
        this.CheckIsDead();
    }

    protected virtual bool IsDead()
    {
        return this.hp <= 0;
    }

    protected virtual void CheckIsDead()
    {
        if (!this.IsDead()) return;
        this.isDead = true;
        this.OnDead();
    }

    protected virtual void LoadCollider()
    {
        if (circleCollider2D != null) return;
        this.circleCollider2D = GetComponent<CircleCollider2D>();
        this.circleCollider2D.isTrigger = true;
        //this.circleCollider2D.radius = 0.3f;
    }

    protected abstract void OnDead();
}