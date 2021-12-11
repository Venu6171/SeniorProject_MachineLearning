using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 0.0f;
    [SerializeField] private int iteration = 0;

    private Rigidbody rigidBody;
    private GameManager gameManager;
    private Vector3 spawnLocation = new Vector3(0.0f, 2.3f, -105.0f);
    private Vector3 velocity;

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
    private bool trained = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

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

        Train();
    }

    private void Train()
    {
        for (var i = 0; i < iteration; ++i)
        {
            randomCount = Random.Range(0, gameManager.ReadInputValues(gameManager.inputValueFileName).Count - 1);
            neuralNetwork.FeedForward(inputValues[randomCount]);
            outputValues = neuralNetwork.GetResults();
            neuralNetwork.BackPropogate(targetValues[randomCount]);
            Debug.Log("Training Model");
        }
        trained = true;
    }

    private void MoveUp()
    {
        Vector3 upDirection = new Vector3(0.0f, 0.0f, 1.0f);
        rigidBody.position += upDirection * speed * Time.fixedDeltaTime;
    }

    //private float MoveLeft()
    //{
    //    return -1.0f;
    //}
    //
    //private float MoveRight()
    //{
    //    return 1.0f;
    //}

    private void Idle()
    {
        Vector3 idle = new Vector3(0.0f, 0.0f, 0.0f);
        rigidBody.position += idle * Time.fixedDeltaTime;
    }


    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;

        //if (timer > 0.5f && saveValueCount < gameManager.maxSaveCount)
        //{
        //    timer = 0.0f;
        //    gameManager.SaveInputValues(saveValueCount);

        //    if (Input.GetKey(KeyCode.UpArrow))
        //        gameManager.SaveTargetValues(1.0f, 0.0f, saveValueCount);
        //    else
        //        gameManager.SaveTargetValues(0.0f, 1.0f, saveValueCount);

        //    saveValueCount += 1;
        //}

        //Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        //velocity = moveInput.normalized * speed;

        StartCoroutine(Wait());

        if (trained)
        {
            GetPositions();

            neuralNetwork.FeedForward(currentInputValues[0]);
            outputValues = neuralNetwork.GetResults();

            if (outputValues[0] > outputValues[1])
            {
                MoveUp();
                Debug.Log("Moving Up");
            }
            else
            {
                Idle();
                Debug.Log("Doing Nothing");
            }
        }
    }

    private void FixedUpdate()
    {
        //rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Enemies"))
        //{
        //    //Instantiate(this, spawnLocation, Quaternion.identity);
        //    //Destroy(this);
        //    //this.gameObject.SetActive(false);
        //    this.transform.position = spawnLocation;
        //    gameManager.generationCount += 1;
        //    Debug.Log("Player Destroyed");
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Goal")
        {
            //gameManager.GameFinished();
            this.transform.position = spawnLocation;
            gameManager.generationCount += 1;
            Debug.Log("You Won!");
        }

        if (other.gameObject.name == "Cube_L" ||
            other.gameObject.name == "Cube_R" || other.gameObject.CompareTag("Enemies"))
        {
            //Instantiate(this, spawnLocation, Quaternion.identity);
            //Destroy(this);
            //this.gameObject.SetActive(false);
            this.transform.position = spawnLocation;
            gameManager.generationCount += 1;
            Debug.Log("Player Destroyed");
        }
    }

    private void GetPositions()
    {
        timer += Time.deltaTime;

        if (timer > 0.5f)
        {
            timer = 0.0f;
            currentInputValues[0][0] = 0.0f;
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

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5.0f);
    }
}
