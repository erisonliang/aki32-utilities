using MathNet.Numerics.LinearAlgebra.Double;

using Microsoft.FSharp.Data.UnitSystems.SI.UnitNames;

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
            // 微分対象の要素が含まれている枝を採用。それ以外は微分すると0になるので無視。
            if (LeftChild.GetAllChildrenKernelsAndSelf().Any(k => k.KernelID == targetParameter.Item1))
                return LeftChild.CalcKernelGrad(X1, X2, targetParameter);
            else
                return RightChild.CalcKernelGrad(X1, X2, targetParameter);

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
            return $"{LeftChild.ToString()}+{RightChild.ToString()}";
        }

        public override string ToInitialStateString()
        {
            return $"{LeftChild.ToInitialStateString()}+{RightChild.ToInitialStateString()}";
        }


        // ★★★★★★★★★★★★★★★

    }
}
