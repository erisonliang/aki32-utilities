using System.Linq.Expressions;

using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegression
{
    public class WhiteNoiseKernel : IKernel
    {
        public double NoiseLambda { get; set; }


    }
}
