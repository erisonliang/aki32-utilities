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

        internal override DenseMatrix CalcKernel(DenseVector m1, DenseVector m2)
        {
            var K = new DenseMatrix(m1.Count, m2.Count);

            for (int i1 = 0; i1 < m1.Count; i1++)
            {
                for (int i2 = 0; i2 < m2.Count; i2++)
                {
                    var d = m1[i1] - m2[i2];
                    var to = -0.5 * Math.Pow(d / LengthScale, 2);
                    K[i1, i2] = Math.Exp(to);
                }
            }

            return K;
        }

        internal DenseMatrix CalcKernelGrad_LengthScale(DenseVector m1, DenseVector m2)
        {
            var K = new DenseMatrix(m1.Count, m2.Count);

            for (int i1 = 0; i1 < m1.Count; i1++)
            {
                for (int i2 = 0; i2 < m2.Count; i2++)
                {
                    var d = m1[i1] - m2[i2];
                    var to = -0.5 * Math.Pow(d / LengthScale, 2);
                    K[i1, i2] = -2 * Math.Pow(LengthScale, -3) * to * Math.Exp(to);
                }
            }

            return K;
        }

        internal override void OptimizeParameters(DenseVector X, DenseVector Y,
          double tryCount = 100,
          double learning_rate = 0.05
          )
        {
            throw new NotImplementedException();





        }

        public override string ToString()
        {
            return $"SE({LengthScale:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
