using UnityEngine;

/// <summary>
/// Giữ reference đến Camera và AudioListener.
///
/// Nó kiểm soát vị trí của Camera và AudioListener.
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private AudioListener audioListener;

    public Camera Camera => camera;
    public AudioListener AudioListener => audioListener;
}
