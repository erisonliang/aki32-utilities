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

        internal override DenseMatrix CalcKernel(DenseVector X1, DenseVector X2)
        {
            return DenseMatrix.Create(X1.Count, X2.Count, ConstantWeight);
        }

        internal override DenseMatrix CalcKernelGrad(DenseVector X1, DenseVector X2, (Guid, string) targetParameter)
        {
            if (targetParameter.Item1 == KernelID)
            {
                return targetParameter.Item2 switch
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

        internal DenseMatrix CalcKernelGrad_ConstantWeight(DenseVector X1, DenseVector X2)
        {
            return DenseMatrix.Create(X1.Count, X2.Count, 0);
        }

        public override string ToString()
        {
            return $"C({ConstantWeight:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
