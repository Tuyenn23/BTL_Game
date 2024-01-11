using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Ubh paint lock on shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Paint Shot (Lock On)")]
public class UbhPaintLockOnShot : UbhPaintShot
{
    [Header("===== PaintLockOnShot Settings =====")]
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
    // "Overwrite PaintCenterAngle in direction of target to Transform.position."
    [FormerlySerializedAs("_TargetTransform")]
    public Transform m_targetTransform;

    /// <summary>
    /// is lock on shot flag.
    /// </summary>
    public override bool lockOnShot { get { return true; } }

    public override void Shot()
    {
        AimTarget();
        base.Shot();
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
            m_paintCenterAngle = UbhUtil.GetAngleFromTwoPosition(transform, m_targetTransform, shotCtrl.m_axisMove);
        }
    }
}