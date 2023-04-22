using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Squared Exponential (also known as Radial Basis Function Kernel (RBF))
    /// <br/> * without σ² in front
    /// </summary>
    public class RationalQuadraticKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public double LengthScale { get; set; }
        public double Alpha { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public RationalQuadraticKernel(double lengthScale, double alpha)
        {
            LengthScale = lengthScale;
            Alpha = alpha;
        }


        // ★★★★★★★★★★★★★★★ methods

        internal override double CalcKernel(double x1, double x2, bool isSameIndex)
        {
            var d = x1 - x2;
            var to = 1 + 0.5 * Math.Pow(d / LengthScale, 2) / Alpha;
            return Math.Pow(to, -Alpha);
        }

        internal override double CalcKernelGrad_Parameter1(double x1, double x2, bool isSameIndex)
        {
            throw new NotImplementedException();



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
            return $"RQK({LengthScale:F3}, {Alpha:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}