using UnityEngine;
using UnityEngine.Serialization;

public class UbhSpaceship : UbhMonoBehaviour
{
    [FormerlySerializedAs("_Speed")]
    public float m_speed;

    [SerializeField, FormerlySerializedAs("_ExplosionPrefab")]
    private GameObject m_explosionPrefab = null;

    private Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Explosion()
    {
        if (m_explosionPrefab != null)
        {
            Instantiate(m_explosionPrefab, transform.position, transform.rotation);
        }
    }

    public Animator GetAnimator()
    {
        return m_animator;
    }
}
