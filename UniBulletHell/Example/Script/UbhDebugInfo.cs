using UnityEngine;
using UnityEngine.UI;

public class UbhDebugInfo : UbhMonoBehaviour
{
    private const float INTERVAL_SEC = 1f;

    [SerializeField]
    private Text m_fpsText = null;
    [SerializeField]
    private Text m_bulletNumText = null;

    private UbhBulletManager m_bulletManager;
    private float m_lastUpdateTime;
    private int m_frame = 0;

    private void Start()
    {
        if (Debug.isDebugBuild == false)
        {
            gameObject.SetActive(false);
            return;
        }
        m_lastUpdateTime = Time.realtimeSinceStartup;

        m_bulletManager = UbhBulletManager.instance;
    }

    private void Update()
    {
        m_frame++;
        float time = Time.realtimeSinceStartup - m_lastUpdateTime;

        if (time < INTERVAL_SEC)
        {
            return;
        }

        // Count FPS
        float frameRate = m_frame / time;
        if (m_fpsText != null)
        {
            m_fpsText.text = ((int)frameRate).ToString();
        }
        m_lastUpdateTime = Time.realtimeSinceStartup;
        m_frame = 0;

        // Count Bullet Num
        if (m_bulletManager != null)
        {
            if (m_bulletNumText != null)
            {
                m_bulletNumText.text = m_bulletManager.activeBulletCount.ToString();
            }
        }
    }
}
