using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Image gameIcon;
    [SerializeField] private TextMeshProUGUI gameOver;

    private void Awake()
    {
        //gameOver.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideUI()
    {
        Time.timeScale = 1.0f;
        playButton.SetEnabled(false);
        
    }

    public void DisplayGameOver()
    {
        gameOver.gameObject.SetActive(true);
    }
}
