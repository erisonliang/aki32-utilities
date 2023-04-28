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

        public HyperParameter ConstantWeight { get; private set; }


        // ★★★★★★★★★★★★★★★ inits

        public ConstantKernel(double constantWeight, bool fixConstantWeight = false)
        {
            ConstantWeight = new HyperParameter(nameof(ConstantWeight), constantWeight, KernelID, fixConstantWeight, double.MinValue, double.MaxValue);

            HyperParameters = new HyperParameter[] { ConstantWeight };

        }


        // ★★★★★★★★★★★★★★★ methods

        /// <summary>
        /// K = σ²
        /// </summary>
        internal override DenseMatrix CalcKernel(DenseVector X1, DenseVector X2)
        {
            return DenseMatrix.Create(X1.Count, X2.Count, ConstantWeight);
        }

        internal override DenseMatrix CalcKernelGrad(DenseVector X1, DenseVector X2, HyperParameter targetParameter)
        {
            if (targetParameter.ParentKernelID == KernelID)
            {
                return targetParameter.Name switch
                {
                    nameof(ConstantWeight) => CalcKernelGrad_ConstantWeight(X1, X2),
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
            else
            {
                return CalcKernel(X1, X2);
            }
        }

        /// <summary>
        /// dK/dσ = 2σ
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="X2"></param>
        /// <returns></returns>
        internal DenseMatrix CalcKernelGrad_ConstantWeight(DenseVector X1, DenseVector X2)
        {
            if (ConstantWeight.IsFixed)
                return DenseMatrix.Create(X1.Count, X2.Count, 0);
            return DenseMatrix.Create(X1.Count, X2.Count, 2 * Math.Sqrt(ConstantWeight));
        }

        public override string ToString()
        {
            return $"C({ConstantWeight.Value:F3})";
        }

        public override string ToInitialStateString()
        {
            return $"C({ConstantWeight.InitialValue:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
