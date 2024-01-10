using System;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Threading;
using Thyme_new;

internal class ThymeHelpers
{
    
    // Methods
    internal static double A(double rad, double dim) => Math.Pow((Math.PI * rad), dim);
    // Constant SIGNAL for dependencies of A and IN
    internal static double C(double[] whitenoise, double A)
    {
        double c = 0;

        for (int y = 0; y < whitenoise.Length; y++)
        {
            try
            {
                c = Math.Exp(Math.Pow(A, Math.Exp(whitenoise[y]) / Math.Exp(-whitenoise[y])));
            }
            catch (NullReferenceException n)
            {
                throw new ArgumentNullException("n");
            }
            catch (DivideByZeroException z)
            {
                throw new DivideByZeroException("z");
            }
            catch (Exception e)
            {
                throw new Exception("e");
            }
            finally
            {
                whitenoise[y] = 0.01;
                c = Math.Exp(Math.Pow(A, Math.Exp(whitenoise[y]) / Math.Exp(-whitenoise[y])));
            }
        }
        return c;
    }

    //Generic sigmoid standard
    internal static double SIGMOID(double o) => 1.0 / (1.0 + Math.Exp(-o));

    // Generic sigmoid function modified for a unique signal implements a spherical constant per value of whitenoise
    internal static double SIG(double[] whitenoise, double A) { double c = C(whitenoise, A); return 1.0 / (-1 * c); }

    /**
     * COST should factor in the outputs from the neural network compared to the
     * outputs from satcomm rx where target is the same rx as tx and predicted is
     * rx from NN and AI.
     * partial derivatives (scope) should be same size arrays
     */
    internal static double Cost(double[] predicted, double[] target)
    {
        // Mean Squared Error
        if (predicted.Length != target.Length)
        {
            throw new ArgumentException("Arrays must be of the same length");
        }

        double sum = 0;

        for (int i = 0; i < predicted.Length; i++)
        {
            try
            {
                sum += Math.Pow(predicted[i] - target[i], 2);
            }
            catch (NullReferenceException n) 
            {
                throw new NullReferenceException("n");
            }
            finally
            {
                sum = sum;
            }
        }

        return (sum / predicted.Length);
    }

    //Gradient of descent
    internal static double GradientDescent(double[] predicted, double[] target, double Epsilon)
    {
        if (predicted.Length != target.Length)
        {
            throw new ArgumentException("Arrays must be of the same length");
        }

        double cost = Cost(predicted, target);

        return (cost - Epsilon) * 2;
    }

    //Mean Squared error full cost
    internal static double MSE_full_cost(double[] predicted, double[] target, double Epsilon)
    {

        double result = 0;

        // Mean Squared Error result
        if (predicted.Length != target.Length)
        {
            throw new ArgumentException("Arrays must be of the same length");
        }

        double cost2 = Cost(predicted, target);

        double r = 0;

        for (int u = 0; u < predicted.Length; u++)
        {
            try
            {
                r = ((predicted[u] - target[u]) / cost2) - GradientDescent(predicted, target, Epsilon);
            }
            catch (NullReferenceException n)
            {
                throw new ArgumentNullException("n");
            }
            catch (DivideByZeroException z)
            {
                throw new DivideByZeroException("z");
            }
            catch (Exception e)
            {
                throw new Exception("e");
            }
            finally 
            {     
                result += r; 
            }
        }

        result /= predicted.Length;
        return result;
    }
}