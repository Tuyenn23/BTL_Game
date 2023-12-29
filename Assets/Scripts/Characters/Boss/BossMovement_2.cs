using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BossMovement_2 : MonoBehaviour
{
    public float moveDistance = 5f; // Khoảng cách di chuyển
    public float moveDuration = 2f; // Thời gian di chuyển
    public float delayBetweenMoves = 3f; // Thời gian giữa các lần di chuyển

    void Start()
    {
        MoveUpDownLoop();
    }

    void MoveUpDownLoop()
    {
        // Sử dụng DOMove để di chuyển từ trên xuống
        transform.DOMoveY(/*transform.position.y - moveDistance*/ 1.7f, moveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => ReturnToOriginalPosition());
    }

    void ReturnToOriginalPosition()
    {
        // Giữ một khoảng thời gian trước khi quay lại vị trí gốc
        DOTween.Sequence()
            .AppendInterval(delayBetweenMoves)
            .Append(transform.DOMoveY(/*transform.position.y + moveDistance*/ 5.5f, moveDuration)
                     .SetEase(Ease.Linear)
                     .OnComplete(() => MoveUpDownLoop()));
    }
}
