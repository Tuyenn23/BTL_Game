using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// Ubh shot ctrl.
/// </summary>
[AddComponentMenu("UniBulletHell/Controller/Shot Controller")]
public sealed class UbhShotCtrl : UbhMonoBehaviour
{
    private enum UpdateStep
    {
        StartDelay,
        StartShot,
        WaitDelay,
        UpdateIndex,
        FinishShot,
    }

    [Serializable]
    public class ShotInfo
    {
        // "Set a shot pattern component (inherits UbhBaseShot)."
        [FormerlySerializedAs("_ShotObj")]
        public UbhBaseShot m_shotObj;
        // "Set a delay time to starting next shot pattern. (sec)"
        [FormerlySerializedAs("_AfterDelay")]
        public float m_afterDelay;
    }

    // "Axis on bullet move."
    [FormerlySerializedAs("_AxisMove")]
    public UbhUtil.AXIS m_axisMove = UbhUtil.AXIS.X_AND_Y;
    // "Flag that inherits angle of UbhShotCtrl."
    public bool m_inheritAngle = false;
    // "This flag starts a shot routine at same time as instantiate."
    [FormerlySerializedAs("_StartOnAwake")]
    public bool m_startOnAwake = true;
    // "Set a delay time at using Start On Awake. (sec)"
    [FormerlySerializedAs("_StartOnAwakeDelay"), UbhConditionalHide("m_startOnAwake")]
    public float m_startOnAwakeDelay = 1f;
    // "This flag starts a shot routine at same time as enabled."
    public bool m_startOnEnable = false;
    // "Set a delay time at using Start On Enable. (sec)"
    [UbhConditionalHide("m_startOnEnable")]
    public float m_startOnEnableDelay = 1f;
    // "Flag that repeats a shot routine."
    [FormerlySerializedAs("_Loop")]
    public bool m_loop = true;
    // "Flag that makes a shot routine randomly."
    [FormerlySerializedAs("_AtRandom")]
    public bool m_atRandom = false;
    // "List of shot information. this size is necessary at least 1 or more."
    [FormerlySerializedAs("_ShotList")]
    public List<ShotInfo> m_shotList = new List<ShotInfo>();

    [Space(10)]

    // "Set a callback method after shot routine."
    public UnityEvent m_shotRoutineFinishedCallbackEvents = new UnityEvent();

    private bool m_shooting;
    private UpdateStep m_updateStep;
    private int m_nowIndex;
    private float m_delayTimer;
    private List<ShotInfo> m_randomShotList = new List<ShotInfo>(32);

    /// <summary>
    /// is shooting flag.
    /// </summary>
    public bool shooting { get { return m_shooting; } }

    private void Start()
    {
        if (m_startOnAwake)
        {
            StartShotRoutine(m_startOnAwakeDelay);
        }
    }

    private void OnEnable()
    {
        UbhShotManager.instance.AddShot(this);

        if (m_startOnEnable)
        {
            StartShotRoutine(m_startOnEnableDelay);
        }
    }

    private void OnDisable()
    {
        m_shooting = false;

        UbhShotManager shotMgr = UbhShotManager.instance;
        if (shotMgr != null)
        {
            shotMgr.RemoveShot(this);
        }
    }

    public void UpdateShot(float deltaTime)
    {
        if (m_shooting == false)
        {
            return;
        }

        if (m_updateStep == UpdateStep.StartDelay)
        {
            if (m_delayTimer > 0f)
            {
                m_delayTimer -= deltaTime;
                return;
            }
            else
            {
                m_delayTimer = 0f;
                m_updateStep = UpdateStep.StartShot;
            }
        }

        ShotInfo nowShotInfo = m_atRandom ? m_randomShotList[m_nowIndex] : m_shotList[m_nowIndex];

        if (m_updateStep == UpdateStep.StartShot)
        {
            if (nowShotInfo.m_shotObj != null)
            {
                nowShotInfo.m_shotObj.SetShotCtrl(this);
                nowShotInfo.m_shotObj.Shot();
            }

            m_delayTimer = 0f;
            m_updateStep = UpdateStep.WaitDelay;
        }

        if (m_updateStep == UpdateStep.WaitDelay)
        {
            if (nowShotInfo.m_afterDelay > 0 && nowShotInfo.m_afterDelay > m_delayTimer)
            {
                m_delayTimer += deltaTime;
            }
            else
            {
                m_delayTimer = 0f;
                m_updateStep = UpdateStep.UpdateIndex;
            }
        }

        if (m_updateStep == UpdateStep.UpdateIndex)
        {
            if (m_atRandom)
            {
                m_randomShotList.RemoveAt(m_nowIndex);

                if (m_loop && m_randomShotList.Count <= 0)
                {
                    m_randomShotList.AddRange(m_shotList);
                }

                if (m_randomShotList.Count > 0)
                {
                    m_nowIndex = UnityEngine.Random.Range(0, m_randomShotList.Count);
                    m_updateStep = UpdateStep.StartShot;
                }
                else
                {
                    m_updateStep = UpdateStep.FinishShot;
                }
            }
            else
            {
                if (m_loop || m_nowIndex < m_shotList.Count - 1)
                {
                    m_nowIndex = (int)Mathf.Repeat(m_nowIndex + 1f, m_shotList.Count);
                    m_updateStep = UpdateStep.StartShot;
                }
                else
                {
                    m_updateStep = UpdateStep.FinishShot;
                }
            }
        }

        if (m_updateStep == UpdateStep.StartShot)
        {
            UpdateShot(deltaTime);
        }
        else if (m_updateStep == UpdateStep.FinishShot)
        {
            m_shooting = false;
            m_shotRoutineFinishedCallbackEvents.Invoke();
        }
    }

    /// <summary>
    /// Start the shot routine.
    /// </summary>
    public void StartShotRoutine(float startDelay = 0f)
    {
        if (m_shotList == null || m_shotList.Count <= 0)
        {
            UbhDebugLog.LogWarning(name + " Cannot shot because ShotList is null or empty.", this);
            return;
        }

        bool enableShot = false;
        for (int i = 0; i < m_shotList.Count; i++)
        {
            if (m_shotList[i].m_shotObj != null)
            {
                enableShot = true;
                break;
            }
        }
        if (enableShot == false)
        {
            UbhDebugLog.LogWarning(name + " Cannot shot because all ShotObj of ShotList is not set.", this);
            return;
        }

        if (m_loop)
        {
            bool enableDelay = false;
            for (int i = 0; i < m_shotList.Count; i++)
            {
                if (0f < m_shotList[i].m_afterDelay)
                {
                    enableDelay = true;
                    break;
                }
            }
            if (enableDelay == false)
            {
                UbhDebugLog.LogWarning(name + " Cannot shot because loop is true and all AfterDelay of ShotList is zero.", this);
                return;
            }
        }

        if (m_shooting)
        {
            UbhDebugLog.LogWarning(name + " Already shooting.", this);
            return;
        }

        m_shooting = true;
        m_delayTimer = startDelay;
        m_updateStep = m_delayTimer > 0f ? UpdateStep.StartDelay : UpdateStep.StartShot;
        if (m_atRandom)
        {
            m_randomShotList.Clear();
            m_randomShotList.AddRange(m_shotList);
            m_nowIndex = UnityEngine.Random.Range(0, m_randomShotList.Count);
        }
        else
        {
            m_nowIndex = 0;
        }
    }

    /// <summary>
    /// Stop the shot routine.
    /// </summary>
    public void StopShotRoutine()
    {
        m_shooting = false;
    }

    /// <summary>
    /// Stop the shot routine and playing shot.
    /// </summary>
    public void StopShotRoutineAndPlayingShot()
    {
        m_shooting = false;

        if (m_shotList == null || m_shotList.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < m_shotList.Count; i++)
        {
            if (m_shotList[i].m_shotObj != null)
            {
                m_shotList[i].m_shotObj.FinishedShot();
            }
        }
    }
}