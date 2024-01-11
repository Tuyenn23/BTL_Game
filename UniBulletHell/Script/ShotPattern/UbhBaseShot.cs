using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// Ubh base shot.
/// Each shot pattern classes inherit this class.
/// </summary>
public abstract class UbhBaseShot : UbhMonoBehaviour
{
    [Header("===== Common Settings =====")]
    // "Set a bullet prefab for the shot. (ex. sprite or model)"
    [FormerlySerializedAs("_BulletPrefab")]
    public GameObject m_bulletPrefab;
    // "Set a bullet number of shot."
    [FormerlySerializedAs("_BulletNum")]
    public int m_bulletNum = 10;
    // "Set a bullet base speed of shot."
    [FormerlySerializedAs("_BulletSpeed")]
    public float m_bulletSpeed = 2f;
    // "Set an acceleration of bullet speed."
    [FormerlySerializedAs("_AccelerationSpeed")]
    public float m_accelerationSpeed = 0f;
    // "Use max speed flag."
    public bool m_useMaxSpeed = false;
    // "Set a bullet max speed of shot."
    [UbhConditionalHide("m_useMaxSpeed")]
    public float m_maxSpeed = 0f;
    // "Use min speed flag"
    public bool m_useMinSpeed = false;
    // "Set a bullet min speed of shot."
    [UbhConditionalHide("m_useMinSpeed")]
    public float m_minSpeed = 0f;
    // "Set an acceleration of bullet turning."
    [FormerlySerializedAs("_AccelerationTurn")]
    public float m_accelerationTurn = 0f;
    // "This flag is pause and resume bullet at specified time."
    [FormerlySerializedAs("_UsePauseAndResume")]
    public bool m_usePauseAndResume = false;
    // "Set a time to pause bullet."
    [FormerlySerializedAs("_PauseTime"), UbhConditionalHide("m_usePauseAndResume")]
    public float m_pauseTime = 0f;
    // "Set a time to resume bullet."
    [FormerlySerializedAs("_ResumeTime"), UbhConditionalHide("m_usePauseAndResume")]
    public float m_resumeTime = 0f;
    // "This flag is automatically release the bullet GameObject at the specified time."
    [FormerlySerializedAs("_UseAutoRelease")]
    public bool m_useAutoRelease = false;
    // "Set a time to automatically release after the shot at using UseAutoRelease. (sec)"
    [FormerlySerializedAs("_AutoReleaseTime"), UbhConditionalHide("m_useAutoRelease")]
    public float m_autoReleaseTime = 10f;

    [Space(10)]

    // "Set a callback method fired shot."
    public UnityEvent m_shotFiredCallbackEvents = new UnityEvent();
    // "Set a callback method after shot."
    public UnityEvent m_shotFinishedCallbackEvents = new UnityEvent();

    protected bool m_shooting;

    private UbhShotCtrl m_shotCtrl;

    public UbhShotCtrl shotCtrl
    {
        get
        {
            if (m_shotCtrl == null)
            {
                m_shotCtrl = GetComponentInParent<UbhShotCtrl>();
            }
            return m_shotCtrl;
        }
    }

    /// <summary>
    /// is shooting flag.
    /// </summary>
    public bool shooting { get { return m_shooting; } }

    /// <summary>
    /// is lock on shot flag.
    /// </summary>
    public virtual bool lockOnShot { get { return false; } }

    /// <summary>
    /// Call from override OnDisable method in inheriting classes.
    /// Example : protected override void OnDisable () { base.OnDisable (); }
    /// </summary>
    protected virtual void OnDisable()
    {
        m_shooting = false;
    }

    /// <summary>
    /// Abstract shot method.
    /// </summary>
    public abstract void Shot();

    /// <summary>
    /// UbhShotCtrl setter.
    /// </summary>
    public void SetShotCtrl(UbhShotCtrl shotCtrl)
    {
        m_shotCtrl = shotCtrl;
    }

    /// <summary>
    /// Fired shot.
    /// </summary>
    protected virtual void FiredShot()
    {
        m_shotFiredCallbackEvents.Invoke();
    }

    /// <summary>
    /// Finished shot.
    /// </summary>
    public virtual void FinishedShot()
    {
        m_shooting = false;
        m_shotFinishedCallbackEvents.Invoke();
    }

    /// <summary>
    /// Get UbhBullet object from object pool.
    /// </summary>
    protected UbhBullet GetBullet(Vector3 position, bool forceInstantiate = false)
    {
        if (m_bulletPrefab == null)
        {
            UbhDebugLog.LogWarning(name + " Cannot generate a bullet because BulletPrefab is not set.", this);
            return null;
        }

        return UbhObjectPool.instance.GetBullet(m_bulletPrefab, position, forceInstantiate);
    }

    /// <summary>
    /// Shot UbhBullet object.
    /// </summary>
    protected void ShotBullet(UbhBullet bullet, float speed, float angle,
                               bool homing = false, Transform homingTarget = null, float homingAngleSpeed = 0f,
                               bool sinWave = false, float sinWaveSpeed = 0f, float sinWaveRangeSize = 0f, bool sinWaveInverse = false)
    {
        if (bullet == null)
        {
            return;
        }
        bullet.Shot(this,
                    speed, angle, m_accelerationSpeed, m_accelerationTurn,
                    homing, homingTarget, homingAngleSpeed,
                    sinWave, sinWaveSpeed, sinWaveRangeSize, sinWaveInverse,
                    m_usePauseAndResume, m_pauseTime, m_resumeTime,
                    m_useAutoRelease, m_autoReleaseTime,
                    m_shotCtrl.m_axisMove, m_shotCtrl.m_inheritAngle,
                    m_useMaxSpeed, m_maxSpeed, m_useMinSpeed, m_minSpeed);
    }
}