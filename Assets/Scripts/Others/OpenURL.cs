using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenURL : MonoBehaviour
{
    public string targetURL = "https://github.com/Tuyenn23/BTL_Game";

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(Open);
        }
    }

    void Open()
    {
        Application.OpenURL(targetURL);
        AudioController.Instance.PlaySound(AudioController.Instance.click);
    }
}
