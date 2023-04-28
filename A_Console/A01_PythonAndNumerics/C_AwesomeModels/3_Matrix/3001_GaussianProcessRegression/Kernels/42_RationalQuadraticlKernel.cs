using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Squared Exponential (also known as Radial Basis Function Kernel (RBF))
    /// <br/> * without σ² in front
    /// </summary>
    public class RationalQuadraticKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public HyperParameter LengthScale { get; private set; }
        public HyperParameter Alpha { get; private set; }


        // ★★★★★★★★★★★★★★★ inits

        public RationalQuadraticKernel(double lengthScale, double alpha)
        {
            LengthScale = new HyperParameter(nameof(LengthScale), lengthScale, KernelID, false, double.Epsilon, double.MaxValue);
            Alpha = new HyperParameter(nameof(Alpha), alpha, KernelID, false, double.Epsilon, double.MaxValue);
            HyperParameters = new HyperParameter[] { LengthScale, Alpha };

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
                    var to = 1 + 0.5 * Math.Pow(d / LengthScale, 2) / Alpha;
                    K[i1, i2] = Math.Pow(to, -Alpha);
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
                    nameof(Alpha) => CalcKernelGrad_Alpha(X1, X2),
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

        internal DenseMatrix CalcKernelGrad_Alpha(DenseVector X1, DenseVector X2)
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
            return $"RQ({LengthScale.Value:F3}, {Alpha.Value:F3})";
        }

        public override string ToInitialStateString()
        {
            return $"RQ({LengthScale.InitialValue:F3}, {Alpha.InitialValue:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}