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

        internal override DenseMatrix CalcKernel(DenseVector X1, DenseVector X2)
        {
            return (DenseMatrix)LeftChild.CalcKernel(X1, X2)
                .PointwiseMultiply(RightChild.CalcKernel(X1, X2));
        }

        internal override DenseMatrix CalcKernelGrad(DenseVector X1, DenseVector X2, (Guid, string) targetParameter)
        {
            return (DenseMatrix)LeftChild.CalcKernelGrad(X1, X2, targetParameter)
                .PointwiseMultiply(RightChild.CalcKernelGrad(X1, X2, targetParameter));
        }

        internal override double? GetParameterValue((Guid, string) targetParameter)
        {
            return LeftChild.GetParameterValue(targetParameter)
                ?? RightChild.GetParameterValue(targetParameter);
        }
        internal override void SetParameterValue((Guid, string) targetParameter, double settingValue)
        {
            LeftChild.SetParameterValue(targetParameter, settingValue);
            RightChild.SetParameterValue(targetParameter, settingValue);
        }

        public override string ToString()
        {
            return $"{LeftChild.ToString()}×{RightChild.ToString()}";
        }

        public override string ToInitialStateString()
        {
            return $"{LeftChild.ToInitialStateString()}×{RightChild.ToInitialStateString()}";
        }


        // ★★★★★★★★★★★★★★★

    }
}
