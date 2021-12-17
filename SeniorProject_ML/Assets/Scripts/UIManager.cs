using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button playerInput;
    [SerializeField] private Button readyButton;
    [SerializeField] private Button machineLearning;
    [SerializeField] private Button trainButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button backToMenuButton;

    [SerializeField] private ToggleGroup toggleGroup;

    [SerializeField] private Image gameIcon;
    [SerializeField] private Image pauseBlur;
    [SerializeField] private Image toggleBlur;


    [SerializeField] private TextMeshProUGUI gameOver;
    [SerializeField] private TextMeshProUGUI gameFinished;
    [SerializeField] private TextMeshProUGUI gamePause;
    [SerializeField] private TextMeshProUGUI intelligenceText;
    [SerializeField] private TextMeshProUGUI gameTypeText;
    [SerializeField] private TextMeshProUGUI modelTrainedText;

    public AudioSource audioSource;
    public List<AudioClip> soundEffects;
    private GameManager gameManager;

    private void Awake()
    {
        audioSource.PlayOneShot(soundEffects[2]);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetInstance();
        audioSource = GetComponentInChildren<AudioSource>();
        toggleGroup = GameObject.Find("ToggleGroup").GetComponent<ToggleGroup>();

        gameOver = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        gameFinished = GameObject.Find("Win_Text").GetComponent<TextMeshProUGUI>();
        gamePause = GameObject.Find("PauseText").GetComponent<TextMeshProUGUI>();
        intelligenceText = GameObject.Find("IntelligenceText").GetComponent<TextMeshProUGUI>();
        gameTypeText = GameObject.Find("GameTypeText").GetComponent<TextMeshProUGUI>();
        modelTrainedText = GameObject.Find("ModelTrainedText").GetComponent<TextMeshProUGUI>();
        StartCoroutine(SetValues());
    }

    IEnumerator SetValues()
    {
        gameOver.gameObject.SetActive(false);
        gameFinished.gameObject.SetActive(false);
        gamePause.gameObject.SetActive(false);
        intelligenceText.gameObject.SetActive(false);
        gameTypeText.gameObject.SetActive(false);
        modelTrainedText.gameObject.SetActive(false);

        readyButton.gameObject.SetActive(false);
        playerInput.gameObject.SetActive(false);
        machineLearning.gameObject.SetActive(false);
        trainButton.gameObject.SetActive(false);

        resumeButton.gameObject.SetActive(false);
        backToMenuButton.gameObject.SetActive(false);
        gamePause.gameObject.SetActive(false);

        pauseBlur.gameObject.SetActive(false);
        toggleBlur.gameObject.SetActive(false);

        toggleGroup.gameObject.SetActive(false);
        yield return null;
    }

    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(soundEffects[0], 0.8f);
    }

    public void PlayToggleSelect()
    {
        audioSource.PlayOneShot(soundEffects[1], 0.2f);
    }
    public void Ready()
    {
        gameManager.playGame = true;
        Time.timeScale = 1.0f;
        gameManager.fpsText.gameObject.SetActive(true);
        gameManager.generationCountText.gameObject.SetActive(true);

        intelligenceText.gameObject.SetActive(false);
        gameTypeText.gameObject.SetActive(false);
        toggleGroup.gameObject.SetActive(false);
        playerInput.gameObject.SetActive(false);
        machineLearning.gameObject.SetActive(false);
        trainButton.gameObject.SetActive(false);
        readyButton.gameObject.SetActive(false);
        modelTrainedText.gameObject.SetActive(false);
        toggleBlur.gameObject.SetActive(false);
        pauseBlur.gameObject.SetActive(false);
        gameIcon.gameObject.SetActive(false);

        audioSource.Stop();

        Debug.Log("Input Detected");
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
        Time.timeScale = 1.0f;
        gameManager.playGame = true;
        pauseBlur.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        backToMenuButton.gameObject.SetActive(false);
        gamePause.gameObject.SetActive(false);

        gameManager.playGame = true;

    }

    public void ToggleNaive()
    {
        gameManager.player.iteration = 500;
        trainButton.gameObject.SetActive(true);
        modelTrainedText.gameObject.SetActive(false);
        readyButton.gameObject.SetActive(false);
    }

    public void ToggleSmart()
    {
        gameManager.player.iteration = 5000;
        trainButton.gameObject.SetActive(true);
        modelTrainedText.gameObject.SetActive(false);
        readyButton.gameObject.SetActive(false);
    }
    public void ToggleHuman()
    {
        gameManager.player.iteration = 10000;
        trainButton.gameObject.SetActive(true);
        modelTrainedText.gameObject.SetActive(false);
        readyButton.gameObject.SetActive(false);
    }

    public void DisplayGameType()
    {
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        pauseBlur.gameObject.SetActive(true);

        playerInput.gameObject.SetActive(true);
        machineLearning.gameObject.SetActive(true);
        gameTypeText.gameObject.SetActive(true);
    }

    public void PlayerInput()
    {
        readyButton.gameObject.SetActive(true);
        toggleGroup.SetAllTogglesOff(true);

        gameManager.player.isModelTrained = false;
        intelligenceText.gameObject.SetActive(false);
        trainButton.gameObject.SetActive(false);
        toggleGroup.gameObject.SetActive(false);
        toggleBlur.gameObject.SetActive(false);
    }

    public void MachineLearning()
    {
        intelligenceText.gameObject.SetActive(true);
        toggleGroup.gameObject.SetActive(true);
        toggleBlur.gameObject.SetActive(true);
        readyButton.gameObject.SetActive(false);
    }
    public void TrainModel()
    {
        GameManager.GetInstance().player.Train();
        trainButton.interactable = false;
        StartCoroutine(ReadyToGo());
    }

    IEnumerator ReadyToGo()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        readyButton.gameObject.SetActive(true);
        modelTrainedText.gameObject.SetActive(true);
        trainButton.interactable = true;
    }

    public void BackToMenu()
    {
        gameManager.ResetValues();
        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        gameIcon.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        backToMenuButton.gameObject.SetActive(false);
        gamePause.gameObject.SetActive(false);
        pauseBlur.gameObject.SetActive(false);
        gameFinished.gameObject.SetActive(false);
        audioSource.PlayOneShot(soundEffects[2]);
    }

    public void DisplayGameOver()
    {
        gameOver.gameObject.SetActive(true);
    }

    public void DisplayGameFinished()
    {
        gameFinished.gameObject.SetActive(true);
        StartCoroutine(MenuScreen());
    }

    IEnumerator MenuScreen()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        gameIcon.gameObject.SetActive(true);
        gameFinished.gameObject.SetActive(false);
        gameManager.ResetValues();
        audioSource.PlayOneShot(soundEffects[2]);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is Exiting.");
    }

}
