using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime;

namespace ML
{
    using Layer = List<Neuron>;
    public struct Connection
    {
        public float Weight { get; set; }

        public Connection(float weight)
        {
            this.Weight = weight;
        }
    }

    public class Neuron
    {
        public float TanH(float x)
        {
            return Mathf.Tan(x);
        }

        public float TanHDerivate(float x)
        {
            return 1.0f - (x * x);
        }

        // This works better for Digit
        public float Sigmoid(float x)
        {
            return 1 / (1 + Mathf.Exp(-x));
        }

        public float SigmoidDerivative(float x)
        {
            float output = Sigmoid(x);
            return output * (1 - output);
        }

        private List<Connection> mOutPutWeights = new List<Connection>();
        private int mMyIndex = 0;
        private float mOutputValue = 0.0f;
        private float mGradient = 0.0f;

        public Neuron(int numOutputs, int myIndex)
        {
            this.mMyIndex = myIndex;

            for (var i = 0; i < numOutputs; ++i)
            {
                Connection connection = new Connection();
                connection.Weight = Random.Range(0.0f, 1.0f);
                mOutPutWeights.Add(connection);
            }
        }

        public void SetOutputValue(float value) { mOutputValue = value; }
        public float GetOutputValue() { return mOutputValue; }
        public void FeedForward(Layer previousLayer)
        {
            float sum = 0.0f;
            for (var neuron = 0; neuron < previousLayer.Count; ++neuron)
            {
                var n = previousLayer[neuron];
                sum += n.GetOutputValue() * n.mOutPutWeights[mMyIndex].Weight;
            }
            mOutputValue = Sigmoid(sum);
        }
        public void CalculateOutputGradients(float targettValue)
        {
            float error = targettValue - mOutputValue;
            mGradient = error * SigmoidDerivative(mOutputValue);
        }
        public void CalculateHiddenGradients(Layer nextLayer)
        {
            float sumDOW = 0.0f;

            for (var neuron = 0; neuron + 1 < nextLayer.Count; ++neuron)
                sumDOW += mOutPutWeights[neuron].Weight * nextLayer[neuron].mGradient;

            mGradient = sumDOW * SigmoidDerivative(mOutputValue);
        }
        public void UpdateInputWeights(Layer previousLayer)
        {
            const float learningRate = 0.15f;

            for (var neuron = 0; neuron < previousLayer.Count; ++neuron)
            {
                var n = previousLayer[neuron];
                float deltaWeight = learningRate * n.GetOutputValue() * mGradient;

                Connection connection = new Connection();
                connection.Weight += deltaWeight;
                n.mOutPutWeights.Add(connection);
            }
        }
    }
    public class NeuralNetwork
    {
        private List<Layer> mLayers = new List<Layer>();
        public NeuralNetwork(List<int> topology)
        {
            var numLayers = topology.Count;
            Debug.Assert(numLayers >= 2, "NeuralNetwork - Invalid Topology, must have atleast 2 layers");

            mLayers = new List<Layer>(numLayers);

            for (var layer = 0; layer < numLayers; ++layer)
            {
                int numOutputs = (layer + 1 == numLayers) ? 0 : topology[layer + 1];
                int numNeurons = topology[layer];

                mLayers.Add(new Layer(layer));

                for (var neuron = 0; neuron <= numNeurons; ++neuron)
                {
                    //mLayers[layer].emplace_back(numOutputs, neuron);
                    mLayers[layer].Add(new Neuron(numOutputs, neuron));
                }

                //mLayers[layer].back().SetOutputValue(1.0f);
                mLayers[layer][mLayers[layer].Count - 1].SetOutputValue(1.0f);
            }
        }
        public void FeedForward(List<float> inputValues)
        {
            Debug.Assert(inputValues.Count == mLayers[0].Count - 1, "NeuralNetwork -- Invalid input count");

            // Assign the input values to the input layer neurons
            for (var i = 0; i < inputValues.Count; ++i)
                mLayers[0][i].SetOutputValue(inputValues[i]);

            // Forward Propagate
            for (var layer = 0; layer + 1 < mLayers.Count; ++layer)
            {
                var currentLayer = mLayers[layer];
                var nextLayer = mLayers[layer + 1];

                for (var neuron = 0; neuron + 1 < nextLayer.Count; ++neuron)
                    nextLayer[neuron].FeedForward(currentLayer);
            }
        }
        public void BackPropogate(List<float> targetValues)
        {
            Debug.Assert(targetValues.Count + 1 == mLayers[mLayers.Count - 1].Count, "NeuralNetwork -- Invalid output count");

            Layer outputLayer = mLayers[mLayers.Count - 1];

            // Calculate output layer gradient
            for (var neuron = 0; neuron + 1 < outputLayer.Count; ++neuron)
                outputLayer[neuron].CalculateOutputGradients(targetValues[neuron]);

            // calculate gradients on hidden layers
            for (var layer = mLayers.Count - 2; layer > 0; --layer)
            {
                Layer hiddenLayer = mLayers[layer];
                Layer nextLayer = mLayers[layer + 1];

                for (var neuron = 0; neuron < hiddenLayer.Count; ++neuron)
                    hiddenLayer[neuron].CalculateHiddenGradients(nextLayer);
            }

            // Update Connection Weights
            for (var layer = mLayers.Count - 1; layer > 0; --layer)
            {
                Layer currentLayer = mLayers[layer];
                Layer previousLayer = mLayers[layer - 1];
                for (var neuron = 0; neuron + 1 < currentLayer.Count; ++neuron)
                    currentLayer[neuron].UpdateInputWeights(previousLayer);
            }

        }
        public List<float> GetResults()
        {
            List<float> results = new List<float>();
            Layer outputLayer = mLayers[mLayers.Count - 1];
            for (var neuron = 0; neuron + 1 < outputLayer.Count; ++neuron)
                results.Add(outputLayer[neuron].GetOutputValue());

            return results;
        }

    }
}