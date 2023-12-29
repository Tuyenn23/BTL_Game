using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caution : MonoBehaviour
{
    public float flashDuration = 0.2f;
    public float disappearDelay = 0.2f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Gọi coroutine để thực hiện nhấp nháy và biến mất
        StartCoroutine(FlashAndDisappearRoutine());
    }

    IEnumerator FlashAndDisappearRoutine()
    {
        // Lặp để nhấp nháy sprite
        for (int i = 0; i < 3; i++) // Chẳng hạn, nhấp nháy 3 lần
        {
            // Hiển thị sprite
            spriteRenderer.enabled = true;

            // Đợi một khoảng thời gian
            yield return new WaitForSeconds(flashDuration);

            // Ẩn sprite
            spriteRenderer.enabled = false;

            // Đợi một khoảng thời gian trước khi nhấp nháy tiếp theo
            yield return new WaitForSeconds(disappearDelay);
        }

        // Sau khi nhấp nháy xong, có thể thực hiện các hành động khác, chẳng hạn như phá hủy đối tượng
        Destroy(gameObject);
    }
}
