using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndedMenu : MonoBehaviour
{
    public void PlayAgain()
    {
        // Þu anki sahneyi yeniden yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMainMenu()
    {
        // Ana menü sahnesine geçiþ yap
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        // Oyundan çýk
        Debug.Log("Game is quitting...");
        Application.Quit();

        // Eðer oyun sahnede çalýþýyorsa, bunu test etmek için:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
