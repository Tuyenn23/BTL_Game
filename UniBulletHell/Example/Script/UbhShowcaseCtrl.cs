using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UbhShowcaseCtrl : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("_GoShotCtrlList")]
    private GameObject[] m_goShotCtrlList = null;

    [SerializeField]
    private Text m_shotNameText = null;

    private int m_nowIndex = 0;
    private string m_nowGoName;

    private void Start()
    {
        if (m_goShotCtrlList != null)
        {
            for (int i = 0; i < m_goShotCtrlList.Length; i++)
            {
                m_goShotCtrlList[i].SetActive(false);
            }
        }

        m_nowIndex = -1;
        ChangeShot(true);
    }

    public void ChangeShot(bool toNext)
    {
        if (m_goShotCtrlList == null)
        {
            return;
        }

        StopAllCoroutines();

        if (0 <= m_nowIndex && m_nowIndex < m_goShotCtrlList.Length)
        {
            m_goShotCtrlList[m_nowIndex].SetActive(false);
        }

        if (toNext)
        {
            m_nowIndex = (int)Mathf.Repeat(m_nowIndex + 1f, m_goShotCtrlList.Length);
        }
        else
        {
            m_nowIndex--;
            if (m_nowIndex < 0)
            {
                m_nowIndex = m_goShotCtrlList.Length - 1;
            }
        }

        if (0 <= m_nowIndex && m_nowIndex < m_goShotCtrlList.Length)
        {
            m_goShotCtrlList[m_nowIndex].SetActive(true);

            m_nowGoName = m_goShotCtrlList[m_nowIndex].name;

            m_shotNameText.text = "No." + (m_nowIndex + 1).ToString() + " : " + m_nowGoName;

            StartCoroutine(StartShot());
        }
    }

    private IEnumerator StartShot()
    {
        float cntTimer = 0f;
        while (cntTimer < 1f)
        {
            cntTimer += UbhTimer.instance.deltaTime;
            yield return null;
        }

        yield return null;

        UbhShotCtrl shotCtrl = m_goShotCtrlList[m_nowIndex].GetComponent<UbhShotCtrl>();
        if (shotCtrl != null)
        {
            shotCtrl.StartShotRoutine();
        }
    }
}