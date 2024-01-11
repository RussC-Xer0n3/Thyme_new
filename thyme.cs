using System;
using System.Diagnostics;
using System.Security.AccessControl;

namespace Thyme_new
{
    public class Thyme
    {
        public sbyte[] whitenoise;      // Input - a bit stream or an output from socket
        public sbyte[] predicted;       // Input - a bit stream or an output from our NN
        public sbyte[] target;          // Input - what we are expecting or what the AI is expecting
        private double T;               // Time, dependencies are: V, sum PT, FT, DIM, and IN
        private double PT;              // Past Time dependencies are: REMAINDER of future time over the constant divisible over T
        private double FT;              // Future Time dependencies are: Time divided over the constant subtracted from PT
        private double V = v();         // constant based on 3d space time

        private const double Epsilon = 5E-5;

        // Methods
        //Estimated time velocity disregarding distance
        private static double v()
        {
            Stopwatch timera = new Stopwatch();
            Stopwatch timerb = new Stopwatch();

            timera.Start();
            timerb.Start();

            Thread.Sleep(10000);

            timera.Stop();
            timerb.Stop();

            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = timera.Elapsed;
            TimeSpan ts2 = timerb.Elapsed;
            TimeSpan ts3 = ts + ts2;

            //Need to implement timer and disregard distance
            double V = (ts - ts2) / ((ts3) / 2);
            
            Console.WriteLine($"Approximate time velocity being calculated as: {V}");
            return V;
        }

        public static double[] CalculateThyme(double[] whitenoise, double[] predicted, double[] target, double A, double DIM)
        {

            // Implement C:whitenoise, FT:predicted, PT:actual and T:time here
            double[] IN = whitenoise;
            double PT;
            double FT;
            double[] T = null;
            double c = ThymeHelpers.C(whitenoise, A);
            double V = v();

            Console.WriteLine("CalculateThyme being used...");

            if (predicted.Length != target.Length) {
                throw new ArgumentException("Arrays must be of the same length");
            }
             /**
            for (int n = 0; n < predicted.Length; n++) {
                double result = predicted[n] - target[n];
            }
             */
            for (int k = 0; k < predicted.Length; k++) {
                PT = predicted[k] % c / whitenoise[k];
            }

            for (int l = 0; l < whitenoise.Length; l++) {
                FT = target[l] / c - whitenoise[l];
            }

            //V:velocity or time delay, IN:Submissive? DIM:Spherical radius

            for (int h = 0; h < IN.Length; h++) {
                T[h] = Math.Exp(V / (target[h] + predicted[h])) / Math.Exp(Math.Pow(IN[h], DIM) / V);
            }

            Console.WriteLine("T output");
            return T;
        }
    }
}