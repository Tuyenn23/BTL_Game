using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager of UbhShotCtrl
/// </summary>
[DisallowMultipleComponent]
public sealed class UbhShotManager : UbhSingletonMonoBehavior<UbhShotManager>
{
    private List<UbhShotCtrl> m_shotList = new List<UbhShotCtrl>(2048);
    private HashSet<UbhShotCtrl> m_shotHashSet = new HashSet<UbhShotCtrl>();

    public int activeShotCount { get { return m_shotList.Count; } }

    protected override void DoAwake()
    {
        // Create UbhTimer
        if (UbhTimer.instance == null) { }
    }

    /// <summary>
    /// Update Shots
    /// </summary>
    public void UpdateShots(float deltaTime)
    {
        for (int i = m_shotList.Count - 1; i >= 0; i--)
        {
            UbhShotCtrl shotCtrl = m_shotList[i];
            if (shotCtrl == null)
            {
                m_shotList.Remove(shotCtrl);
                continue;
            }
            shotCtrl.UpdateShot(deltaTime);
        }
    }

    /// <summary>
    /// Add shot
    /// </summary>
    public void AddShot(UbhShotCtrl shotCtrl)
    {
        if (m_shotHashSet.Contains(shotCtrl))
        {
            return;
        }
        m_shotList.Add(shotCtrl);
        m_shotHashSet.Add(shotCtrl);
    }

    /// <summary>
    /// Remove shot
    /// </summary>
    public void RemoveShot(UbhShotCtrl shotCtrl)
    {
        if (m_shotHashSet.Contains(shotCtrl) == false)
        {
            return;
        }
        m_shotList.Remove(shotCtrl);
        m_shotHashSet.Remove(shotCtrl);
    }
}
