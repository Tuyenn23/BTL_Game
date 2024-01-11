using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAir : MonoBehaviour
{
    Vector2 difference = Vector2.zero;
    

    private Camera mainCamera;
    private float minX, maxX, minY, maxY;
    private float playerHalfWidth, playerHalfHeight;

    private void OnMouseDown()
    {
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }

    private void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }

    private void moveShip()
    {
        transform.position = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition) - difference;
    }

    private void Start()
    {
        

        mainCamera = Camera.main;
        // Lấy kích thước của người chơi (Player)
        SpriteRenderer playerRenderer = GetComponent<SpriteRenderer>();
        playerHalfWidth = playerRenderer.bounds.size.x / 2f;
        playerHalfHeight = playerRenderer.bounds.size.y / 2f;

        // Tính toán giới hạn dựa trên Camera
        CalculateCameraBounds();
    }

    private void Update()
    {
        CheckLimitCam();
        FireBullet();
        //moveShip();
    }

    private void FixedUpdate()
    {

    }


    void CalculateCameraBounds()
    {
        if (mainCamera != null)
        {
            float cameraOrthographicSize = mainCamera.orthographicSize;
            float aspectRatio = Screen.width / (float)Screen.height;

            // Tính toán giới hạn dựa trên Camera
            minX = mainCamera.transform.position.x - cameraOrthographicSize * aspectRatio;
            maxX = mainCamera.transform.position.x + cameraOrthographicSize * aspectRatio;
            minY = mainCamera.transform.position.y - cameraOrthographicSize;
            maxY = mainCamera.transform.position.y + cameraOrthographicSize;
        }
    }

    void CheckLimitCam()
    {
        //moveShip();
        // Lấy vị trí hiện tại của người chơi
        Vector3 currentPosition = transform.position;

        // Giới hạn vị trí trên trục X
        currentPosition.x = Mathf.Clamp(currentPosition.x, minX + playerHalfWidth, maxX - playerHalfWidth);

        // Giới hạn vị trí trên trục Y
        currentPosition.y = Mathf.Clamp(currentPosition.y, minY + playerHalfHeight, maxY - playerHalfHeight);

        // Cập nhật vị trí của người chơi
        transform.position = currentPosition;

        // Xử lý di chuyển của người chơi
        // Đảm bảo bạn có mã xử lý di chuyển ở đây, ví dụ: Input.GetKey, Input.GetAxis, ...
    }
    protected virtual void FireBullet()
    {

    }



}
