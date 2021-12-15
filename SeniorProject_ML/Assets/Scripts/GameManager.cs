using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager GetInstance()
    {
        return instance;
    }

    private AudioSource trafficNoise;

    [SerializeField] public Player player;
    private Vector3 playerSpawnLocation;
    private Rigidbody playerRigidBody;
    [SerializeField] public Rigidbody[] enemiesRigidBody;
    [SerializeField] private CameraController mainCamera;
    [SerializeField] public TextMeshProUGUI fpsText;
    [SerializeField] public TextMeshProUGUI generationCountText;

    private List<Vector3> enemyPositions;

    private float fps = 0.0f;
    public int generationCount = 0;
    private UIManager uiManager;
    private Vector3 viewPlayerPosition;

    private List<List<string>> inputString;
    private List<List<string>> targetString;
    private List<string> inputValues;
    private List<string> targetValues;

    public string inputValueFileName;
    public string targetValueFileName;

    public int maxSaveCount = 0;
    public bool playGame = false;

    private void Awake()
    {
        Time.timeScale = 0.0f;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("Game Manager Instance created");
        }
        else
        {
            Destroy(this.gameObject);
            Debug.Log("Game Manager Destroyed");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Alien").GetComponent<Player>();
        playerSpawnLocation = player.GetComponent<Transform>().position;
        playerRigidBody = player.GetComponent<Rigidbody>();

        trafficNoise = GetComponent<AudioSource>();

        for (int i = 0; i < enemiesRigidBody.Length; ++i)
            enemiesRigidBody[i] = enemiesRigidBody[i].GetComponent<Rigidbody>();

        enemyPositions = new List<Vector3>();
        for (int i = 0; i < enemiesRigidBody.Length; ++i)
            enemyPositions.Add(enemiesRigidBody[i].position);

        mainCamera = GameObject.Find("Main Camera").GetComponent<CameraController>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        fpsText = GameObject.Find("FPS").GetComponent<TextMeshProUGUI>();
        generationCountText = GameObject.Find("GenerationCountText").GetComponent<TextMeshProUGUI>();

        fpsText.gameObject.SetActive(true);
        generationCountText.gameObject.SetActive(true);

        inputString = new List<List<string>>(maxSaveCount);
        targetString = new List<List<string>>(maxSaveCount);

    }

    // Update is called once per frame
    void Update()
    {
        if (playGame)
        {
            if (!trafficNoise.isPlaying)
                trafficNoise.Play();

            fps = 1.0f / Time.deltaTime;
            fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString() + "fps";

            if (player.isModelTrained)
                generationCountText.text = "Generation: " + generationCount.ToString();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            trafficNoise.Pause();
            uiManager.PauseGame();
        }
    }

    void FixedUpdate()
    {
        // viewPlayerPosition = mainCamera.GetComponent<Camera>().WorldToViewportPoint(player.transform.position);
        // if (viewPlayerPosition.z < 0.0f)
        //     DestroyPlayer();

        //if (viewPlayerPosition.y >= 0.5)
        //    mainCamera.speed = 2.5f;
        //else if (viewPlayerPosition.y <= 0.5f && viewPlayerPosition.y > 0)
        //    mainCamera.speed = 1.0f;
    }

    public void ResetValues()
    {
        player.transform.position = playerSpawnLocation;

        for (int i = 0; i < enemiesRigidBody.Length; ++i)
            enemiesRigidBody[i].position = enemyPositions[i];

        generationCount = 0;

        fpsText.gameObject.SetActive(false);
        generationCountText.gameObject.SetActive(false);
        player.isModelTrained = false;
    }

    public void DestroyPlayer()
    {
        player.gameObject.SetActive(false);
        uiManager.DisplayGameOver();
        Time.timeScale = 0.0f;
    }

    public void GameFinished()
    {
        Time.timeScale = 0.0f;
        uiManager.DisplayGameFinished();
        playGame = false;
    }

    public void SaveInputValues(int i)
    {
        inputValues = new List<string> {
            playerRigidBody.position.x + "," + enemiesRigidBody[0].position.x + "," +
            +enemiesRigidBody[0].velocity.x + "," + enemiesRigidBody[1].position.x + "," + enemiesRigidBody[1].velocity.x + ","
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
