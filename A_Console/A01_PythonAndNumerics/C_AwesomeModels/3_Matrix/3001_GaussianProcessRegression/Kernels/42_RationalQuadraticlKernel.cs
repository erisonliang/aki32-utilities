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

        public double LengthScale { get; set; }
        public double Alpha { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public RationalQuadraticKernel(double lengthScale, double alpha)
        {
            LengthScale = lengthScale;
            Alpha = alpha;

            HyperParameters = new string[]
            {
                nameof(LengthScale),
                nameof(Alpha),
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
                    var to = 1 + 0.5 * Math.Pow(d / LengthScale, 2) / Alpha;
                    K[i1, i2] = Math.Pow(to, -Alpha);
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
                    throw new NotImplementedException();



                }
            }

            return K;
        }

        internal DenseMatrix CalcKernelGrad_Alpha(DenseVector m1, DenseVector m2)
        {

            var K = new DenseMatrix(m1.Count, m2.Count);

            for (int i1 = 0; i1 < m1.Count; i1++)
            {
                for (int i2 = 0; i2 < m2.Count; i2++)
                {
                    throw new NotImplementedException();



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
            return $"RQ({LengthScale:F3}, {Alpha:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}