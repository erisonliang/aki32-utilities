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

            HyperParameters = new string[]
            {
                nameof(NoiseLambda),
            };

        }


        // ★★★★★★★★★★★★★★★ methods

        internal override DenseMatrix CalcKernel(DenseVector m1, DenseVector m2)
        {
            if (m1.Count == m2.Count)
                return NoiseLambda * DenseMatrix.CreateIdentity(m1.Count);

            return DenseMatrix.Create(m1.Count, m2.Count, 0);
        }

        internal DenseMatrix CalcKernelGrad_NoiseLambda(DenseVector x1, DenseVector x2)
        {
            return DenseMatrix.Create(x1.Count, x2.Count, 0);
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
            return $"δ({NoiseLambda:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
