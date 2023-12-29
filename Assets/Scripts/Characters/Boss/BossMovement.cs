using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class BossMovement : MonoBehaviour
{
    public float moveDistance = 5f; // Khoảng cách di chuyển
    public float moveDuration = 2f; // Thời gian di chuyển
    

    void Start()
    {
        MoveLeftRightLoop();
       
    }

    void MoveLeftRightLoop()
    {
        // Sử dụng DOLocalMove để di chuyển đối tượng sang trái rồi lại sang phải và lặp lại
        transform.DOLocalMoveX(-moveDistance, moveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(MoveRight);
    }

    private void MoveRight()
    {
        transform.DOLocalMoveX(moveDistance, moveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(MoveLeftRightLoop);
    }
}
