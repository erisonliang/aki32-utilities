using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Periodic Kernel
    /// <br/> * without σ² in front
    /// </summary>
    public class PeriodicKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public HyperParameter LengthScale { get; private set; }
        public HyperParameter P { get; private set; }


        // ★★★★★★★★★★★★★★★ inits

        public PeriodicKernel(double lengthScale, double p)
        {
            LengthScale = new HyperParameter(nameof(LengthScale), lengthScale, KernelID, false, double.Epsilon, double.MaxValue);
            P = new HyperParameter(nameof(P), p, KernelID, false, double.Epsilon, double.MaxValue);
            HyperParameters = new HyperParameter[] { LengthScale, P };

        }


        // ★★★★★★★★★★★★★★★ methods

        internal override DenseMatrix CalcKernel(DenseVector X1, DenseVector X2)
        {
            var K = new DenseMatrix(X1.Count, X2.Count);

            for (int i1 = 0; i1 < X1.Count; i1++)
            {
                for (int i2 = 0; i2 < X2.Count; i2++)
                {
                    var d = Math.Abs(X1[i1] - X2[i2]);
                    var to = -2 * Math.Pow(Math.Sin(Math.PI * d / P) / LengthScale, 2);
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
                    nameof(P) => CalcKernelGrad_P(X1, X2),
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
                    throw new NotImplementedException();



                }
            }

            return K;
        }

        internal DenseMatrix CalcKernelGrad_P(DenseVector X1, DenseVector X2)
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

        public override string ToString()
        {
            return $"P({LengthScale.Value:F3}, {P.Value:F3})";
        }

        public override string ToInitialStateString()
        {
            return $"P({LengthScale.InitialValue:F3}, {P.InitialValue:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}