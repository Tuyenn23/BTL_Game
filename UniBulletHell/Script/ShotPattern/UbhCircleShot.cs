using UnityEngine;

/// <summary>
/// Ubh circle shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Circle Shot")]
public class UbhCircleShot : UbhBaseShot
{
    public override void Shot()
    {
        if (m_bulletNum <= 0 || m_bulletSpeed <= 0f)
        {
            UbhDebugLog.LogWarning(name + " Cannot shot because BulletNum or BulletSpeed is not set.", this);
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

        float shiftAngle = 360f / (float)m_bulletNum;

        for (int i = 0; i < m_bulletNum; i++)
        {
            UbhBullet bullet = GetBullet(transform.position);
            if (bullet == null)
            {
                break;
            }

            float angle = shiftAngle * i;

            ShotBullet(bullet, m_bulletSpeed, angle);
        }

        FiredShot();

        FinishedShot();
    }
}