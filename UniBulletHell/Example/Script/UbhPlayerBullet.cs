using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class UbhPlayerBullet : UbhBulletSimpleSprite2d
{
    [FormerlySerializedAs("_Power")]
    public int m_power = 1;
}