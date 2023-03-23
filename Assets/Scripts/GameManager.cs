using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject BG;
    public GameObject cinemachineCam;
    public GameObject weapons;
    public Text wonTxt, loseTxt;
    public Text endGameTxt;
    public GameObject enemies;
    private int winCount, loseCount;
    private Transform[] allEnemies;
    private int enemiesCount;

    private void Awake()
    {
        Time.timeScale = 1f;
        Instance = this;

        allEnemies = enemies.GetComponentsInChildren<Transform>();
        enemiesCount = allEnemies.Length;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Lose"))
        {
            loseCount = PlayerPrefs.GetInt("Lose");
            Debug.Log("losecount " + loseCount);
        }

        if (PlayerPrefs.HasKey("Win"))
        {
            winCount = PlayerPrefs.GetInt("Win");
            Debug.Log("winCount " + winCount);
        }
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(Scene.SampleScene.ToString());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(Scene.MainMenu.ToString());
    }

    public void GameOver()
    {
        // Time.timeScale = 0f;
        // BG.gameObject.SetActive(true);

        // weapons.SetActive(false);
        loseCount += 1;
        PlayerPrefs.SetInt("Lose", loseCount);
        
        endGameTxt.text = "You lose!";
        Endgame();
    }

    public void EnemyKilled()
    {
        enemiesCount--;
        if (enemiesCount <= allEnemies.Length)
        {
            Win();
        }
    }

    private void Win()
    {
        winCount += 1;
        PlayerPrefs.SetInt("Win", winCount);
        
        endGameTxt.text = "You win!";
        Endgame();
    }

    private void Endgame()
    {
        BG.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        cinemachineCam.SetActive(false);
        loseTxt.text = "Lose " + loseCount + " Times";
        wonTxt.text = "Win " + winCount + " Times";
        Time.timeScale = 0.1f;
    }

    public enum Scene
    {
        SampleScene,
        MainMenu
    }
}