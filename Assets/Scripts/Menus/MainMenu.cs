using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button storyMode;
    public Button endlessMode;
    public Button options;
    public Button exit;

    public void StartStory()
    {
        SceneManager.LoadScene("Chapter1-1");
    }
    public void StartEndless()
    {

    }
    public void StartOptions()
    {

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
