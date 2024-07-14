using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReloadScene : MonoBehaviour
{
    public void Play()
    {
            SceneManager.LoadScene("Level1");
    }

    public void MainMenu()
    {
            SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        Application.Quit();
        
    }  
}
