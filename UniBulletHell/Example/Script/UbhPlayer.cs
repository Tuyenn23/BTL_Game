using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(UbhSpaceship))]
public class UbhPlayer : UbhMonoBehaviour
{
    public const string NAME_ENEMY_BULLET = "EnemyBullet";
    public const string NAME_ENEMY = "Enemy";

    private const string AXIS_HORIZONTAL = "Horizontal";
    private const string AXIS_VERTICAL = "Vertical";

    private readonly Vector2 VIEW_PORT_LEFT_BOTTOM = UbhUtil.VECTOR2_ZERO;
    private readonly Vector2 VIEW_PORT_RIGHT_TOP = UbhUtil.VECTOR2_ONE;

    [SerializeField, FormerlySerializedAs("_UseAxis")]
    private UbhUtil.AXIS m_useAxis = UbhUtil.AXIS.X_AND_Y;

    private UbhSpaceship m_spaceship;
    private UbhGameManager m_manager;
    private Transform m_backgroundTransform;
    private bool m_isTouch;
    private float m_lastXpos;
    private float m_lastYpos;
    private Vector2 m_tempVector2 = UbhUtil.VECTOR2_ZERO;

    public bool isDead { get; set; }

    private void Start()
    {
        m_spaceship = GetComponent<UbhSpaceship>();
        m_manager = FindObjectOfType<UbhGameManager>();
        m_backgroundTransform = FindObjectOfType<UbhBackground>().transform;
    }

    private void Update()
    {
        if (UbhUtil.IsMobilePlatform())
        {
            TouchMove();
#if UNITY_EDITOR
            KeyMove();
#endif
        }
        else
        {
            KeyMove();
        }
    }

    private void KeyMove()
    {
        m_tempVector2.x = Input.GetAxisRaw(AXIS_HORIZONTAL);
        m_tempVector2.y = Input.GetAxisRaw(AXIS_VERTICAL);
        Move(m_tempVector2.normalized);
    }

    private void TouchMove()
    {
        float xPos = 0f;
        float yPos = 0f;
        if (Input.GetMouseButtonDown(0))
        {
            m_isTouch = true;
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            xPos = vec.x;
            yPos = vec.y;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            xPos = vec.x;
            yPos = vec.y;
            if (m_isTouch)
            {
                m_tempVector2.x = (xPos - m_lastXpos) * 10f;
                m_tempVector2.y = (yPos - m_lastYpos) * 10f;
                Move(m_tempVector2.normalized);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_isTouch = false;
        }
        m_lastXpos = xPos;
        m_lastYpos = yPos;
    }

    private void Move(Vector2 direction)
    {
        Vector2 min;
        Vector2 max;
        if (m_manager != null && m_manager.m_scaleToFit)
        {
            min = Camera.main.ViewportToWorldPoint(VIEW_PORT_LEFT_BOTTOM);
            max = Camera.main.ViewportToWorldPoint(VIEW_PORT_RIGHT_TOP);
        }
        else
        {
            Vector2 scale = m_backgroundTransform.localScale;
            min = scale * -0.5f;
            max = scale * 0.5f;
        }

        Vector2 pos = transform.position;
        if (m_useAxis == UbhUtil.AXIS.X_AND_Z)
        {
            pos.y = transform.position.z;
        }

        pos += direction * m_spaceship.m_speed * UbhTimer.instance.deltaTime;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        if (m_useAxis == UbhUtil.AXIS.X_AND_Z)
        {
            transform.SetPosition(pos.x, transform.position.y, pos.y);
        }
        else
        {
            transform.position = pos;
        }
    }

    private void Damage()
    {
        isDead = true;

        if (m_manager != null)
        {
            m_manager.GameOver();
        }

        m_spaceship.Explosion();

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        HitCheck(c.transform);
    }

    private void OnTriggerEnter(Collider c)
    {
        HitCheck(c.transform);
    }

    private void HitCheck(Transform colTrans)
    {
        // *It is compared with name in order to separate as Asset from project settings.
        //  However, it is recommended to use Layer or Tag.
        string goName = colTrans.name;
        if (goName.Contains(NAME_ENEMY_BULLET))
        {
            UbhBullet bullet = colTrans.GetComponentInParent<UbhBullet>();
            if (bullet.isActive)
            {
                UbhObjectPool.instance.ReleaseBullet(bullet);
                Damage();
            }
        }
        else if (goName.Contains(NAME_ENEMY))
        {
            Damage();
        }
    }
}
