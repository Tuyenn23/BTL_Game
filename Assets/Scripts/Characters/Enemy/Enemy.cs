﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseEnemy
{
    public Transform firePos;
    public GameObject bulletPrefab;
    //public float timeBtwFire;
    public Transform holderPrefabs;
    
    public float minShootInterval = 1f;
    public float maxShootInterval = 3f;

    private float timeToShoot;
    private float nextShootTime;

    public GameObject[] itemPrefab;
    public float dropProbability = 30f; // Tỉ lệ phần trăm

    void Start()
    {
        // Khởi tạo thời gian bắn ngẫu nhiên ban đầu
        SetNextShootTime();
    }

    
    protected override void FireBullet()
    {
        
        //_timeBtwFire -= Time.deltaTime;
        if (Time.time > timeToShoot)
        {
            //_timeBtwFire = timeBtwFire;
            anim.SetBool("Shooting", true);
            SetNextShootTime();

            /*GameObject bulletClone = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);*/
            SimplePool.Spawn(bulletPrefab, firePos.position, Quaternion.identity);
            /*Rigidbody2D rb = bulletClone.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.down * bulletForce, ForceMode2D.Impulse);*/
        }
    }

    void SetNextShootTime()
    {
        // Thiết lập thời gian bắn ngẫu nhiên trong khoảng min và max
        nextShootTime = Random.Range(minShootInterval, maxShootInterval);
        timeToShoot = Time.time + nextShootTime;
    }


    public void EndShootingEvent()
    {
        anim.SetBool("Shooting", false);
    }

    private void OnDisable()
    {
        // Kiểm tra xác suất rơi vật phẩm
        if (Random.Range(0f, 100f) <= dropProbability)
        {
            Instantiate(itemPrefab[Random.Range(0, itemPrefab.Length)], transform.position, Quaternion.identity);
        }
    }



}
