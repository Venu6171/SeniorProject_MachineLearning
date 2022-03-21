using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 0.0f;

    private Animator animator;
    private int isIdleHash;
    private int isRunningHash;
    private int isCollidedHash;
    private Rigidbody rigidBody;
    private Vector3 spawnLocation;

    private GameManager gameManager;
    private UIManager uiManager;

    private ML.NeuralNetwork neuralNetwork;
    private List<List<float>> inputValues;
    private List<List<float>> targetValues;
    private List<List<float>> currentInputValues;
    private List<float> outputValues;
    private List<int> topology;
    private List<float> posX;

    private float timer = 0.0f;
    private int saveValueCount = 0;
    private int randomCount = 0;
    private int maxRandomRange = 0;
    public bool isModelTrained = false;
    private bool movement = false;
    private bool isCollided = false;
    private bool isModelPlaying = false;

    private TrainingData TrainingData;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetInstance();
        uiManager = UIManager.GetInstance();
        TrainingData = new TrainingData();
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        spawnLocation = new Vector3(0.0f, 0.0500000007f, -161.830002f);

        isIdleHash = Animator.StringToHash("isIdle");
        isRunningHash = Animator.StringToHash("isRunning");
        isCollidedHash = Animator.StringToHash("isCollided");

        topology = new List<int>
        {
            9, 8, 2
        };

        neuralNetwork = new ML.NeuralNetwork(topology);
        inputValues = new List<List<float>>();
        inputValues = TrainingData.ReadInputValues(gameManager.inputValueFileName);
        targetValues = new List<List<float>>();
        targetValues = TrainingData.ReadTargetValues(gameManager.targetValueFileName);

        posX = new List<float>
        {
            rigidBody.position.x
        };

        for (int i = 0; i < gameManager.GetEnemies().Count; ++i)
        {
            posX.Add(gameManager.GetEnemies()[i].position.x);
            posX.Add(gameManager.GetEnemies()[i].velocity.x);
        }

        currentInputValues = new List<List<float>>()
        {
            posX
        };

        outputValues = new List<float> { new float(), new float() };
        maxRandomRange = TrainingData.ReadInputValues(gameManager.inputValueFileName).Count - 1;

        if (uiManager.GetInputType())
            Train();
    }

    private void Train()
    {
        StartCoroutine(TrainModel());
        Debug.Log("Model Trained.");
        isModelTrained = true;
    }

    IEnumerator TrainModel()
    {
        for (int i = 0; i < uiManager.GetModelIteration(); ++i)
        {
            randomCount = Random.Range(0, maxRandomRange);
            neuralNetwork.FeedForward(inputValues[randomCount]);
            outputValues = neuralNetwork.GetResults();
            neuralNetwork.BackPropogate(targetValues[randomCount]);
        }
        gameManager.generationCountText.gameObject.SetActive(true);
        yield return null;
    }

    private void MoveUp()
    {
        animator.SetBool(isRunningHash, true);
        animator.SetBool(isIdleHash, false);

        Vector3 upDirection = new Vector3(0.0f, 0.0f, 1.0f);
        rigidBody.position += upDirection * speed * Time.fixedDeltaTime;
    }

    private void Idle()
    {
        animator.SetBool(isIdleHash, true);
        animator.SetBool(isRunningHash, false);

        Vector3 idle = new Vector3(0.0f, 0.0f, 0.0f);
        rigidBody.position += idle * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //if (timer > 0.5f)
        //{
        //    timer = 0.0f;
        //    TrainingData.SaveInputValues(saveValueCount);

        //    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        //        TrainingData.SaveTargetValues(1.0f, 0.0f, saveValueCount);
        //    else
        //        TrainingData.SaveTargetValues(0.0f, 1.0f, saveValueCount);

        //    saveValueCount += 1;
        //}

        if (!isModelTrained)
        {
            isModelPlaying = false;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                movement = true;
            else
                movement = false;
        }

        if (isModelTrained)
        {
            isModelPlaying = true;
            if (timer > 0.5f)
            {
                timer = 0.0f;
                GetPositions();
            }
            neuralNetwork.FeedForward(currentInputValues[0]);
            outputValues = neuralNetwork.GetResults();
        }
    }

    private void FixedUpdate()
    {
        if (isModelPlaying && !isCollided)
        {
            if (outputValues[0] > outputValues[1])
            {
                MoveUp();
                Debug.Log("Moving Up");
            }
            else
            {
                Idle();
                Debug.Log("Idle");
            }
        }

        if (!isModelPlaying && !isCollided)
        {
            if (movement)
                MoveUp();
            else
                Idle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Goal")
        {
            gameManager.GameWon();
            //LevelManager.GetInstance().SpawnLevel();
            Debug.Log("You Won!");
        }

        if (other.gameObject.CompareTag("Enemies") && !isCollided)
        {
            isCollided = true;
            AudioManager.GetInstance().PlaySound(AudioManager.Sound.PlayerCollision);

            if (animator.GetBool(isRunningHash))
            {
                animator.SetBool(isCollidedHash, true);
            }

            if (animator.GetBool(isIdleHash))
            {
                animator.SetBool(isCollidedHash, true);
            }
            Invoke("Respawn", 3.02f);

            Debug.Log("Player Destroyed");
        }
    }

    private void Respawn()
    {
        isCollided = false;
        animator.SetBool(isCollidedHash, false);
        this.transform.position = spawnLocation;
        gameManager.generationCount += 1;
    }

    private void GetPositions()
    {
        currentInputValues[0][0] = rigidBody.position.x;
        currentInputValues[0][1] = gameManager.GetEnemies()[0].position.x;
        currentInputValues[0][2] = gameManager.GetEnemies()[0].velocity.x;
        currentInputValues[0][3] = gameManager.GetEnemies()[1].position.x;
        currentInputValues[0][4] = gameManager.GetEnemies()[1].velocity.x;
        currentInputValues[0][5] = gameManager.GetEnemies()[2].position.x;
        currentInputValues[0][6] = gameManager.GetEnemies()[2].velocity.x;
        currentInputValues[0][7] = gameManager.GetEnemies()[3].position.x;
        currentInputValues[0][8] = gameManager.GetEnemies()[3].velocity.x;
    }
}
