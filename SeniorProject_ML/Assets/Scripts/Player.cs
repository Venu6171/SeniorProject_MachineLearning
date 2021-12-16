using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<AudioClip> soundEffects;
    public AudioSource sound;

    [SerializeField] private float speed = 0.0f;
    public int iteration = 0;

    private Animator animator;
    private int isIdleHash;
    private int isRunningHash;
    private int isCollidedHash;
    private Rigidbody rigidBody;
    private GameManager gameManager;
    private Vector3 spawnLocation;

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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetInstance();
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        sound = GetComponentInChildren<AudioSource>();

        spawnLocation = transform.position;

        isIdleHash = Animator.StringToHash("isIdle");
        isRunningHash = Animator.StringToHash("isRunning");
        isCollidedHash = Animator.StringToHash("isCollided");

        topology = new List<int>
        {
            9, 6, 2
        };

        neuralNetwork = new ML.NeuralNetwork(topology);
        inputValues = new List<List<float>>();
        inputValues = gameManager.ReadInputValues(gameManager.inputValueFileName);

        targetValues = new List<List<float>>();
        targetValues = gameManager.ReadTargetValues(gameManager.targetValueFileName);

        posX = new List<float>
        {
            rigidBody.position.x
        };

        for (int i = 0; i < gameManager.enemiesRigidBody.Length; ++i)
        {
            posX.Add(gameManager.enemiesRigidBody[i].position.x);
            posX.Add(gameManager.enemiesRigidBody[i].velocity.x);
        }

        currentInputValues = new List<List<float>>()
        {
            posX
        };

        outputValues = new List<float> { new float(), new float() };
        maxRandomRange = gameManager.ReadInputValues(gameManager.inputValueFileName).Count - 1;
    }

    public void Train()
    {
        StartCoroutine(TrainModel());
        Debug.Log("Model Trained.");
        isModelTrained = true;
    }

    IEnumerator TrainModel()
    {
        for (int i = 0; i < iteration; ++i)
        {
            randomCount = Random.Range(0, maxRandomRange);
            neuralNetwork.FeedForward(inputValues[randomCount]);
            outputValues = neuralNetwork.GetResults();
            neuralNetwork.BackPropogate(targetValues[randomCount]);
        }
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
        //    gameManager.SaveInputValues(saveValueCount);

        //    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        //        gameManager.SaveTargetValues(1.0f, 0.0f, saveValueCount);
        //    else
        //        gameManager.SaveTargetValues(0.0f, 1.0f, saveValueCount);

        //    saveValueCount += 1;
        //}

        if (!isModelTrained && gameManager.playGame)
        {
            isModelPlaying = false;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                movement = true;
            else
                movement = false;
        }

        if (isModelTrained && gameManager.playGame)
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
            gameManager.GameFinished();
            //this.transform.position = spawnLocation;
            Debug.Log("You Won!");
        }

        if (other.gameObject.CompareTag("Enemies") && !isCollided)
        {
            isCollided = true;
            sound.PlayOneShot(soundEffects[0]);

            if (animator.GetBool(isRunningHash))
            {
                animator.SetBool(isCollidedHash, true);
            }

            if (animator.GetBool(isIdleHash))
            {
                animator.SetBool(isCollidedHash, true);
            }
            Invoke("Respawn", 3.0f);

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
        currentInputValues[0][1] = gameManager.enemiesRigidBody[0].position.x;
        currentInputValues[0][2] = gameManager.enemiesRigidBody[0].velocity.x;
        currentInputValues[0][3] = gameManager.enemiesRigidBody[1].position.x;
        currentInputValues[0][4] = gameManager.enemiesRigidBody[1].velocity.x;
        currentInputValues[0][5] = gameManager.enemiesRigidBody[2].position.x;
        currentInputValues[0][6] = gameManager.enemiesRigidBody[2].velocity.x;
        currentInputValues[0][7] = gameManager.enemiesRigidBody[3].position.x;
        currentInputValues[0][8] = gameManager.enemiesRigidBody[3].velocity.x;
    }
}
