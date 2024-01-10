using System;
using System.Reflection.Metadata;

namespace Thyme_new
{
    public class NeuralNetwork
    {
        private readonly int inputSize;
        private readonly int hiddenSize;
        private readonly int outputSize;

        private double[,] weightsInputHidden;
        private double[,] weightsHiddenOutput;
        private double[] biasesHidden;
        private double[] biasesOutput;

        private readonly Random random = new Random();

        public NeuralNetwork(int inputSize, int hiddenSize, int outputSize)
        {
            this.inputSize = inputSize;
            this.hiddenSize = hiddenSize;
            this.outputSize = outputSize;

            InitializeWeightsAndBiases();
        }

        private void InitializeWeightsAndBiases()
        {
            // Initialize weights and biases with random values
            weightsInputHidden = InitializeRandomWeights(inputSize, hiddenSize);
            weightsHiddenOutput = InitializeRandomWeights(hiddenSize, outputSize);
            biasesHidden = InitializeRandomBiases(hiddenSize);
            biasesOutput = InitializeRandomBiases(outputSize);
        }

        private double[,] InitializeRandomWeights(int rows, int cols)
        {
            double[,] weights = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    weights[i, j] = random.NextDouble() - 0.5; // Random values between -0.5 and 0.5
                }
            }

            return weights;
        }

        private double[] InitializeRandomBiases(int size)
        {
            double[] biases = new double[size];

            for (int i = 0; i < size; i++)
            {
                biases[i] = random.NextDouble() - 0.5; // Random values between -0.5 and 0.5
            }

            return biases;
        }

        public double[] Predict(double[] input)
        {
            if (input.Length != inputSize)
            {
                throw new ArgumentException("Input size mismatch");
            }

            // Calculate hidden layer outputs
            double[] hiddenOutputs = new double[hiddenSize];
            for (int i = 0; i < hiddenSize; i++)
            {
                double sum = 0;
                for (int j = 0; j < inputSize; j++)
                {
                    sum += input[j] * weightsInputHidden[j, i];
                }

                hiddenOutputs[i] = ThymeHelpers.SIGMOID(sum + biasesHidden[i]);
            }

            // Calculate final outputs
            double[] finalOutputs = new double[outputSize];
            for (int i = 0; i < outputSize; i++)
            {
                double sum = 0;
                for (int j = 0; j < hiddenSize; j++)
                {
                    sum += hiddenOutputs[j] * weightsHiddenOutput[j, i];
                }
                finalOutputs[i] = ThymeHelpers.SIGMOID(sum + biasesOutput[i]);
            }

            return finalOutputs;
        }

        public void Train(double[] input, byte[] target, double learningRate)
        {
            if (input.Length != inputSize || target.Length != outputSize)
            {
                throw new ArgumentException("Input or target size mismatch");
            }

            // Forward pass
            double[] hiddenOutputs = new double[hiddenSize];
            for (int i = 0; i < hiddenSize; i++)
            {
                double sum = 0;
                for (int j = 0; j < inputSize; j++)
                {
                    sum += input[j] * weightsInputHidden[j, i];
                }
                hiddenOutputs[i] = ThymeHelpers.SIGMOID(sum + biasesHidden[i]);
            }

            double[] finalOutputs = new double[outputSize];
            for (int i = 0; i < outputSize; i++)
            {
                double sum = 0;
                for (int j = 0; j < hiddenSize; j++)
                {
                    sum += hiddenOutputs[j] * weightsHiddenOutput[j, i];
                }
                finalOutputs[i] = ThymeHelpers.SIGMOID(sum + biasesOutput[i]);
            }

            //Generate noise array for the purposes of whitenoise as an input
            double a = ThymeHelpers.A(1.0, 3.0);
            double[] signal = new double[outputSize];
            double[] noise = new double[outputSize];
            for (int p = 0; p < noise.Length; p++)
            {
                signal[p] = ThymeHelpers.SIG(finalOutputs, a);
                noise[p] =  ThymeHelpers.C(signal, a);
            }

            //Calculate time taken via the method and see if we can put it to good use in this NN
            //Lets see what happens..............
            double[] T = Thyme.CalculateThyme(noise, finalOutputs, input, a, 3.0);

            // Backward pass
            // Calculate output layer errors with the white noise as an input,
            // finaloutput is target and learning rate is AIHelpers.CalculateMDP
            double[] outputErrors = new double[outputSize];
            for (int i = 0; i < outputSize; i++)
            {
                outputErrors[i] = ThymeHelpers.MSE_full_cost(noise, finalOutputs, learningRate);
            }

            // Update output layer weights and biases
            for (int i = 0; i < hiddenSize; i++)
            {
                for (int j = 0; j < outputSize; j++)
                {
                    weightsHiddenOutput[i, j] += learningRate * outputErrors[j] * hiddenOutputs[i];
                }
            }

            for (int i = 0; i < outputSize; i++)
            {
                biasesOutput[i] += learningRate * outputErrors[i] * T[i];
            }

            // Calculate hidden layer errors
            double[] hiddenErrors = new double[hiddenSize];
            for (int i = 0; i < hiddenSize; i++)
            {
                double sum = 0;
                for (int j = 0; j < outputSize; j++)
                {
                    sum += outputErrors[j] * weightsHiddenOutput[i, j] * T[j];
                }
                hiddenErrors[i] = sum * hiddenOutputs[i] * (1 - hiddenOutputs[i]);
            }

            // Update input layer weights and biases
            for (int i = 0; i < inputSize; i++)
            {
                for (int j = 0; j < hiddenSize; j++)
                {
                    weightsInputHidden[i, j] += learningRate * hiddenErrors[j] * input[i] * T[i];
                }
            }

            for (int i = 0; i < hiddenSize; i++)
            {
                biasesHidden[i] += learningRate * hiddenErrors[i] * T[i];
            }
        }
    }
}
