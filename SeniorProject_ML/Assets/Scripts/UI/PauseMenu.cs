using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuContainer;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        AudioManager.GetInstance().PauseSound();
        pauseMenuContainer.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        AudioManager.GetInstance().UnPauseSound();
        pauseMenuContainer.SetActive(false);
    }
    public void BackToMainMenu()
    {
        pauseMenuContainer.SetActive(false);
        AudioManager.GetInstance().StopSound();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
