using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Linear Kernel
    /// <br/> * without σ² in front
    /// </summary>
    public class LinearKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public HyperParameter C { get; private set; }


        // ★★★★★★★★★★★★★★★ inits

        public LinearKernel(double c)
        {
            C = new HyperParameter(nameof(C), c, KernelID, false, double.MinValue, double.MaxValue);

            HyperParameters = new HyperParameter[] { C };

        }


        // ★★★★★★★★★★★★★★★ methods

        internal override DenseMatrix CalcKernel(DenseVector X1, DenseVector X2)
        {
            var K = new DenseMatrix(X1.Count, X2.Count);

            for (int i1 = 0; i1 < X1.Count; i1++)
            {
                for (int i2 = 0; i2 < X2.Count; i2++)
                {
                    K[i1, i2] = (X1[i1] - C) * (X2[i2] - C);
                }
            }

            return K;
        }

        internal override DenseMatrix CalcKernelGrad(DenseVector X1, DenseVector X2, HyperParameter targetParameter)
        {
            if (targetParameter.ParentKernelID == KernelID)
            {
                return targetParameter.Name switch
                {
                    nameof(C) => CalcKernelGrad_C(X1, X2),
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
            else
            {
                return CalcKernel(X1, X2);
            }
        }

        internal DenseMatrix CalcKernelGrad_C(DenseVector X1, DenseVector X2)
        {

            var K = new DenseMatrix(X1.Count, X2.Count);

            for (int i1 = 0; i1 < X1.Count; i1++)
            {
                for (int i2 = 0; i2 < X2.Count; i2++)
                {
                    K[i1, i2] = -X1[i1] - X2[i2] - 2 * C;
                }
            }

            return K;
        }

        public override string ToString()
        {
            return $"L({C.Value:F3})";
        }

        public override string ToInitialStateString()
        {
            return $"L({C.InitialValue:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
