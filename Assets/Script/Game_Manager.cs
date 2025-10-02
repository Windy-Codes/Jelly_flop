using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    public GameObject pausepanel;
    public void Resetgame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausepanel.activeSelf) // If the panel is active, continue the game
            {
                ContinueGame();
            }
            else // If the panel is not active, pause the game
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pausepanel.SetActive(true);
    }

    public void ContinueGame()
    {
        pausepanel.SetActive(false);
    }
    public void maun()
    {
        SceneManager.LoadScene(0);
    }
}
