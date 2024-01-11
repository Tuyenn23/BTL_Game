using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Ubh spiral multi nway shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Spiral Multi nWay Shot")]
public class UbhSpiralMultiNwayShot : UbhBaseShot
{
    [Header("===== SpiralMultiNwayShot Settings =====")]
    // "Set a number of shot spiral way."
    [FormerlySerializedAs("_SpiralWayNum")]
    public int m_spiralWayNum = 4;
    // "Set a number of shot way."
    [FormerlySerializedAs("_WayNum")]
    public int m_wayNum = 5;
    // "Set a starting angle of shot. (0 to 360)"
    [Range(0f, 360f), FormerlySerializedAs("_StartAngle")]
    public float m_startAngle = 180f;
    // "Set a shift angle of spiral. (-360 to 360)"
    [Range(-360f, 360f), FormerlySerializedAs("_ShiftAngle")]
    public float m_shiftAngle = 5f;
    // "Set a angle between bullet and next bullet. (0 to 360)"
    [Range(0f, 360f), FormerlySerializedAs("_BetweenAngle")]
    public float m_betweenAngle = 5f;
    // "Set a delay time between shot and next line shot. (sec)"
    [FormerlySerializedAs("_NextLineDelay")]
    public float m_nextLineDelay = 0.1f;

    private int m_nowIndex;
    private float m_delayTimer;

    public override void Shot()
    {
        if (m_bulletNum <= 0 || m_bulletSpeed <= 0f || m_wayNum <= 0 || m_spiralWayNum <= 0)
        {
            UbhDebugLog.LogWarning(name + " Cannot shot because BulletNum or BulletSpeed or WayNum or SpiralWayNum is not set.", this);
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

        float spiralWayShiftAngle = 360f / m_spiralWayNum;

        for (int i = 0; i < m_spiralWayNum; i++)
        {
            for (int k = 0; k < m_wayNum; k++)
            {
                UbhBullet bullet = GetBullet(transform.position);
                if (bullet == null)
                {
                    break;
                }

                float centerAngle = m_startAngle + (spiralWayShiftAngle * i) + (m_shiftAngle * Mathf.Floor(m_nowIndex / m_wayNum));

                float baseAngle = m_wayNum % 2 == 0 ? centerAngle - (m_betweenAngle / 2f) : centerAngle;

                float angle = UbhUtil.GetShiftedAngle(k, baseAngle, m_betweenAngle);

                ShotBullet(bullet, m_bulletSpeed, angle);

                m_nowIndex++;
                if (m_nowIndex >= m_bulletNum)
                {
                    break;
                }
            }

            if (m_nowIndex >= m_bulletNum)
            {
                break;
            }
        }

        FiredShot();

        if (m_nowIndex >= m_bulletNum)
        {
            FinishedShot();
        }
        else
        {
            m_delayTimer = m_nextLineDelay;
            if (m_delayTimer <= 0f)
            {
                Update();
            }
        }
    }
}