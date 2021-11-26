using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Enemies[] enemies;
    [SerializeField] private CameraController mainCamera;
    [SerializeField] private TextMeshProUGUI fpsText;
    
    private UIManager uiManager;
    private Vector3 viewPlayerPosition;

    private void Awake()
    {
        Time.timeScale = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        for (int i = enemies.Length - 1; i <= 0; --i)
        {
            enemies[i] = GameObject.Find("Enemies").GetComponentInChildren<Enemies>();
        }
        mainCamera = GameObject.Find("Main Camera").GetComponent<CameraController>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        fpsText = GameObject.Find("FPS").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (uiManager.playGame && Time.timeScale == 1.0f)
        {
            float fps = 1.0f / Time.deltaTime;
            fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString() + "fps";
        }
    }

    void FixedUpdate()
    {
        viewPlayerPosition = mainCamera.GetComponent<Camera>().WorldToViewportPoint(player.transform.position);
        if (viewPlayerPosition.z < 0.0f)
            DestroyPlayer();

        //if (viewPlayerPosition.y >= 0.5)
        //    mainCamera.speed = 2.5f;
        //else if (viewPlayerPosition.y <= 0.5f && viewPlayerPosition.y > 0)
        //    mainCamera.speed = 1.0f;
    }

    public void DestroyPlayer()
    {
        Destroy(player);
        player.gameObject.SetActive(false);
        uiManager.DisplayGameOver();
        Time.timeScale = 0.0f;
    }

    public void GameFinished()
    {
        uiManager.DisplayGameFinished();
        Time.timeScale = 0.0f;
    }
}
