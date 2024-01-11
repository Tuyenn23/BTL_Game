using UnityEngine;

/// <summary>
/// Ubh bullet for sprite2d and rigidbody2d prefabs.
/// </summary>
public class UbhBulletSimpleModel3d : UbhBullet
{
    [SerializeField]
    private Rigidbody m_rigidbody3d = null;
    [SerializeField]
    private Collider[] m_collider3ds = null;
    [SerializeField]
    private MeshRenderer[] m_meshRenderers = null;

    private bool m_isActive;

    /// <summary>
    /// Activate/Inactivate flag
    /// Override this property when you want to change the behavior at Active / Inactive.
    /// </summary>
    public override bool isActive { get { return m_isActive; } }

    /// <summary>
    /// Activate/Inactivate Bullet
    /// </summary>
    public override void SetActive(bool isActive)
    {
        m_isActive = isActive;

        m_rigidbody3d.detectCollisions = isActive;

        if (m_collider3ds != null && m_collider3ds.Length > 0)
        {
            for (int i = 0; i < m_collider3ds.Length; i++)
            {
                m_collider3ds[i].enabled = isActive;
            }
        }

        if (m_meshRenderers != null && m_meshRenderers.Length > 0)
        {
            for (int i = 0; i < m_meshRenderers.Length; i++)
            {
                m_meshRenderers[i].enabled = isActive;
            }
        }
    }
}
