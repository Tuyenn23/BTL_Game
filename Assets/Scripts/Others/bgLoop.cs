using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgLoop : MonoBehaviour
{
    public float scrollSpeedY = 1.0f; // Tốc độ cuộn nền theo trục Y
    private float tileSizeY; // Chiều cao của tile (ảnh nền)'

    

    private Transform camTransform;
    private Vector3 startPosition;

    private void Start()
    {
        camTransform = Camera.main.transform;
        startPosition = transform.position;
        tileSizeY = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    private void Update()
    {
        // Tính toán vị trí mới cho nền theo trục Y
        float newPositionY = Mathf.Repeat(Time.time * scrollSpeedY, tileSizeY);
        // Di chuyển nền theo vị trí mới này
        transform.position = startPosition + Vector3.down * newPositionY;
    }
}
