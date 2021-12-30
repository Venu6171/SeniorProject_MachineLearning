using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        AudioManager.GetInstance().PlaySound(AudioManager.Sound.MainMenu);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
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
        UIManager.GetInstance().modelIntelligence = (UIManager.IntelligenceType)intelligence;
        UIManager.GetInstance().SetModelIteration();
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
