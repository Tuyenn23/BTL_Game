using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTime : MonoBehaviour
{
    public float shieldDuration = 5f; // Thời gian khiên tồn tại
    private bool isShieldActive = false;

    private void OnEnable()
    {
        ActivateShield();
    }

    public void ActivateShield()
    {
        // Kích hoạt khiên và bắt đầu Coroutine để đếm thời gian
        isShieldActive = true;
        StartCoroutine(ShieldTimer());
    }

    IEnumerator ShieldTimer()
    {
        yield return new WaitForSeconds(shieldDuration);

        // Hủy kích hoạt khiên khi thời gian hạn chế kết thúc
        isShieldActive = false;
        AudioController.Instance.PlaySound(AudioController.Instance.shieldBreak);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopCoroutine(ShieldTimer());
    }

}
