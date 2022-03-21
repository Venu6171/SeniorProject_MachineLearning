using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    public static GameManager GetInstance()
    {
        return Instance;
    }

    private PauseMenu pauseMenu;
    private GameObject displayMenu;

    public static Player player;
    private Vector3 playerSpawnLocation;

    public static List<Rigidbody> enemiesRigidBody;
    private List<Vector3> enemyPositions;

    public string inputValueFileName;
    public string targetValueFileName;

    public int generationCount = 0;
    public int maxSaveCount = 0;

    public TextMeshProUGUI generationCountText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI gameWonText;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        AudioManager.GetInstance().PlaySound(AudioManager.Sound.Traffic);

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Game_Manager Instance created");
        }
        else
        {
            Destroy(gameObject);
            Destroy(player);
            enemiesRigidBody.Clear();
            Debug.Log("Game_Manager duplicate Destroyed");
        }

        player = GameObject.Find("Alien").GetComponent<Player>();

        enemiesRigidBody = new List<Rigidbody>();
        enemyPositions = new List<Vector3>();
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemies"))
        {
            enemiesRigidBody.Add(enemy.GetComponent<Rigidbody>());
            enemyPositions.Add(enemy.transform.position);
        }

        pauseMenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        DontDestroyOnLoad(pauseMenu);

        gameOverText.gameObject.SetActive(false);
        gameWonText.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSpawnLocation = player.GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.PauseGame();
        }

        //if (!AudioManager.GetInstance().IsPlaying(AudioManager.Sound.Traffic) && !AudioManager.GetInstance().isPaused)
        //    AudioManager.GetInstance().PlaySound(AudioManager.Sound.Traffic);
    }

    public List<Rigidbody> GetEnemies()
    {
        return enemiesRigidBody;
    }

    public void ResetValues()
    {
        player.transform.position = playerSpawnLocation;

        for (int i = 0; i < enemiesRigidBody.Count; ++i)
            enemiesRigidBody[i].position = enemyPositions[i];

        generationCount = 0;
        generationCountText.gameObject.SetActive(false);
    }

    public void DestroyPlayer()
    {
        player.gameObject.SetActive(false);
        UIManager.GetInstance().DisplayGameOver();
        Time.timeScale = 0.0f;
    }

    public void GameWon()
    {
        Time.timeScale = 0.0f;
        StartCoroutine(BackToMenu());
        UIManager.GetInstance().DisplayGameWon();
    }

    IEnumerator BackToMenu()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        pauseMenu.BackToMainMenu();
    }

}