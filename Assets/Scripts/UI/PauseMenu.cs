using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogPauseGame;
    
    
    public void OpenDialogPauseGame()
    {
        

        
        dialogPauseGame.SetActive(true);
        AudioController.Instance.PlaySound(AudioController.Instance.click);
        Time.timeScale = 0f; // Tiếp tục trò chơi

    }

    public void CloseDialogPauseGame()
    {
        dialogPauseGame.SetActive(false);
        AudioController.Instance.PlaySound(AudioController.Instance.click);
        Time.timeScale = 1f; // Tiếp tục trò chơi

    }

    public void Home()
    {
        SceneManager.LoadScene("Lobby");
        AudioController.Instance.PlaySound(AudioController.Instance.click);
        Time.timeScale = 1f; // Tiếp tục trò chơi
        

    }
}
