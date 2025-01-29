using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndedMenu : MonoBehaviour
{
    public void PlayAgain()
    {
        // �u anki sahneyi yeniden y�kle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMainMenu()
    {
        // Ana men� sahnesine ge�i� yap
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        // Oyundan ��k
        Debug.Log("Game is quitting...");
        Application.Quit();

        // E�er oyun sahnede �al���yorsa, bunu test etmek i�in:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
