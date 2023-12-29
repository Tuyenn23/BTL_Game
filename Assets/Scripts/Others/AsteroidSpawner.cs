using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] asteroidPrefabs;
    [SerializeField] float spawnInterval = 1f;
    [SerializeField] float minpPosX, maxPosX, posY;

    public int countEnemy;
    private int countSpawn = 0;

    private void Start()
    {
        InvokeRepeating("SpawnAsteroid", 0f, spawnInterval);
    }
    void SpawnAsteroid()
    {
        if(countSpawn == countEnemy) return;
        // Lấy một vị trí ngẫu nhiên trên trục X va 1 diem y
        float randomX = Random.Range(minpPosX, maxPosX);
        

        // Chọn ngẫu nhiên một loại thiên thạch từ mảng
        GameObject randomAsteroidPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

        // Tạo ra một thiên thạch tại vị trí ngẫu nhiên trên trục y
        Instantiate(randomAsteroidPrefab, new Vector3(randomX, 10f, transform.position.z), transform.rotation);
        countSpawn++;
        /*SimplePool.Spawn(randomAsteroidPrefab, new Vector3(randomX, posY, transform.position.z), Quaternion.identity);*/

    }
}
