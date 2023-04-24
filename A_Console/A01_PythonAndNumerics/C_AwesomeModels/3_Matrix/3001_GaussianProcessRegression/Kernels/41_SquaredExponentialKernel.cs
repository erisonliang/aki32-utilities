using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Squared Exponential (also known as Radial Basis Function Kernel (RBF))
    /// <br/> * without σ² in front
    /// </summary>
    public class SquaredExponentialKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public double LengthScale { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public SquaredExponentialKernel(double lengthScale)
        {
            LengthScale = lengthScale;

            HyperParameters = new string[]
            {
                nameof(LengthScale),
            };

        }


        // ★★★★★★★★★★★★★★★ methods

        internal override DenseMatrix CalcKernel(DenseVector v1, DenseVector v2)
        {
            var K = new DenseMatrix(v1.Count, v2.Count);

            for (int i1 = 0; i1 < v1.Count; i1++)
            {
                for (int i2 = 0; i2 < v2.Count; i2++)
                {
                    var d = v1[i1] - v2[i2];
                    var to = -0.5 * Math.Pow(d / LengthScale, 2);
                    K[i1, i2] = Math.Exp(to);
                }
            }

            return K;
        }

        internal override DenseMatrix CalcKernelGrad(DenseVector v1, DenseVector v2, (Guid, string) targetParameter)
        {
            if (targetParameter.Item1 == KernelID)
            {
                return targetParameter.Item2 switch
                {
                    nameof(LengthScale) => CalcKernelGrad_LengthScale(v1, v2),
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
            else
            {
                return CalcKernel(v1, v2);
            }
        }

        internal DenseMatrix CalcKernelGrad_LengthScale(DenseVector v1, DenseVector v2)
        {
            var K = new DenseMatrix(v1.Count, v2.Count);

            for (int i1 = 0; i1 < v1.Count; i1++)
            {
                for (int i2 = 0; i2 < v2.Count; i2++)
                {
                    var d = v1[i1] - v2[i2];
                    var to = -0.5 * Math.Pow(d / LengthScale, 2);
                    K[i1, i2] = -2 * Math.Pow(LengthScale, -3) * to * Math.Exp(to);
                }
            }

            return K;
        }

        public override string ToString()
        {
            return $"SE({LengthScale:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
