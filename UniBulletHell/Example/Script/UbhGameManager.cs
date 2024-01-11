using UnityEngine;
using UnityEngine.Serialization;

public class UbhGameManager : UbhMonoBehaviour
{
    public const int BASE_SCREEN_WIDTH = 600;
    public const int BASE_SCREEN_HEIGHT = 450;

    [FormerlySerializedAs("_ScaleToFit")]
    public bool m_scaleToFit = false;

    [SerializeField, FormerlySerializedAs("_PlayerPrefab")]
    private GameObject m_playerPrefab = null;
    [SerializeField, FormerlySerializedAs("_GoTitle")]
    private GameObject m_goTitle = null;
    [SerializeField, FormerlySerializedAs("_GoLetterBox")]
    private GameObject m_goLetterBox = null;
    [SerializeField, FormerlySerializedAs("_Score")]
    private UbhScore m_score = null;

    private void Start()
    {
        m_goLetterBox.SetActive(m_scaleToFit == false);
    }

    private void Update()
    {
        if (IsPlaying())
        {
            return;
        }

        if (UbhUtil.IsMobilePlatform())
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameStart();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                GameStart();
            }
        }
    }

    private void GameStart()
    {
        if (m_score != null)
        {
            m_score.Initialize();
        }

        if (m_goTitle != null)
        {
            m_goTitle.SetActive(false);
        }

        CreatePlayer();
    }

    public void GameOver()
    {
        if (m_score != null)
        {
            m_score.Save();
        }

        if (m_goTitle != null)
        {
            m_goTitle.SetActive(true);
        }
        else
        {
            // for UBH_ShotShowcase scene.
            CreatePlayer();
        }
    }

    private void CreatePlayer()
    {
        UbhPlayer player = FindObjectOfType<UbhPlayer>();
        if (player != null &&
            player.isDead == false)
        {
            return;
        }

        Instantiate(m_playerPrefab, m_playerPrefab.transform.position, m_playerPrefab.transform.rotation);
    }

    public bool IsPlaying()
    {
        if (m_goTitle != null)
        {
            return m_goTitle.activeSelf == false;
        }
        else
        {
            // for UBH_ShotShowcase scene.
            return true;
        }
    }
}
