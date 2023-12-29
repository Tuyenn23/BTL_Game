using Game_Fly;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : BaseAir
{
    public Transform firePos;
    public GameObject bulletPrefab;
    public float timeBtwFire = 0.2f;

    float _timeBtwFire;

    protected override void FireBullet()
    {
        if (!GameManager.Instance.isStartGame) return;
        _timeBtwFire -= Time.deltaTime;
        if (_timeBtwFire < 0)
        {
            _timeBtwFire = timeBtwFire;
            // GameObject bulletClone = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);
            SimplePool.Spawn(bulletPrefab, firePos.position, Quaternion.identity);


            /*Rigidbody2D rb = bulletClone.GetComponent<Rigidbody2D>();
            rb.AddForce(transform.up * bulletForce, ForceMode2D.Impulse);*/
        }
        
    }



    
}
