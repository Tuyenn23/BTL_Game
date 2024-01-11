using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Ubh random spiral shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Random Spiral Shot")]
public class UbhRandomSpiralShot : UbhBaseShot
{
    [Header("===== RandomSpiralShot Settings =====")]
    // "Set a starting angle of shot. (0 to 360)"
    [Range(0f, 360f), FormerlySerializedAs("_StartAngle")]
    public float m_startAngle = 180f;
    // "Set a shift angle of spiral. (-360 to 360)"
    [Range(-360f, 360f), FormerlySerializedAs("_ShiftAngle")]
    public float m_shiftAngle = 5f;
    // "Set a angle size of random range. (0 to 360)"
    [Range(0f, 360f), FormerlySerializedAs("_RandomRangeSize")]
    public float m_randomRangeSize = 30f;
    // "Set a minimum bullet speed of shot."
    // "BulletSpeed is ignored."
    [FormerlySerializedAs("_RandomSpeedMin")]
    public float m_randomSpeedMin = 1f;
    // "Set a maximum bullet speed of shot."
    // "BulletSpeed is ignored."
    [FormerlySerializedAs("_RandomSpeedMax")]
    public float m_randomSpeedMax = 3f;
    // "Set a minimum delay time between bullet and next bullet. (sec)"
    [FormerlySerializedAs("_RandomDelayMin")]
    public float m_randomDelayMin = 0.01f;
    // "Set a maximum delay time between bullet and next bullet. (sec)"
    [FormerlySerializedAs("_RandomDelayMax")]
    public float m_randomDelayMax = 0.1f;

    private int m_nowIndex;
    private float m_delayTimer;

    public override void Shot()
    {
        if (m_bulletNum <= 0 || m_randomSpeedMin <= 0f || m_randomSpeedMax <= 0)
        {
            UbhDebugLog.LogWarning(name + " Cannot shot because BulletNum or RandomSpeedMin or RandomSpeedMax is not set.", this);
            return;
        }

        if (m_shooting)
        {
            return;
        }

        m_shooting = true;
        m_nowIndex = 0;
        m_delayTimer = 0f;
    }

    protected virtual void Update()
    {
        if (m_shooting == false)
        {
            return;
        }

        if (m_delayTimer >= 0f)
        {
            m_delayTimer -= UbhTimer.instance.deltaTime;
            if (m_delayTimer >= 0f)
            {
                return;
            }
        }

        UbhBullet bullet = GetBullet(transform.position);
        if (bullet == null)
        {
            FinishedShot();
            return;
        }

        float bulletSpeed = Random.Range(m_randomSpeedMin, m_randomSpeedMax);

        float centerAngle = m_startAngle + (m_shiftAngle * m_nowIndex);
        float minAngle = centerAngle - (m_randomRangeSize / 2f);
        float maxAngle = centerAngle + (m_randomRangeSize / 2f);
        float angle = Random.Range(minAngle, maxAngle);

        ShotBullet(bullet, bulletSpeed, angle);
        FiredShot();

        m_nowIndex++;

        if (m_nowIndex >= m_bulletNum)
        {
            FinishedShot();
        }
        else
        {
            m_delayTimer = Random.Range(m_randomDelayMin, m_randomDelayMax);
            if (m_delayTimer <= 0f)
            {
                Update();
            }
        }
    }
}