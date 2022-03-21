using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.GetInstance().PlaySound(AudioManager.Sound.MainMenu);
    }
    // Start is called before the first frame update

    public void PlayButtonClick()
    {
        AudioManager.GetInstance().PlaySound(AudioManager.Sound.ButtonClick);
    }

    public void PlayToggleSelect()
    {
        AudioManager.GetInstance().PlaySound(AudioManager.Sound.ToggleSelect);
    }

    public void LoadScene()
    {
        AudioManager.GetInstance().StopSound();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Input Detected");
    }

    public void SetModelIntelligence(int intelligence)
    {
        UIManager.GetInstance().SetModelIteration(intelligence);
    }

    public void InputType(bool type)
    {
        UIManager.GetInstance().isModel = type;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
