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

        public double LengthScale { get; set; }
        public double P { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public PeriodicKernel(double lengthScale, double p)
        {
            LengthScale = lengthScale;
            P = p;

            HyperParameters = new string[]
            {
                nameof(LengthScale),
                nameof(P),
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
                    var d = Math.Abs(m1[i1] - m2[i2]);
                    var to = -2 * Math.Pow(Math.Sin(Math.PI * d / P) / LengthScale, 2);
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
                    throw new NotImplementedException();



                }
            }

            return K;
        }

        internal DenseMatrix CalcKernelGrad_P(DenseVector m1, DenseVector m2)
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
            return $"P({LengthScale:F3}, {P:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}