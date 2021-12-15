using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button trainButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button backToMenuButton;

    [SerializeField] private Image gameIcon;
    [SerializeField] private Image pauseBlur;

    [SerializeField] private TextMeshProUGUI gameOver;
    [SerializeField] private TextMeshProUGUI gameFinished;
    [SerializeField] private TextMeshProUGUI gamePause;

    private AudioSource buttonClick;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetInstance();

        buttonClick = GetComponent<AudioSource>();

        gameOver = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        gameFinished = GameObject.Find("Win_Text").GetComponent<TextMeshProUGUI>();
        gamePause = GameObject.Find("PauseText").GetComponent<TextMeshProUGUI>();

        gameOver.gameObject.SetActive(false);
        gameFinished.gameObject.SetActive(false);
        gamePause.gameObject.SetActive(false);

        resumeButton.gameObject.SetActive(false);
        backToMenuButton.gameObject.SetActive(false);
        gamePause.gameObject.SetActive(false);
        pauseBlur.gameObject.SetActive(false);
    }

    public void PlaySound()
    {
        buttonClick.Play();
    }

    public void HideUI()
    {
        Time.timeScale = 1.0f;
        gameManager.fpsText.gameObject.SetActive(true);
        gameManager.generationCountText.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        trainButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        gameIcon.gameObject.SetActive(false);
        Debug.Log("Input Detected");
        gameManager.playGame = true;

    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        pauseBlur.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(true);
        backToMenuButton.gameObject.SetActive(true);
        gamePause.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        if (gameManager.playGame)
        {
            Time.timeScale = 1.0f;
            gameManager.playGame = true;
            pauseBlur.gameObject.SetActive(false);
            resumeButton.gameObject.SetActive(false);
            backToMenuButton.gameObject.SetActive(false);
            gamePause.gameObject.SetActive(false);
        }

    }

    public void BackToMenu()
    {
        gameManager.playGame = false;
        gameManager.ResetValues();
        playButton.gameObject.SetActive(true);
        trainButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        gameIcon.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        backToMenuButton.gameObject.SetActive(false);
        gamePause.gameObject.SetActive(false);
        pauseBlur.gameObject.SetActive(false);
        gameFinished.gameObject.SetActive(false);
    }

    public void DisplayGameOver()
    {
        gameOver.gameObject.SetActive(true);
    }

    public void DisplayGameFinished()
    {
        gameFinished.gameObject.SetActive(true);
    }

    public void TrainModel()
    {
        GameManager.GetInstance().player.Train();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is Exiting.");
    }

}
