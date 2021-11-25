using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearRegression : MonoBehaviour
{
    public struct Dataset
    {
        public List<float> x0;
        public List<float> x1;
        public List<float> y;
    }

    public float b0 = 0.0f;
    public float b1 = 0.0f;
    public float learningRate = 0.01f;

    // Start is called before the first frame update

    public float Fit(Dataset dataset)
    {
        float error_b0 = 0.0f;
        float error_b1 = 0.0f;
        float mse = 0.0f;
        float m = dataset.x0.Capacity;

        for (int i = 0; i < dataset.x0.Capacity; ++i)
        {
            float predictedValue = Predict(dataset.x0[i]);
            float trueValue = dataset.y[i];
            float error = predictedValue - trueValue;

            error_b0 += error;
            error_b1 += error * dataset.x0[i];
            mse += Mathf.Sqrt(error);

        }

        b0 -= learningRate * error_b0 / m;
        b1 -= learningRate * error_b1 / m;
        mse /= m;

        return mse;
    }

    public float Predict(float X)
    {
        return b0 + (b1 * X);
    }

    public Dataset MakeLinear(int n_samples, float b0, float b1, float minX, float maxX, float noise)
    {
        Dataset dataset = new Dataset();

        List<float> newX0 = new List<float>(n_samples);
        List<float> newY = new List<float>(n_samples);

        dataset.x0 = newX0;
        dataset.y = newY;

        for (int i = 0; i < n_samples; ++i)
        {
            float x = Random.Range(minX, maxX);
            float y = b0 + (b1 * x) + Random.Range(0.0f, noise);
            dataset.x0.Add(x);
            dataset.y.Add(y);
        }
        return dataset;
    }

}
