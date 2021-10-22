using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Image gameIcon;
    [SerializeField] private Text gameOver;


    private void Awake()
    {
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
        playButton.gameObject.SetActive(false);
        gameIcon.gameObject.SetActive(false);
    }

    public void DisplayGameOver()
    {
        gameOver.gameObject.SetActive(true);
    }
}
