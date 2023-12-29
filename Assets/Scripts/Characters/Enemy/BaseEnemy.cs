using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField]
    protected Animator anim;

   
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        FireBullet();
    }

   

    protected virtual void FireBullet()
    {

    }

    /*protected virtual void SetNextShootTime()
    {

    }*/

    
}
