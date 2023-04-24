using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Multiple kernel combination with addition
    /// </summary>
    public class AdditionOperationKernel : OperationKernelBase
    {

        // ★★★★★★★★★★★★★★★ inits

        internal AdditionOperationKernel()
        {

        }


        // ★★★★★★★★★★★★★★★ methods

        internal override DenseMatrix CalcKernel(DenseVector v1, DenseVector m2)
        {
            return LeftChild.CalcKernel(v1, m2)
                + RightChild.CalcKernel(v1, m2);
        }

        internal override DenseMatrix CalcKernelGrad(DenseVector v1, DenseVector v2, (Guid, string) targetParameter)
        {
            return (DenseMatrix)LeftChild.CalcKernelGrad(v1, v2, targetParameter)
                + RightChild.CalcKernelGrad(v1, v2, targetParameter);
        }

        public override string ToString()
        {
            return $"{LeftChild.ToString()}+{RightChild.ToString()}";
        }


        // ★★★★★★★★★★★★★★★

    }
}
