using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public static String winner;

    public TextMeshProUGUI TextMeshProUGUI;

    private void Start()
    {
        TextMeshProUGUI.SetText(winner);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
