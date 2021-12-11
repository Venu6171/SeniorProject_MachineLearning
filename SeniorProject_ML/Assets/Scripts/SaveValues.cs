using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveValues : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody playerRigidBody;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerRigidBody = gameManager.player.GetComponent<Rigidbody>();
    }

    public void SaveInputValues()
    {
        List<string> inputValues = new List<string>
        {
            "InputValues " + playerRigidBody.position.x + "," + gameManager.enemiesRigidBody[0].position.x + "," +
            +gameManager.enemiesRigidBody[0].velocity.x + "," + gameManager.enemiesRigidBody[1].position.x + "," + gameManager.enemiesRigidBody[1].velocity.x + ","
            + gameManager.enemiesRigidBody[2].position.x + "," + gameManager.enemiesRigidBody[2].velocity.x + "," + gameManager.enemiesRigidBody[3].position.x + ","
            + gameManager.enemiesRigidBody[3].velocity.x
        };

        File.WriteAllText(Application.dataPath + "/InputValues.txt", inputValues[0]);
    }

    public void SaveTargetValues(float up, float idle)
    {
        List<string> targetValues = new List<string>
        { "TargetValues " + up + "," + idle };

        File.WriteAllText(Application.dataPath + "/TargetValues.txt", targetValues[0]);
    }
}
