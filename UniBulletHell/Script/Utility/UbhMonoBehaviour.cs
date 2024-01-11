using UnityEngine;

/// <summary>
/// Ubh mono behaviour.
/// </summary>
public abstract class UbhMonoBehaviour : MonoBehaviour
{
    private GameObject m_gameObject;
    private Transform m_transform;
    private Renderer m_renderer;
    private Rigidbody m_rigidbody;
    private Rigidbody2D m_rigidbody2D;

    public new GameObject gameObject
    {
        get
        {
            if (m_gameObject == null)
            {
                m_gameObject = base.gameObject;
            }
            return m_gameObject;
        }
    }

    public new Transform transform
    {
        get
        {
            if (m_transform == null)
            {
                m_transform = GetComponent<Transform>();
            }
            return m_transform;
        }
    }

    public new Renderer renderer
    {
        get
        {
            if (m_renderer == null)
            {
                m_renderer = GetComponent<Renderer>();
            }
            return m_renderer;
        }
    }

    public new Rigidbody rigidbody
    {
        get
        {
            if (m_rigidbody == null)
            {
                m_rigidbody = GetComponent<Rigidbody>();
            }
            return m_rigidbody;
        }
    }

    public new Rigidbody2D rigidbody2D
    {
        get
        {
            if (m_rigidbody2D == null)
            {
                m_rigidbody2D = GetComponent<Rigidbody2D>();
            }
            return m_rigidbody2D;
        }
    }
}
