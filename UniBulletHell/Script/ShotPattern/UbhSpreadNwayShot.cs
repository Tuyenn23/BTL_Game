using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Ubh spread nway shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Spread nWay Shot")]
public class UbhSpreadNwayShot : UbhBaseShot
{
    [Header("===== SpreadNwayShot Settings =====")]
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
    }

    private void Update()
    {
        if (m_shooting == false)
        {
            return;
        }

        int wayIndex = 0;

        float bulletSpeed = m_bulletSpeed;

        for (int i = 0; i < m_bulletNum; i++)
        {
            if (m_wayNum <= wayIndex)
            {
                wayIndex = 0;

                bulletSpeed -= m_diffSpeed;
                while (bulletSpeed <= 0)
                {
                    bulletSpeed += Mathf.Abs(m_diffSpeed);
                }
            }

            UbhBullet bullet = GetBullet(transform.position);
            if (bullet == null)
            {
                break;
            }

            float baseAngle = m_wayNum % 2 == 0 ? m_centerAngle - (m_betweenAngle / 2f) : m_centerAngle;

            float angle = UbhUtil.GetShiftedAngle(wayIndex, baseAngle, m_betweenAngle);

            ShotBullet(bullet, bulletSpeed, angle);

            wayIndex++;
        }

        FiredShot();

        FinishedShot();
    }
}