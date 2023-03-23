using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void StartNewGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
