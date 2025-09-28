using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manumanager : MonoBehaviour
{
   public void Quitgame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void Startgame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
