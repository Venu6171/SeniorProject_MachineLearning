using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TrainingData
{
    public List<List<string>> inputString;
    public List<List<string>> targetString;
    public List<string> inputValues;
    public List<string> targetValues;

    public void SaveInputValues(int i)
    {
        inputValues = new List<string> {
            GameManager.player.GetComponent<Rigidbody>().position.x + "," + GameManager.enemiesRigidBody[0].position.x + "," +
            + GameManager.enemiesRigidBody[0].velocity.x + "," + GameManager.enemiesRigidBody[1].position.x + "," + GameManager.enemiesRigidBody[1].velocity.x + ","
            + GameManager.enemiesRigidBody[2].position.x + "," + GameManager.enemiesRigidBody[2].velocity.x + "," + GameManager.enemiesRigidBody[3].position.x + ","
            + GameManager.enemiesRigidBody[3].velocity.x
        };

        inputString.Add(inputValues);

        using StreamWriter input = File.AppendText(Application.streamingAssetsPath + "/" + GameManager.GetInstance().inputValueFileName);
        input.WriteLine(inputString[i][0]);
    }

    public List<List<float>> ReadInputValues(string filePath)
    {
        List<List<float>> inputFloats = new List<List<float>>(GameManager.GetInstance().maxSaveCount);

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

        using StreamWriter target = File.AppendText(Application.streamingAssetsPath + "/" + GameManager.GetInstance().targetValueFileName);
        target.WriteLine(targetString[i][0]);
    }

    public List<List<float>> ReadTargetValues(string filePath)
    {
        List<List<float>> targetFloats = new List<List<float>>(GameManager.GetInstance().maxSaveCount);

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
