using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
    [SerializeField]
    public Transform[] routes;
    private Animator anim;

    public int routeToGo;

    private float tParam;

    private Vector2 objectPosition;

    private float speedModifier;

    private bool coroutineAllowed;
    public int id;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        if (id == 1)
        {
            routes[0] = FindObjectOfType<RouteMove1>().transform;
        }
        else
        {
            routes[0] = FindObjectOfType<RouteMove2>().transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 0.4f;
        coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int routeNum)
    {
        coroutineAllowed = false;

        Vector2 p0 = routes[routeNum].GetChild(0).position;
        Vector2 p1 = routes[routeNum].GetChild(1).position;
        Vector2 p2 = routes[routeNum].GetChild(2).position;
        Vector2 p3 = routes[routeNum].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 *
              (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

            transform.position = objectPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;

        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
            //army
            anim.enabled = true;
            //GameManager.Instance.enemyState = EnemyState.SHOOTING;
        }

        //coroutineAllowed = true;
    }


}
