/*
using UnityEngine;

[RequireComponent(typeof(GUIText))]
public class UbhAutoScaleGUIText : MonoBehaviour
{
    private GUIText m_guiText;
    private float m_orgFontSize;

    private void Awake()
    {
        m_guiText = GetComponent<GUIText>();
        m_orgFontSize = m_guiText.fontSize;
    }

    private void Update()
    {
        float screenScaleX = (float)Screen.width / (float)UbhGameManager.BASE_SCREEN_WIDTH;
        float screenScaleY = (float)Screen.height / (float)UbhGameManager.BASE_SCREEN_HEIGHT;
        float screenScale = Screen.height < Screen.width ? screenScaleY : screenScaleX;

        m_guiText.fontSize = (int)(m_orgFontSize * screenScale);
    }
}
*/