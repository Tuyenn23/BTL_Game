using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Ubh over take nway shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Over Take nWay Shot")]
public class UbhOverTakeNwayShot : UbhBaseShot
{
    [Header("===== OverTakeNwayShot Settings =====")]
    // "Set a number of shot way."
    [FormerlySerializedAs("_WayNum")]
    public int m_wayNum = 8;
    // "Set a center angle of shot. (0 to 360)"
    [Range(0f, 360f), FormerlySerializedAs("_CenterAngle")]
    public float m_centerAngle = 180f;
    // "Set a angle between bullet and next bullet. (0 to 360)"
    [Range(0f, 360f), FormerlySerializedAs("_BetweenAngle")]
    public float m_betweenAngle = 10f;
    // "Set a difference speed between shot and next line shot."
    [FormerlySerializedAs("_DiffSpeed")]
    public float m_diffSpeed = 0.5f;
    // "Set a shift angle between shot and next line shot. (-360 to 360)"
    [Range(-360f, 360f), FormerlySerializedAs("_ShiftAngle")]
    public float m_shiftAngle = 0f;
    // "Set a delay time between shot and next line shot. (sec)"
    [FormerlySerializedAs("_NextLineDelay")]
    public float m_nextLineDelay = 0.1f;

    private int m_nowIndex;
    private float m_delayTimer;

    private float m_nowBulletSpeed;
    private float m_nowShiftAngle;

    public override void Shot()
    {
        if (m_bulletNum <= 0 || m_bulletSpeed <= 0f || m_wayNum <= 0)
        {
            UbhDebugLog.LogWarning(name + " Cannot shot because BulletNum or BulletSpeed or WayNum is not set.", this);
            return;
        }

        if (m_shooting)
        {
            return;
        }

        m_shooting = true;
        m_nowIndex = 0;
        m_delayTimer = 0f;

        m_nowBulletSpeed = m_bulletSpeed;
        m_nowShiftAngle = 0f;
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

        for (int i = 0; i < m_wayNum; i++)
        {
            UbhBullet bullet = GetBullet(transform.position);
            if (bullet == null)
            {
                break;
            }

            float baseAngle = m_wayNum % 2 == 0 ? m_centerAngle - (m_betweenAngle / 2f) : m_centerAngle;

            float angle = UbhUtil.GetShiftedAngle(i, baseAngle, m_betweenAngle) + m_nowShiftAngle;

            ShotBullet(bullet, m_nowBulletSpeed, angle);

            m_nowIndex++;
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
            m_nowBulletSpeed += m_diffSpeed;
            m_nowShiftAngle += m_shiftAngle;

            m_delayTimer = m_nextLineDelay;
            if (m_delayTimer <= 0f)
            {
                Update();
            }
        }
    }
}