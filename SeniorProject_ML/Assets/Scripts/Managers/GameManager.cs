using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    public static GameManager GetInstance()
    {
        return Instance;
    }

    private PauseMenu pauseMenu;

    private static Player player;
    private Vector3 playerSpawnLocation;

    private TextMeshProUGUI generationCountText;
    private TextMeshProUGUI gameOverText;
    private TextMeshProUGUI gameWonText;

    private static List<Rigidbody> enemiesRigidBody;
    private List<Vector3> enemyPositions;
    private List<List<string>> inputString;
    private List<List<string>> targetString;
    private List<string> inputValues;
    private List<string> targetValues;

    public string inputValueFileName;
    public string targetValueFileName;
    public int generationCount = 0;
    public int maxSaveCount = 0;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSpawnLocation = player.GetComponent<Transform>().position;

        //generationCountText = GameObject.Find("DisplayText").GetComponentInChildren<TextMeshProUGUI>();
        //gameOverText = GameObject.Find("DisplayText").GetComponentInChildren<TextMeshProUGUI>();
        //gameWonText = GameObject.Find("DisplayText").GetComponentInChildren<TextMeshProUGUI>();

        //if (player.isModelTrained)
        //    generationCountText.gameObject.SetActive(true);
        //else
        //    generationCountText.gameObject.SetActive(false);
        //
        //gameOverText.gameObject.SetActive(false);
        //gameWonText.gameObject.SetActive(false);

        inputString = new List<List<string>>();
        targetString = new List<List<string>>();

    }

    // Update is called once per frame
    void Update()
    {
        //if (player.isModelTrained)
        //    generationCountText.text = "Generation: " + generationCount.ToString();

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.PauseGame();
        }
    }

    public List<Rigidbody> GetEnemies()
    {
        return enemiesRigidBody;
    }

    void FixedUpdate()
    {

    }

    public void ResetValues()
    {
        player.transform.position = playerSpawnLocation;

        for (int i = 0; i < enemiesRigidBody.Count; ++i)
            enemiesRigidBody[i].position = enemyPositions[i];

        generationCount = 0;
        generationCountText.gameObject.SetActive(false);
        player.isModelTrained = false;
    }

    public void DestroyPlayer()
    {
        player.gameObject.SetActive(false);
        DisplayGameOver();
        Time.timeScale = 0.0f;
    }

    public void GameWon()
    {
        Time.timeScale = 0.0f;
        DisplayGameWon();
    }

    public void DisplayGameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }

    public void DisplayGameWon()
    {
        gameWonText.gameObject.SetActive(true);
    }

    public void SaveInputValues(int i)
    {
        inputValues = new List<string> {
            player.GetComponent<Rigidbody>().position.x + "," + enemiesRigidBody[0].position.x + "," +
            + enemiesRigidBody[0].velocity.x + "," + enemiesRigidBody[1].position.x + "," + enemiesRigidBody[1].velocity.x + ","
            + enemiesRigidBody[2].position.x + "," + enemiesRigidBody[2].velocity.x + "," + enemiesRigidBody[3].position.x + ","
            + enemiesRigidBody[3].velocity.x
        };

        inputString.Add(inputValues);

        using StreamWriter input = File.AppendText(Application.streamingAssetsPath + "/" + inputValueFileName);
        input.WriteLine(inputString[i][0]);
    }

    public List<List<float>> ReadInputValues(string filePath)
    {
        List<List<float>> inputFloats = new List<List<float>>(maxSaveCount);

        if (filePath == null)
            return inputFloats;

        using StreamReader readInput = new StreamReader(Application.streamingAssetsPath + "/" + filePath);
        string data = readInput.ReadLine();
        while (data != null)
        {
            List<float> readInputFloats = new List<float>();
            string[] inputString = data.Split(',');

            for (int i = 0; i < inputString.Length; ++i)
                readInputFloats.Add(float.Parse(inputString[i]));

            inputFloats.Add(readInputFloats);

            data = readInput.ReadLine();
        }

        return inputFloats;
    }
    public void SaveTargetValues(float up, float idle, int i)
    {
        targetValues = new List<string>
        { + up + "," + idle };

        targetString.Add(targetValues);

        using StreamWriter target = File.AppendText(Application.streamingAssetsPath + "/" + targetValueFileName);
        target.WriteLine(targetString[i][0]);
    }

    public List<List<float>> ReadTargetValues(string filePath)
    {
        List<List<float>> targetFloats = new List<List<float>>(maxSaveCount);

        if (filePath == null)
            return targetFloats;

        using StreamReader readInput = new StreamReader(Application.streamingAssetsPath + "/" + filePath);
        string data = readInput.ReadLine();
        while (data != null)
        {
            List<float> readTargetFloats = new List<float>();
            string[] inputString = data.Split(',');

            for (int i = 0; i < inputString.Length; ++i)
                readTargetFloats.Add(float.Parse(inputString[i]));

            targetFloats.Add(readTargetFloats);

            data = readInput.ReadLine();
        }

        return targetFloats;
    }
}
