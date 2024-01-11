using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(UbhSpaceship))]
public class UbhEnemy : UbhMonoBehaviour
{
    public const string NAME_PLAYER = "Player";
    public const string NAME_PLAYER_BULLET = "PlayerBullet";

    private const string ANIM_DAMAGE_TRIGGER = "Damage";

    [SerializeField, FormerlySerializedAs("_Hp")]
    private int m_hp = 1;
    [SerializeField, FormerlySerializedAs("_Point")]
    private int m_point = 100;
    [SerializeField, FormerlySerializedAs("_UseStop")]
    private bool m_useStop = false;
    [SerializeField, FormerlySerializedAs("_StopPoint")]
    private float m_stopPoint = 2f;

    private UbhSpaceship m_spaceship;

    private void Start()
    {
        m_spaceship = GetComponent<UbhSpaceship>();

        Move(transform.up * -1);
    }

    private void FixedUpdate()
    {
        if (m_useStop)
        {
            if (transform.position.y < m_stopPoint)
            {
                rigidbody2D.velocity = UbhUtil.VECTOR2_ZERO;
                m_useStop = false;
            }
        }
    }

    public void Move(Vector2 direction)
    {
        rigidbody2D.velocity = direction * m_spaceship.m_speed;
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        // *It is compared with name in order to separate as Asset from project settings.
        //  However, it is recommended to use Layer or Tag.
        if (c.name.Contains(NAME_PLAYER_BULLET))
        {
            UbhPlayerBullet playerBullet = c.transform.parent.GetComponent<UbhPlayerBullet>();
            if (playerBullet != null && playerBullet.isActive)
            {
                UbhObjectPool.instance.ReleaseBullet(playerBullet);

                m_hp = m_hp - playerBullet.m_power;

                if (m_hp <= 0)
                {
                    FindObjectOfType<UbhScore>().AddPoint(m_point);

                    m_spaceship.Explosion();

                    Destroy(gameObject);
                }
                else
                {
                    m_spaceship.GetAnimator().SetTrigger(ANIM_DAMAGE_TRIGGER);
                }
            }
        }
    }
}