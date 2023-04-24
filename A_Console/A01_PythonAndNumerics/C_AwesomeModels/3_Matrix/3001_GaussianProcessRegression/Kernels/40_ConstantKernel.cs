using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Constant Kernel
    /// </summary>
    public class ConstantKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public double ConstantWeight { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public ConstantKernel(double constantWeight)
        {
            ConstantWeight = constantWeight;

            HyperParameters = new string[]
            {
                nameof(ConstantWeight),
            };

        }


        // ★★★★★★★★★★★★★★★ methods

        internal override DenseMatrix CalcKernel(DenseVector m1, DenseVector m2)
        {
            return DenseMatrix.Create(m1.Count, m2.Count, ConstantWeight);
        }

        internal DenseMatrix CalcKernelGrad_ConstantWeight(DenseVector x1, DenseVector x2)
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
            return $"C({ConstantWeight:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
