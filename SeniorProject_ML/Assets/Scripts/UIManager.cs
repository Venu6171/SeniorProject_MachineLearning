using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Image gameIcon;
    [SerializeField] private TextMeshProUGUI gameOver;
    [SerializeField] private TextMeshProUGUI gameFinished;

    public bool playGame = false;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        gameFinished = GameObject.Find("Win_Text").GetComponent<TextMeshProUGUI>();

        gameOver.gameObject.SetActive(false);
        gameFinished.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideUI()
    {
        Time.timeScale = 1.0f;
        playButton.gameObject.SetActive(false);
        gameIcon.gameObject.SetActive(false);
        Debug.Log("Input Detected");
        playGame = true;
    }

    public void DisplayGameOver()
    {
        gameOver.gameObject.SetActive(true);
    }

    public void DisplayGameFinished()
    {
        gameFinished.gameObject.SetActive(true);
    }
}
