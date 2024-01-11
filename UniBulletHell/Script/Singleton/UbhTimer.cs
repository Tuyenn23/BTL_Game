using UnityEngine;

/// <summary>
/// Ubh timer.
/// </summary>
[DisallowMultipleComponent]
public sealed class UbhTimer : UbhSingletonMonoBehavior<UbhTimer>
{
    // 60fps's FixedDeltaTime
    private const float FIXED_DELTA_TIME_BASE = (1f / 60f);

    [SerializeField]
    private UbhUtil.TIME m_deltaTimeType = UbhUtil.TIME.DELTA_TIME;

    private float m_deltaTime;
    private float m_deltaTimeUnscaled;
    private float m_deltaTimeFixed;

    private float m_deltaFrameCount;
    private float m_deltaFrameCountUnscaled;
    private float m_deltaFrameCountFixed;

    private float m_totalFrameCount;
    private float m_totalFrameCountUnscaled;
    private float m_totalFrameCountFixed;

    private bool m_pausing;

    /// <summary>
    /// Time type
    /// </summary>
    public UbhUtil.TIME deltaTimeType { get { return m_deltaTimeType; } set { m_deltaTimeType = value; } }

    /// <summary>
    /// Get pause flag
    /// </summary>
    public bool pausing { get { return m_pausing; } }

    /// <summary>
    /// Get delta time by time type.
    /// </summary>
    public float deltaTime
    {
        get
        {
            if (m_pausing)
            {
                return 0f;
            }

            switch (m_deltaTimeType)
            {
                case UbhUtil.TIME.UNSCALED_DELTA_TIME:
                    return m_deltaTimeUnscaled;

                case UbhUtil.TIME.FIXED_DELTA_TIME:
                    return m_deltaTimeFixed;

                case UbhUtil.TIME.DELTA_TIME:
                default:
                    return m_deltaTime;
            }
        }
    }

    /// <summary>
    /// Get delta frame count by time type.
    /// </summary>
    public float deltaFrameCount
    {
        get
        {
            if (m_pausing)
            {
                return 0f;
            }

            switch (m_deltaTimeType)
            {
                case UbhUtil.TIME.UNSCALED_DELTA_TIME:
                    return m_deltaFrameCountUnscaled;

                case UbhUtil.TIME.FIXED_DELTA_TIME:
                    return m_deltaFrameCountFixed;

                case UbhUtil.TIME.DELTA_TIME:
                default:
                    return m_deltaFrameCount;
            }
        }
    }

    /// <summary>
    /// Get total frame count by time type.
    /// </summary>
    public float totalFrameCount
    {
        get
        {
            switch (m_deltaTimeType)
            {
                case UbhUtil.TIME.UNSCALED_DELTA_TIME:
                    return m_totalFrameCountUnscaled;

                case UbhUtil.TIME.FIXED_DELTA_TIME:
                    return m_totalFrameCountFixed;

                case UbhUtil.TIME.DELTA_TIME:
                default:
                    return m_totalFrameCount;
            }
        }
    }

    protected override void DoAwake()
    {
        UpdateTimes();
    }

    private void Update()
    {
        UpdateTimes();
        UbhBulletManager.instance.UpdateBullets(deltaTime);
        UbhShotManager.instance.UpdateShots(deltaTime);

#if UNITY_EDITOR
        // Runtime Scene Change Test
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.Z))
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "UBH_ShotShowcase")
            {
                UbhObjectPool.instance.ReleaseAllBullet();
                UbhObjectPool.instance.RemoveAllPool();
                UnityEngine.SceneManagement.SceneManager.LoadScene("UBH_GameExample");
            }
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "UBH_GameExample")
            {
                UbhObjectPool.instance.ReleaseAllBullet();
                UbhObjectPool.instance.RemoveAllPool();
                UnityEngine.SceneManagement.SceneManager.LoadScene("UBH_ShotShowcase");
            }
        }
#endif
    }

    private void UpdateTimes()
    {
        m_deltaTime = Time.deltaTime;
        m_deltaTimeUnscaled = Time.unscaledDeltaTime;

        float nowFps = 0;
        int vSyncCount = QualitySettings.vSyncCount;
        if (vSyncCount == 1)
        {
            nowFps = Screen.currentResolution.refreshRate;
        }
        else if (vSyncCount == 2)
        {
            nowFps = Screen.currentResolution.refreshRate / 2f;
        }
        else
        {
            nowFps = Application.targetFrameRate;
        }

        if (nowFps > 0)
        {
            m_deltaTimeFixed = FIXED_DELTA_TIME_BASE * (60 / nowFps);
        }
        else
        {
            m_deltaTimeFixed = 0;
        }

        m_deltaFrameCount = m_deltaTime / FIXED_DELTA_TIME_BASE;
        m_deltaFrameCountUnscaled = m_deltaTimeUnscaled / FIXED_DELTA_TIME_BASE;
        m_deltaFrameCountFixed = m_deltaTimeFixed / FIXED_DELTA_TIME_BASE;

        if (m_pausing == false)
        {
            m_totalFrameCount += m_deltaFrameCount;
            m_totalFrameCountUnscaled += m_deltaFrameCountUnscaled;
            m_totalFrameCountFixed += m_deltaFrameCountFixed;
        }
    }

    /// <summary>
    /// Pause time of UniBulletHell.
    /// </summary>
    public void Pause()
    {
        m_pausing = true;
    }

    /// <summary>
    /// Resume time of UniBulletHell.
    /// </summary>
    public void Resume()
    {
        m_pausing = false;
    }
}