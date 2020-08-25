using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI waveText;
    public GameObject healthBar;
    public bool isGameActive;

    void Start()
    {
        //Game is active
        isGameActive = true;
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverText.gameObject.SetActive(true);
        waveText.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(false);
    }
}
