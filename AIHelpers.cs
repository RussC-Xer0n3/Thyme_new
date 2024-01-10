using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//in this ai, we try to set the truth as the bias in the reward and probability systems for the mdp
namespace Thyme_new
{ 
    internal class AIHelpers
    {

        //threshold of input data array per index
        public bool Threshold(double[] input)
        {
            bool n = false;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] > 0.5 && input[i] < 0.9999999) { n = true; }
                else if (input[i] < 0.5 && input[i] > 0.0) {  n = false; }
                else { n = false; }
            }
            return n;
        }

        //threshold of a single nth value of input array
        public bool Thresh(double input) 
        {
            bool n = false;

            if (input > 0.5 && input < 0.9999999) { n = true; }
            else if (input < 0.5 && input > 0.0) { n = false; }
            else { n = false; }
            
            return n;
        }

        //coditional statement
        public static int Y(bool condition)
        {
            return condition ? 1 : 0;
        }

        public double CalculateReward(double[] input)
        {
            // Implement your reward calculation logic here
            bool x = Threshold(input);
            // Example: Sum of input values
            return input.Sum() / Y(x);
        }

        public double CalculateProbability(double[] input)
        {
            // Implement your probability calculation logic here
            double k = input.Sum() / input.Length;
            bool x = Thresh(k);
            // Example: Sum of input values divided by bias
            return (k + Y(x)) / input.Sum();
        }

        public double CalculateMDP(double[] input)
        {
            // Implement your MDP calculation logic here

            double Reward = CalculateReward(input);
            double Probability = CalculateProbability(input);
            double k = input.Sum() / input.Length;
            bool x = Thresh(k);
            // Example: Sum of reward and bias divided by probability
            return (Reward + Y(x)) / Probability;
        }

    }
}
