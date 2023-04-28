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

        public bool FixConstantWeight { get; init; } = false;
        private double _ConstantWeight;
        public double ConstantWeight
        {
            get => _ConstantWeight;
            set => _ConstantWeight = MathExtension.Between(MinConstantWeight, value, MaxConstantWeight);
        }
        public double InitialConstantWeight { get; private set; }
        public double MinConstantWeight { get; init; } = double.MinValue;
        public double MaxConstantWeight { get; init; } = double.MaxValue;


        // ★★★★★★★★★★★★★★★ inits

        public ConstantKernel(double constantWeight)
        {
            InitialConstantWeight = constantWeight;
            ConstantWeight = constantWeight;

            HyperParameters = new string[]
            {
                nameof(ConstantWeight),
            };
        }


        // ★★★★★★★★★★★★★★★ methods

        /// <summary>
        /// K = σ²
        /// </summary>
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

        /// <summary>
        /// dK/dσ = 2σ
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="X2"></param>
        /// <returns></returns>
        internal DenseMatrix CalcKernelGrad_ConstantWeight(DenseVector X1, DenseVector X2)
        {
            if (FixConstantWeight)
                return DenseMatrix.Create(X1.Count, X2.Count, 0);
            return DenseMatrix.Create(X1.Count, X2.Count, 2 * Math.Sqrt(ConstantWeight));
        }

        internal override double? GetParameterValue((Guid, string) targetParameter)
        {
            if (targetParameter.Item1 == KernelID)
            {
                return targetParameter.Item2 switch
                {
                    nameof(ConstantWeight) => ConstantWeight,
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
            else
            {
                return null;
            }
        }
        internal override void SetParameterValue((Guid, string) targetParameter, double settingValue)
        {
            if (targetParameter.Item1 == KernelID)
            {
                _ = targetParameter.Item2 switch
                {
                    nameof(ConstantWeight) => ConstantWeight = settingValue,
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
        }

        public override string ToString()
        {
            return $"C({ConstantWeight:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
