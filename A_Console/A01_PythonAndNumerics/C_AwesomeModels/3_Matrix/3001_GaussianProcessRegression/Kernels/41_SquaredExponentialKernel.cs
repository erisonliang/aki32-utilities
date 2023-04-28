using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Represent all kernels known as followings.
    /// <br/>
    /// <br/>- SE (Squared Exponential Kernel)
    /// <br/>- RBF (Radial Basis Function Kernel)
    /// <br/>- EQ (Exponentiated Quadratic Kernel)
    /// <br/>- G(Gaussian Kernel)
    /// <br/>
    /// <br/> * without σ² in front
    /// </summary>
    public class SquaredExponentialKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public HyperParameter LengthScale { get; private set; }


        // ★★★★★★★★★★★★★★★ inits

        public SquaredExponentialKernel(double lengthScale)
        {
            LengthScale = new HyperParameter(nameof(LengthScale), lengthScale, KernelID, false, double.Epsilon, double.MaxValue);

            HyperParameters = new HyperParameter[] { LengthScale };

        }


        // ★★★★★★★★★★★★★★★ methods

        internal override DenseMatrix CalcKernel(DenseVector X1, DenseVector X2)
        {
            var K = new DenseMatrix(X1.Count, X2.Count);

            for (int i1 = 0; i1 < X1.Count; i1++)
            {
                for (int i2 = 0; i2 < X2.Count; i2++)
                {
                    var d = X1[i1] - X2[i2];
                    var to = -0.5 * Math.Pow(d / LengthScale, 2);
                    K[i1, i2] = Math.Exp(to);
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
                    nameof(LengthScale) => CalcKernelGrad_LengthScale(X1, X2),
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
            else
            {
                return CalcKernel(X1, X2);
            }
        }

        internal DenseMatrix CalcKernelGrad_LengthScale(DenseVector X1, DenseVector X2)
        {
            var K = new DenseMatrix(X1.Count, X2.Count);

            for (int i1 = 0; i1 < X1.Count; i1++)
            {
                for (int i2 = 0; i2 < X2.Count; i2++)
                {
                    var d = X1[i1] - X2[i2];
                    var to = -0.5 * Math.Pow(d / LengthScale, 2);
                    K[i1, i2] = -2 * Math.Pow(LengthScale, -3) * to * Math.Exp(to);
                }
            }

            return K;
        }

        public override string ToString()
        {
            return $"SE({LengthScale.Value:F3})";
        }

        public override string ToInitialStateString()
        {
            return $"SE({LengthScale.InitialValue:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
