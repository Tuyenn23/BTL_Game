using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : DamageSender
{
    [SerializeField]
    protected float speed = 6;
    protected abstract void MoveBullet();

    

}
