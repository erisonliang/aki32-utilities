using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegression
{
    public class WhiteNoiseKernel : IKernel
    {

        // ★★★★★★★★★★★★★★★ props

        public double NoiseLambda { get; set; }


        // ★★★★★★★★★★★★★★★ methods

        internal override double CalcKernel(double x1, double x2)
        {
            throw new NotImplementedException();
        }

        internal override double CalcGradKernel_Parameter1(double x1, double x2)
        {
            throw new NotImplementedException();
        }

        internal override void OptimizeParameters(DenseVector X, DenseVector Y,
          double tryCount = 100,
          double learning_rate = 0.05
          )
        {

        }


        // ★★★★★★★★★★★★★★★

    }
}
