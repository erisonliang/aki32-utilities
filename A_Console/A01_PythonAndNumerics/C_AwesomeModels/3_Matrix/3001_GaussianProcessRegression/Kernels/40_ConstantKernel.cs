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

        internal override DenseMatrix CalcKernel(DenseVector v1, DenseVector v2)
        {
            return DenseMatrix.Create(v1.Count, v2.Count, ConstantWeight);
        }

        internal override DenseMatrix CalcKernelGrad(DenseVector v1, DenseVector v2, (Guid, string) targetParameter)
        {
            if (targetParameter.Item1 == KernelID)
            {
                return targetParameter.Item2 switch
                {
                    nameof(ConstantWeight) => CalcKernelGrad_ConstantWeight(v1, v2),
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
            else
            {
                return CalcKernel(v1, v2);
            }
        }

        internal DenseMatrix CalcKernelGrad_ConstantWeight(DenseVector v1, DenseVector v2)
        {
            return DenseMatrix.Create(v1.Count, v2.Count, 0);
        }

        public override string ToString()
        {
            return $"C({ConstantWeight:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
