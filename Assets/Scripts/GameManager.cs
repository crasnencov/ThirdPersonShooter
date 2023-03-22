using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject BG;
    public GameObject cinemachineCam;
    public GameObject weapons;
    private void Awake()
    {
        Time.timeScale = 1f;
        Instance = this;

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
        BG.gameObject.SetActive(true);
        cinemachineCam.SetActive(false);
    }
    public enum Scene
    {
        SampleScene,
        MainMenu
    }
    
}
