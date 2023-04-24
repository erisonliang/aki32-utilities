using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Auto Regressive Kernel
    /// <br/> * without σ² in front
    /// </summary>
    public class AutoRegressiveKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public double Rho { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public AutoRegressiveKernel(double rho)
        {
            Rho = rho;

            HyperParameters = new string[]
            {
                nameof(Rho),
            };

        }


        // ★★★★★★★★★★★★★★★ methods

        internal override DenseMatrix CalcKernel(DenseVector X1, DenseVector X2)
        {
            var K = new DenseMatrix(X1.Count, X2.Count);

            for (int i1 = 0; i1 < X1.Count; i1++)
            {
                for (int i2 = 0; i2 < X2.Count; i2++)
                {
                    throw new NotImplementedException();




                }
            }

            return K;
        }

        internal override DenseMatrix CalcKernelGrad(DenseVector X1, DenseVector X2, (Guid, string) targetParameter)
        {
            if (targetParameter.Item1 == KernelID)
            {
                return targetParameter.Item2 switch
                {
                    nameof(Rho) => CalcKernelGrad_Rho(X1, X2),
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
            else
            {
                return CalcKernel(X1, X2);
            }
        }

        internal DenseMatrix CalcKernelGrad_Rho(DenseVector X1, DenseVector X2)
        {

            var K = new DenseMatrix(X1.Count, X2.Count);

            for (int i1 = 0; i1 < X1.Count; i1++)
            {
                for (int i2 = 0; i2 < X2.Count; i2++)
                {
                    throw new NotImplementedException();



                }
            }

            return K;
        }

        internal override void AddValueToParameter(double addingValue, (Guid, string) targetParameter)
        {
            if (targetParameter.Item1 == KernelID)
            {
                _ = targetParameter.Item2 switch
                {
                    nameof(Rho) => Rho += addingValue,
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
        }

        public override string ToString()
        {
            return $"AR({Rho:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
