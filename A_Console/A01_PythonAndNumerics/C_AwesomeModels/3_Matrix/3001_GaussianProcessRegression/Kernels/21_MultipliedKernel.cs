using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Multiple kernel combination with pointwise multiplication
    /// </summary>
    public class MultiplicationOperationKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public KernelBase LeftChild { get; set; }
        public KernelBase RightChild { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        internal MultiplicationOperationKernel()
        {
        }


        // ★★★★★★★★★★★★★★★ methods

        internal override DenseMatrix CalcKernel(DenseVector m1, DenseVector m2)
        {
            return (DenseMatrix)LeftChild.CalcKernel(m1, m2).PointwiseMultiply(RightChild.CalcKernel(m1, m2));
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
            return $"{LeftChild.ToString()}⊙{RightChild.ToString()}"; // using Ademar product
        }


        // ★★★★★★★★★★★★★★★

    }
}
