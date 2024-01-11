using System;
using UnityEngine;

/// <summary>
/// Transform extention.
/// </summary>
public static class UbhTransformExtention
{
    private static Vector3 m_tmpVector3 = UbhUtil.VECTOR3_ZERO;
    private static Vector2 m_tmpVector2 = UbhUtil.VECTOR2_ZERO;

    #region Reset

    public static void ResetTransform(this Transform self, bool worldSpace = false)
    {
        self.ResetPosition(worldSpace);
        self.ResetRotation(worldSpace);
        self.ResetLocalScale();
    }

    public static void ResetPosition(this Transform self, bool worldSpace = false)
    {
        if (worldSpace)
        {
            self.position = UbhUtil.VECTOR3_ZERO;
        }
        else
        {
            self.localPosition = UbhUtil.VECTOR3_ZERO;
        }
    }

    public static void ResetRotation(this Transform self, bool worldSpace = false)
    {
        if (worldSpace)
        {
            self.rotation = Quaternion.identity;
        }
        else
        {
            self.localRotation = Quaternion.identity;
        }
    }

    public static void ResetLocalScale(this Transform self)
    {
        self.localScale = UbhUtil.VECTOR3_ONE;
    }

    #endregion

    #region GetVector2Position

    /// <summary>
    /// XYの位置をVector2型で取得
    /// </summary>
    public static Vector2 GetVector2PositionXY(this Transform self)
    {
        return new Vector2(self.position.x, self.position.y);
    }

    /// <summary>
    /// XYのローカル位置をVector2型で取得
    /// </summary>
    public static Vector2 GetVector2LocalPositionXY(this Transform self)
    {
        return new Vector2(self.localPosition.x, self.localPosition.y);
    }

    /// <summary>
    /// XZの位置をVector2型で取得
    /// </summary>
    public static Vector2 GetVector2PositionXZ(this Transform self)
    {
        return new Vector2(self.position.x, self.position.z);
    }

    /// <summary>
    /// XZのローカル位置をVector2型で取得
    /// </summary>
    public static Vector2 GetVector2LocalPositionXZ(this Transform self)
    {
        return new Vector2(self.localPosition.x, self.localPosition.z);
    }

    #endregion

    #region SetPosition

    public static void SetPosition(this Transform self, float x, float y, float z)
    {
        m_tmpVector3.Set(x, y, z);
        self.position = m_tmpVector3;
    }

    public static void SetPosition(this Transform self, float x, float y)
    {
        self.SetPosition(x, y, self.position.z);
    }

    public static void SetPositionX(this Transform self, float x)
    {
        self.SetPosition(x, self.position.y, self.position.z);
    }

    public static void SetPositionY(this Transform self, float y)
    {
        self.SetPosition(self.position.x, y, self.position.z);
    }

    public static void SetPositionZ(this Transform self, float z)
    {
        self.SetPosition(self.position.x, self.position.y, z);
    }

    #endregion

    #region SetLocalPosition

    public static void SetLocalPosition(this Transform self, float x, float y, float z)
    {
        m_tmpVector3.Set(x, y, z);
        self.localPosition = m_tmpVector3;
    }

    public static void SetLocalPosition(this Transform self, float x, float y)
    {
        self.SetLocalPosition(x, y, self.localPosition.z);
    }

    public static void SetLocalPositionX(this Transform self, float x)
    {
        self.SetLocalPosition(x, self.localPosition.y, self.localPosition.z);
    }

    public static void SetLocalPositionY(this Transform self, float y)
    {
        self.SetLocalPosition(self.localPosition.x, y, self.localPosition.z);
    }

    public static void SetLocalPositionZ(this Transform self, float z)
    {
        self.SetLocalPosition(self.localPosition.x, self.localPosition.y, z);
    }

    #endregion

    #region SetEulerAngles

    public static void SetEulerAngles(this Transform self, float x, float y, float z)
    {
        self.rotation = Quaternion.Euler(x, y, z);
    }

    public static void SetEulerAnglesX(this Transform self, float x)
    {
        Vector3 selfAngles = self.eulerAngles;
        self.rotation = Quaternion.Euler(x, selfAngles.y, selfAngles.z);
    }

    public static void SetEulerAnglesY(this Transform self, float y)
    {
        Vector3 selfAngles = self.eulerAngles;
        self.rotation = Quaternion.Euler(selfAngles.x, y, selfAngles.z);
    }

    public static void SetEulerAnglesZ(this Transform self, float z)
    {
        Vector3 selfAngles = self.eulerAngles;
        self.rotation = Quaternion.Euler(selfAngles.x, selfAngles.y, z);
    }

    #endregion

    #region SetLocalEulerAngles

    public static void SetLocalEulerAngles(this Transform self, float x, float y, float z)
    {
        self.localRotation = Quaternion.Euler(x, y, z);
    }

    public static void SetLocalEulerAnglesX(this Transform self, float x)
    {
        Vector3 selfAngles = self.localEulerAngles;
        self.localRotation = Quaternion.Euler(x, selfAngles.y, selfAngles.z);
    }

    public static void SetLocalEulerAnglesY(this Transform self, float y)
    {
        Vector3 selfAngles = self.localEulerAngles;
        self.localRotation = Quaternion.Euler(selfAngles.x, y, selfAngles.z);
    }

    public static void SetLocalEulerAnglesZ(this Transform self, float z)
    {
        Vector3 selfAngles = self.localEulerAngles;
        self.localRotation = Quaternion.Euler(selfAngles.x, selfAngles.y, z);
    }

    #endregion

    #region SetLocalScale

    public static void SetLocalScale(this Transform self, float x, float y, float z)
    {
        m_tmpVector3.Set(x, y, z);
        self.localScale = m_tmpVector3;
    }

    public static void SetLocalScaleX(this Transform self, float x)
    {
        self.SetLocalScale(x, self.localScale.y, self.localScale.z);
    }

    public static void SetLocalScaleY(this Transform self, float y)
    {
        self.SetLocalScale(self.localScale.x, y, self.localScale.z);
    }

    public static void SetLocalScaleZ(this Transform self, float z)
    {
        self.SetLocalScale(self.localScale.x, self.localScale.y, z);
    }

    #endregion

    #region AddPosition

    public static void AddPosition(this Transform self, float x, float y, float z)
    {
        self.SetPosition(self.position.x + x, self.position.y + y, self.position.z + z);
    }

    public static void AddPositionX(this Transform self, float x)
    {
        self.SetPositionX(self.position.x + x);
    }

    public static void AddPositionY(this Transform self, float y)
    {
        self.SetPositionY(self.position.y + y);
    }

    public static void AddPositionZ(this Transform self, float z)
    {
        self.SetPositionZ(self.position.z + z);
    }

    #endregion

    #region AddLocalPosition

    public static void AddLocalPosition(this Transform self, float x, float y, float z)
    {
        self.SetLocalPosition(self.localPosition.x + x, self.localPosition.y + y, self.localPosition.z + z);
    }

    public static void AddLocalPositionX(this Transform self, float x)
    {
        self.SetLocalPositionX(self.localPosition.x + x);
    }

    public static void AddLocalPositionY(this Transform self, float y)
    {
        self.SetLocalPositionY(self.localPosition.y + y);
    }

    public static void AddLocalPositionZ(this Transform self, float z)
    {
        self.SetLocalPositionZ(self.localPosition.z + z);
    }

    #endregion

    #region AddEulerAngles

    public static void AddEulerAngles(this Transform self, float x, float y, float z)
    {
        Vector3 selfAngles = self.eulerAngles;
        self.rotation = Quaternion.Euler(selfAngles.x + x, selfAngles.y + y, selfAngles.z + z);
    }

    public static void AddEulerAnglesX(this Transform self, float x)
    {
        Vector3 selfAngles = self.eulerAngles;
        self.rotation = Quaternion.Euler(selfAngles.x + x, selfAngles.y, selfAngles.z);
    }

    public static void AddEulerAnglesY(this Transform self, float y)
    {
        Vector3 selfAngles = self.eulerAngles;
        self.rotation = Quaternion.Euler(selfAngles.x, selfAngles.y + y, selfAngles.z);
    }

    public static void AddEulerAnglesZ(this Transform self, float z)
    {
        Vector3 selfAngles = self.eulerAngles;
        self.rotation = Quaternion.Euler(selfAngles.x, selfAngles.y, selfAngles.z + z);
    }

    #endregion

    #region AddLocalEulerAngles

    public static void AddLocalEulerAngles(this Transform self, float x, float y, float z)
    {
        Vector3 selfAngles = self.localEulerAngles;
        self.localRotation = Quaternion.Euler(selfAngles.x + x, selfAngles.y + y, selfAngles.z + z);
    }

    public static void AddLocalEulerAnglesX(this Transform self, float x)
    {
        Vector3 selfAngles = self.localEulerAngles;
        self.localRotation = Quaternion.Euler(selfAngles.x + x, selfAngles.y, selfAngles.z);
    }

    public static void AddLocalEulerAnglesY(this Transform self, float y)
    {
        Vector3 selfAngles = self.localEulerAngles;
        self.localRotation = Quaternion.Euler(selfAngles.x, selfAngles.y + y, selfAngles.z);
    }

    public static void AddLocalEulerAnglesZ(this Transform self, float z)
    {
        Vector3 selfAngles = self.localEulerAngles;
        self.localRotation = Quaternion.Euler(selfAngles.x, selfAngles.y, selfAngles.z + z);
    }

    #endregion

    #region AddLocalScale

    public static void AddLocalScale(this Transform self, float x, float y, float z)
    {
        self.SetLocalScale(self.localScale.x + x, self.localScale.y + y, self.localScale.z + z);
    }

    public static void AddLocalScaleX(this Transform self, float x)
    {
        self.SetLocalScaleX(self.localScale.x + x);
    }

    public static void AddLocalScaleY(this Transform self, float y)
    {
        self.SetLocalScaleY(self.localScale.y + y);
    }

    public static void AddLocalScaleZ(this Transform self, float z)
    {
        self.SetLocalScaleZ(self.localScale.z + z);
    }

    #endregion

    #region ClampPosition

    public static void ClampPosition(this Transform self, Vector3 min, Vector3 max)
    {
        float newX = Mathf.Clamp(self.position.x, min.x, max.x);
        float newY = Mathf.Clamp(self.position.y, min.y, max.y);
        float newZ = Mathf.Clamp(self.position.z, min.z, max.z);
        self.SetPosition(newX, newY, newZ);
    }

    public static void ClampPosition(this Transform self, Vector2 min, Vector2 max)
    {
        float newX = Mathf.Clamp(self.position.x, min.x, max.x);
        float newY = Mathf.Clamp(self.position.y, min.y, max.y);
        self.SetPosition(newX, newY);
    }

    public static void ClampPositionX(this Transform self, float min, float max)
    {
        self.SetPositionX(Mathf.Clamp(self.position.x, min, max));
    }

    public static void ClampPositionY(this Transform self, float min, float max)
    {
        self.SetPositionY(Mathf.Clamp(self.position.y, min, max));
    }

    public static void ClampPositionZ(this Transform self, float min, float max)
    {
        self.SetPositionZ(Mathf.Clamp(self.position.z, min, max));
    }

    #endregion

    #region ClampLocalPosition

    public static void ClampLocalPosition(this Transform self, Vector3 min, Vector3 max)
    {
        float newX = Mathf.Clamp(self.localPosition.x, min.x, max.x);
        float newY = Mathf.Clamp(self.localPosition.y, min.y, max.y);
        float newZ = Mathf.Clamp(self.localPosition.z, min.z, max.z);
        self.SetLocalPosition(newX, newY, newZ);
    }

    public static void ClampLocalPosition(this Transform self, Vector2 min, Vector2 max)
    {
        float newX = Mathf.Clamp(self.localPosition.x, min.x, max.x);
        float newY = Mathf.Clamp(self.localPosition.y, min.y, max.y);
        self.SetLocalPosition(newX, newY);
    }

    public static void ClampLocalPositionX(this Transform self, float min, float max)
    {
        self.SetLocalPositionX(Mathf.Clamp(self.localPosition.x, min, max));
    }

    public static void ClampLocalPositionY(this Transform self, float min, float max)
    {
        self.SetLocalPositionY(Mathf.Clamp(self.localPosition.y, min, max));
    }

    public static void ClampLocalPositionZ(this Transform self, float min, float max)
    {
        self.SetLocalPositionZ(Mathf.Clamp(self.localPosition.z, min, max));
    }

    #endregion

    #region ClampEulerAngles

    public static void ClampEulerAngles(this Transform self, Vector3 min, Vector3 max)
    {
        float newX = Mathf.Clamp(self.eulerAngles.x, min.x, max.x);
        float newY = Mathf.Clamp(self.eulerAngles.y, min.y, max.y);
        float newZ = Mathf.Clamp(self.eulerAngles.z, min.z, max.z);
        self.SetEulerAngles(newX, newY, newZ);
    }

    public static void ClampEulerAnglesX(this Transform self, float min, float max)
    {
        self.SetEulerAnglesX(Mathf.Clamp(self.eulerAngles.x, min, max));
    }

    public static void ClampEulerAnglesY(this Transform self, float min, float max)
    {
        self.SetEulerAnglesY(Mathf.Clamp(self.eulerAngles.y, min, max));
    }

    public static void ClampEulerAnglesZ(this Transform self, float min, float max)
    {
        self.SetEulerAnglesZ(Mathf.Clamp(self.eulerAngles.z, min, max));
    }

    #endregion

    #region ClampLocalEulerAngles

    public static void ClampLocalEulerAngles(this Transform self, Vector3 min, Vector3 max)
    {
        float newX = Mathf.Clamp(self.localEulerAngles.x, min.x, max.x);
        float newY = Mathf.Clamp(self.localEulerAngles.y, min.y, max.y);
        float newZ = Mathf.Clamp(self.localEulerAngles.z, min.z, max.z);
        self.SetLocalEulerAngles(newX, newY, newZ);
    }

    public static void ClampLocalEulerAnglesX(this Transform self, float min, float max)
    {
        self.SetLocalEulerAnglesX(Mathf.Clamp(self.localEulerAngles.x, min, max));
    }

    public static void ClampLocalEulerAnglesY(this Transform self, float min, float max)
    {
        self.SetLocalEulerAnglesY(Mathf.Clamp(self.localEulerAngles.y, min, max));
    }

    public static void ClampLocalEulerAnglesZ(this Transform self, float min, float max)
    {
        self.SetLocalEulerAnglesZ(Mathf.Clamp(self.localEulerAngles.z, min, max));
    }

    #endregion

    #region ClampLocalScale

    public static void ClampLocalScale(this Transform self, Vector3 min, Vector3 max)
    {
        float newX = Mathf.Clamp(self.localScale.x, min.x, max.x);
        float newY = Mathf.Clamp(self.localScale.y, min.y, max.y);
        float newZ = Mathf.Clamp(self.localScale.z, min.z, max.z);
        self.SetLocalScale(newX, newY, newZ);
    }

    public static void ClampLocalScaleX(this Transform self, float min, float max)
    {
        self.SetLocalScaleX(Mathf.Clamp(self.localScale.x, min, max));
    }

    public static void ClampLocalScaleY(this Transform self, float min, float max)
    {
        self.SetLocalScaleY(Mathf.Clamp(self.localScale.y, min, max));
    }

    public static void ClampLocalScaleZ(this Transform self, float min, float max)
    {
        self.SetLocalScaleZ(Mathf.Clamp(self.localScale.z, min, max));
    }

    #endregion

    #region HasChanged

    public static void HasChanged(this Transform self, Action changed)
    {
        if (self.hasChanged)
        {
            changed();
            self.hasChanged = false;
        }
    }

    static void HasChanged(this Transform self, Action<Transform> changed)
    {
        if (self.hasChanged)
        {
            changed(self);
            self.hasChanged = false;
        }
    }

    public static void HasChangedInChildren(this Transform self, Action<Transform> changed)
    {
        Transform[] childs = self.GetComponentsInChildren<Transform>();
        if (childs == null)
        {
            return;
        }
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].HasChanged(changed);
        }
    }

    public static void HasChangedInParent(this Transform self, Action<Transform> changed)
    {
        Transform[] parents = self.GetComponentsInParent<Transform>();
        if (parents == null)
        {
            return;
        }
        for (int i = 0; i < parents.Length; i++)
        {
            parents[i].HasChanged(changed);
        }
    }

    #endregion

    #region LookAt2D

    public static void LookAt2D(this Transform self, Transform target)
    {
        LookAt2D(self, target.position, Vector3.forward, 0);
    }

    public static void LookAt2D(this Transform self, Vector2 target)
    {
        LookAt2D(self, target, Vector3.forward, 0);
    }

    public static void LookAt2D(this Transform self, Transform target, float angle)
    {
        LookAt2D(self, target.position, Vector3.forward, angle);
    }

    public static void LookAt2D(this Transform self, Vector2 target, float angle)
    {
        LookAt2D(self, target, Vector3.forward, angle);
    }

    public static void LookAt2D(this Transform self, Transform target, Vector3 axis)
    {
        LookAt2D(self, target.position, axis, 0);
    }

    public static void LookAt2D(this Transform self, Vector2 target, Vector3 axis)
    {
        LookAt2D(self, target, axis, 0);
    }

    public static void LookAt2D(this Transform self, Transform target, Vector3 axis, float angle)
    {
        LookAt2D(self, target.position, axis, angle);
    }

    public static void LookAt2D(this Transform self, Vector2 target, Vector3 axis, float angle)
    {
        m_tmpVector2.Set(target.x - self.position.x, target.y - self.position.y);
        angle = angle + Mathf.Atan2(m_tmpVector2.y, m_tmpVector2.x) * Mathf.Rad2Deg;
        self.rotation = Quaternion.AngleAxis(angle, axis);
    }

    #endregion

    #region Find

    public static Transform FindInChildren(this Transform self, string name)
    {
        int count = self.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = self.GetChild(i);
            if (child.name == name) return child;
            Transform subChild = child.FindInChildren(name);
            if (subChild != null) return subChild;
        }
        return null;
    }

    #endregion

    #region Distance

    public static float Distance(this Transform self, Transform other, bool localPosition = false)
    {
        if (localPosition)
        {
            return Vector3.Distance(self.localPosition, other.localPosition);
        }
        else
        {
            return Vector3.Distance(self.position, other.position);
        }
    }

    public static float DistanceXY(this Transform self, Transform other, bool localPosition = false)
    {
        if (localPosition)
        {
            return Vector2.Distance(self.GetVector2LocalPositionXY(), other.GetVector2LocalPositionXY());
        }
        else
        {
            return Vector2.Distance(self.GetVector2PositionXY(), other.GetVector2PositionXY());
        }
    }

    public static float DistanceXZ(this Transform self, Transform other, bool localPosition = false)
    {
        if (localPosition)
        {
            return Vector2.Distance(self.GetVector2LocalPositionXZ(), other.GetVector2LocalPositionXZ());
        }
        else
        {
            return Vector2.Distance(self.GetVector2PositionXZ(), other.GetVector2PositionXZ());
        }
    }

    #endregion

    #region AngleCheck

    public static bool IsFrontAngle(this Transform self, Vector3 target, float checkAngle)
    {
        return Vector3.Angle(target - self.position, self.forward) <= checkAngle;
    }

    public static bool IsBackAngle(this Transform self, Vector3 target, float checkAngle)
    {
        return Vector3.Angle(target - self.position, -self.forward) <= checkAngle;
    }

    public static bool IsRightAngle(this Transform self, Vector3 target, float checkAngle)
    {
        return Vector3.Angle(target - self.position, self.right) <= checkAngle;
    }

    public static bool IsLeftAngle(this Transform self, Vector3 target, float checkAngle)
    {
        return Vector3.Angle(target - self.position, -self.right) <= checkAngle;
    }

    public static bool IsUpAngle(this Transform self, Vector3 target, float checkAngle)
    {
        return Vector3.Angle(target - self.position, self.up) <= checkAngle;
    }

    public static bool IsDownAngle(this Transform self, Vector3 target, float checkAngle)
    {
        return Vector3.Angle(target - self.position, -self.up) <= checkAngle;
    }

    #endregion
}