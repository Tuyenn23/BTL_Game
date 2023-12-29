using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ChangeSkillBoss : MonoBehaviour
{

    [SerializeField, FormerlySerializedAs("_GoShotCtrlList")]
    private GameObject[] m_goShotCtrlList = null;

    private int m_nowIndex = 0;
    private int lenghSkill;

#if UNITY_EDITOR
    [Button("Change Skill")]
    public void LoadSkill()
    {
        m_goShotCtrlList[m_nowIndex].SetActive(false);
        ChangeShot(true);
    }
#endif

    private void Start()
    {
        if (m_goShotCtrlList != null)
        {
            for (int i = 0; i < m_goShotCtrlList.Length; i++)
            {
                m_goShotCtrlList[i].SetActive(false);
            }
        }
        lenghSkill = m_goShotCtrlList.Length + 1;

        InvokeRepeating("ChangeSkillRuntime", 1f, 5f);
    }
    
    private void ChangeSkillRuntime()
    {
        m_goShotCtrlList[m_nowIndex].SetActive(false);

        if (m_nowIndex != 1)
        {
            m_goShotCtrlList[m_nowIndex].SetActive(false);
        }

        m_nowIndex += 1;
        ChangeShot(true);
        if (m_nowIndex >= lenghSkill)
        {
            m_goShotCtrlList[1].SetActive(false);
            m_nowIndex = 1;
            ChangeShot(true);
        }
    }

    public void ChangeShot(bool toNext)
    {
        if (m_goShotCtrlList == null)
        {
            return;
        }

        //StopAllCoroutines();

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
