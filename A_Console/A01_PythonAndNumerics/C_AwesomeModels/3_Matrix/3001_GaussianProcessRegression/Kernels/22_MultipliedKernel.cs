using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Multiple kernel combination with pointwise multiplication
    /// </summary>
    public class MultiplicationOperationKernel : OperationKernelBase
    {

        // ★★★★★★★★★★★★★★★ inits

        internal MultiplicationOperationKernel()
        {
        }


        // ★★★★★★★★★★★★★★★ methods

        internal override DenseMatrix CalcKernel(DenseVector v1, DenseVector v2)
        {
            return (DenseMatrix)LeftChild.CalcKernel(v1, v2)
                .PointwiseMultiply(RightChild.CalcKernel(v1, v2));
        }

        internal override DenseMatrix CalcKernelGrad(DenseVector v1, DenseVector v2, (Guid, string) targetParameter)
        {
            return (DenseMatrix)LeftChild.CalcKernelGrad(v1, v2, targetParameter)
                .PointwiseMultiply(RightChild.CalcKernelGrad(v1, v2, targetParameter));
        }

        public override string ToString()
        {
            return $"{LeftChild.ToString()}×{RightChild.ToString()}";
        }


        // ★★★★★★★★★★★★★★★

    }
}
