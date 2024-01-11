using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Ubh waving nway lock on shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Waving nWay Shot (Lock On)")]
public class UbhWavingNwayLockOnShot : UbhWavingNwayShot
{
    [Header("===== WavingNwayLockOnShot Settings =====")]
    // "Set a target with tag name."
    [FormerlySerializedAs("_SetTargetFromTag")]
    public bool m_setTargetFromTag = true;
    // "Set a unique tag name of target at using SetTargetFromTag."
    [FormerlySerializedAs("_TargetTagName"), UbhConditionalHide("m_setTargetFromTag")]
    public string m_targetTagName = "Player";
    // "Flag to select random from GameObjects of the same tag."
    [UbhConditionalHide("m_setTargetFromTag")]
    public bool m_randomSelectTagTarget;
    // "Flag to select nearest from GameObjects of the same tag."
    [UbhConditionalHide("m_setTargetFromTag")]
    public bool m_nearestSelectTagTarget;
    // "Transform of lock on target."
    // "It is not necessary if you want to specify target in tag."
    // "Overwrite WaveCenterAngle in direction of target to Transform.position."
    [FormerlySerializedAs("_TargetTransform")]
    public Transform m_targetTransform;
    // "Always aim to target."
    [FormerlySerializedAs("_Aiming")]
    public bool m_aiming;

    /// <summary>
    /// is lock on shot flag.
    /// </summary>
    public override bool lockOnShot { get { return true; } }

    public override void Shot()
    {
        AimTarget();
        base.Shot();
    }

    protected override void Update()
    {
        if (m_shooting && m_aiming)
        {
            AimTarget();
        }

        base.Update();
    }

    public override void FinishedShot()
    {
        if (m_setTargetFromTag)
        {
            m_targetTransform = null;
        }
        base.FinishedShot();
    }

    private void AimTarget()
    {
        if (m_targetTransform == null && m_setTargetFromTag)
        {
            m_targetTransform = UbhUtil.GetTransformFromTagName(m_targetTagName, m_randomSelectTagTarget, m_nearestSelectTagTarget, transform);
        }
        if (m_targetTransform != null)
        {
            m_waveCenterAngle = UbhUtil.GetAngleFromTwoPosition(transform, m_targetTransform, shotCtrl.m_axisMove);
        }
    }
}