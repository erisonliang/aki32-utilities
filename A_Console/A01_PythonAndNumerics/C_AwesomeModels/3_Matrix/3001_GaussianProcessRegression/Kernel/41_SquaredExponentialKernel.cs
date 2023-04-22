using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Squared Exponential (also known as Radial Basis Function Kernel (RBF))
    /// <br/> * without σ² in front
    /// </summary>
    public class SquaredExponentialKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public double LengthScale { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public SquaredExponentialKernel(double lengthScale)
        {
            LengthScale = lengthScale;
        }


        // ★★★★★★★★★★★★★★★ methods

        internal override double CalcKernel(double x1, double x2, bool isSameIndex)
        {
            var d = x1 - x2;
            var to = -0.5 * Math.Pow(d / LengthScale, 2);
            return Math.Exp(to);
        }

        internal override double CalcKernelGrad_Parameter1(double x1, double x2, bool isSameIndex)
        {
            var d = x1 - x2;
            var to = -0.5 * Math.Pow(d / LengthScale, 2);
            return -2 * Math.Pow(LengthScale, -3) * to * Math.Exp(to);
        }

        internal override void OptimizeParameters(DenseVector X, DenseVector Y,
          double tryCount = 100,
          double learning_rate = 0.05
          )
        {
            throw new NotImplementedException();





        }

        public override string ToString()
        {
            return $"SEK({LengthScale:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
