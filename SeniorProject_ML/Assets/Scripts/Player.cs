using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 0.0f;
    [SerializeField] private float b0 = 0.0f;
    [SerializeField] private float b1 = 0.0f;
    [SerializeField] private float minX = 0.0f;
    [SerializeField] private float maxX = 0.0f;
    [SerializeField] private float noise = 0.0f;
    [SerializeField] private float learningRate = 0.0f;
    [SerializeField] private int n_samples = 0;
    [SerializeField] private int cycles = 0;

    private Rigidbody rigidBody;
    private Vector3 velocity;
    private GameManager gameManager;
    private LinearRegression linReg;
    private LinearRegression.Dataset dataset;

    private float error = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        linReg = GetComponent<LinearRegression>();

        b0 = 200.0f;
        b1 = -1.2f;
        minX = 0.0f;
        maxX = 100.0f;
        noise = 1.0f;
        learningRate = 0.0005f;
        n_samples = 100;
        cycles = 100;

        dataset = linReg.MakeLinear(n_samples, b0, b1, minX, maxX, noise);
        linReg.learningRate = learningRate;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        velocity = moveInput.normalized * speed;

        //for (int i = 0; i < cycles; ++i)
        //{
        //    error = linReg.Fit(dataset);
        //}
        //
        //float startX = minX;
        //float endX = maxX;
        //float startZ = linReg.Predict(startX);
        //float endZ = linReg.Predict(endX);
        //
        //List<Vector3> position = new List<Vector3>
        //{
        //    new Vector3(startX, 0.0f, startZ),
        //    new Vector3(endX, 0.0f, endZ)
        //};
        //
        //Vector3 moveInput = Vector3.zero;
        //
        //for (int i = 0; i < position.Capacity; ++i)
        //    moveInput = new Vector3(position[i].x, 0.0f, position[i].z);
        //
        //velocity = moveInput.normalized * speed;
    }

    void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);
        //rigidBody.MovePosition(rigidBody.position + velocity * error * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemies"))
        {
            gameManager.DestroyPlayer();
            Debug.Log("Player Destroyed");
        }

        if (collision.gameObject.name == "Goal")
        {
            gameManager.GameFinished();
            Debug.Log("You Won!");
        }
    }
}
