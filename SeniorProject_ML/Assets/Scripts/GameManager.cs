using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Enemies[] enemies;
    [SerializeField] private CameraController mainCamera;
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
            Enemies all_enemies = GameObject.FindGameObjectWithTag("Enemies").GetComponent<Enemies>();
            enemies[i] = all_enemies;
        }
        mainCamera = GameObject.Find("Main Camera").GetComponent<CameraController>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        viewPlayerPosition = mainCamera.GetComponent<Camera>().WorldToViewportPoint(player.transform.position);
        if (viewPlayerPosition.z < 0)
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

    public void DestroyEnemy()
    {
        foreach (Enemies enemy in enemies)
        {
            Destroy(enemy);
            enemy.gameObject.SetActive(false);
        }
    }
}
