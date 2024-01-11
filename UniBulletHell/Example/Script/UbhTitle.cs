using UnityEngine;
using UnityEngine.UI;

public class UbhTitle : UbhMonoBehaviour
{
    private const string TITLE_PC = "Press X";
    private const string TITLE_MOBILE = "Tap To Start";

    [SerializeField]
    private Text m_startText = null;

    private void Start()
    {
        m_startText.text = UbhUtil.IsMobilePlatform() ? TITLE_MOBILE : TITLE_PC;
    }
}