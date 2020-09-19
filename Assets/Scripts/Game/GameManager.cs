using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject gameOverScreen;
    public bool isGameActive;
    private PauseMenu canvas;

    void Start()
    {
        //Game is active
        isGameActive = true;
        canvas = GameObject.Find("Canvas").GetComponent<PauseMenu>();
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        isGameActive = false;
        gameOverScreen.SetActive(true);
        healthBar.gameObject.SetActive(false);
        canvas.enabled = false;
    }
}
