using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuContainer;

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
        LevelManager.GetInstance().ReleaseObjects();
        GameManager.GetInstance().ResetValues();
        AudioManager.GetInstance().StopSound();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
    }
}