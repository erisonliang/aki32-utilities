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

        internal override DenseMatrix CalcKernel(DenseVector X1, DenseVector X2)
        {
            return LeftChild.CalcKernel(X1, X2)
                + RightChild.CalcKernel(X1, X2);
        }

        internal override DenseMatrix CalcKernelGrad(DenseVector X1, DenseVector X2, (Guid, string) targetParameter)
        {
            return LeftChild.CalcKernelGrad(X1, X2, targetParameter)
                + RightChild.CalcKernelGrad(X1, X2, targetParameter);
        }

        public override string ToString()
        {
            return $"{LeftChild.ToString()}+{RightChild.ToString()}";
        }


        // ★★★★★★★★★★★★★★★

    }
}
