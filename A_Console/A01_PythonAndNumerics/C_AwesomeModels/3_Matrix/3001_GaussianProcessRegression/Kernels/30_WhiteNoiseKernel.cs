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

        internal override DenseMatrix CalcKernel(DenseVector v1, DenseVector v2)
        {
            if (v1.Count == v2.Count)
                return NoiseLambda * DenseMatrix.CreateIdentity(v1.Count);

            return DenseMatrix.Create(v1.Count, v2.Count, 0);
        }

        internal override DenseMatrix CalcKernelGrad(DenseVector v1, DenseVector v2, (Guid, string) targetParameter)
        {
            if (targetParameter.Item1 == KernelID)
            {
                return targetParameter.Item2 switch
                {
                    nameof(NoiseLambda) => CalcKernelGrad_NoiseLambda(v1, v2),
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
            else
            {
                return CalcKernel(v1, v2);
            }
        }

        internal DenseMatrix CalcKernelGrad_NoiseLambda(DenseVector v1, DenseVector v2)
        {
            return DenseMatrix.Create(v1.Count, v2.Count, 0);
        }

        public override string ToString()
        {
            return $"δ({NoiseLambda:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
