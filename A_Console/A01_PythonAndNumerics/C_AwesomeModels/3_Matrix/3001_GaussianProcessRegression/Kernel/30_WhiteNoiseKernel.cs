using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// White Noise Kernel
    /// </summary>
    public class WhiteNoiseKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public double NoiseLambda { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public WhiteNoiseKernel(double noiseLambda)
        {
            NoiseLambda = noiseLambda;
        }


        // ★★★★★★★★★★★★★★★ methods

        internal override double CalcKernel(double x1, double x2, bool isSameIndex)
        {
            return isSameIndex ? NoiseLambda : 0;
        }

        internal override double CalcKernelGrad_Parameter1(double x1, double x2, bool isSameIndex)
        {
            return 0;
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
            return $"δ(#{NoiseLambda:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
