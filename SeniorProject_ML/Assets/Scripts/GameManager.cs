using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Player player;
    private Rigidbody playerRigidBody;
    [SerializeField] public Rigidbody[] enemiesRigidBody;
    [SerializeField] private CameraController mainCamera;
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private TextMeshProUGUI generationCountText;

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

    private void Awake()
    {
        Time.timeScale = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Actor").GetComponent<Player>();
        playerRigidBody = player.GetComponent<Rigidbody>();

        for (int i = 0; i < enemiesRigidBody.Length; ++i)
            enemiesRigidBody[i] = enemiesRigidBody[i].GetComponent<Rigidbody>();

        mainCamera = GameObject.Find("Main Camera").GetComponent<CameraController>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        fpsText = GameObject.Find("FPS").GetComponent<TextMeshProUGUI>();
        generationCountText = GameObject.Find("GenerationCountText").GetComponent<TextMeshProUGUI>();

        inputString = new List<List<string>>(maxSaveCount);
        targetString = new List<List<string>>(maxSaveCount);

    }

    // Update is called once per frame
    void Update()
    {
        if (uiManager.playGame && Time.timeScale == 1.0f)
        {
            fps = 1.0f / Time.deltaTime;
            fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString() + "fps";

            generationCountText.text = "Generation: " + generationCount.ToString();
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

    public void DestroyPlayer()
    {
        player.gameObject.SetActive(false);
        uiManager.DisplayGameOver();
        Time.timeScale = 0.0f;
    }

    public void GameFinished()
    {
        uiManager.DisplayGameFinished();
        Time.timeScale = 0.0f;
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

        using StreamWriter input = File.AppendText(Application.dataPath + "/" + inputValueFileName);
        input.WriteLine(inputString[i][0]);
    }

    public List<List<float>> ReadInputValues(string filePath)
    {
        List<List<float>> inputFloats = new List<List<float>>(maxSaveCount);

        if (filePath == null)
            return inputFloats;

        using StreamReader readInput = new StreamReader(Application.dataPath + "/" + filePath);
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

        using StreamWriter target = File.AppendText(Application.dataPath + "/" + targetValueFileName);
        target.WriteLine(targetString[i][0]);
    }

    public List<List<float>> ReadTargetValues(string filePath)
    {
        List<List<float>> targetFloats = new List<List<float>>(maxSaveCount);

        if (filePath == null)
            return targetFloats;

        using StreamReader readInput = new StreamReader(Application.dataPath + "/" + filePath);
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
