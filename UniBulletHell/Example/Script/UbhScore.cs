using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UbhScore : UbhMonoBehaviour
{
    private const string HIGH_SCORE_KEY = "highScoreKey";
    private const string HIGH_SCORE_TITLE = "HighScore : ";

    [SerializeField, FormerlySerializedAs("_DeleteScore")]
    private bool m_deleteScore = false;
    [SerializeField]
    private Text m_scoreText = null;
    [SerializeField]
    private Text m_highScoreText = null;

    private int m_score;
    private int m_highScore;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (m_highScore < m_score)
        {
            m_highScore = m_score;
        }

        m_scoreText.text = m_score.ToString();
        m_highScoreText.text = HIGH_SCORE_TITLE + m_highScore.ToString();
    }

    public void Initialize()
    {
        if (m_deleteScore)
        {
            PlayerPrefs.DeleteAll();
        }
        m_score = 0;
        m_highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    public void AddPoint(int point)
    {
        m_score = m_score + point;
    }

    public void Save()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, m_highScore);
        PlayerPrefs.Save();

        Initialize();
    }
}
