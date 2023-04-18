using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;

    public void StartGame()
    {
        SceneManager.LoadScene("WillTutorial");
    }

    public void ShowCredits()
    {
        // credits pop-up window or something
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
